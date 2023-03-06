using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;
using TatBlog.Services.Extentions;

namespace TatBlog.Services.Blogs
{
    public class BlogRepository : IBlogRepository
    {
        private readonly BlogDbContext _context;

        public BlogRepository(BlogDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Lấy danh sách chuyên mục và số lượng bài viết nằm thuộc từng chuyên mục chủ đề
        /// </summary>
        /// <param name="showOnMenu"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IList<CategoryItem>> GetCategoriesAsync(bool showOnMenu = false, CancellationToken cancellationToken = default)
        {
            IQueryable<Category> categories = _context.Set<Category>();

            if (showOnMenu)
            {
                categories = categories.Where(x => x.ShowOnMenu);
            }

            return await categories
                .OrderBy(x => x.Name)
                .Select(x => new CategoryItem()
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlSlug = x.UrlSlug,
                    Description = x.Description,
                    ShowOnMenu = x.ShowOnMenu,
                    PostCount = x.Posts.Count(p => p.Published)
                })
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Tìm Top N bài viết phổ biến được nhiều người xem nhất
        /// </summary>
        /// <param name="numPosts"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IList<Post>> GetPopularArticleAsync(int numPosts, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Post>()
                .Include(x => x.Author)
                .Include(x => x.Category)
                .OrderByDescending(p => p.ViewCount)
                .Take(numPosts)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Tìm bài viết có tên định danh là 'slug' và được đăng vào tháng 'month' năm 'year'
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="slug"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Post> GetPostAsync(int year, int month, string slug, CancellationToken cancellationToken = default)
        {
            IQueryable<Post> postsQuery = _context.Set<Post>()
                .Include(x => x.Category)
                .Include(x => x.Author);

            if (year > 0)
            {
                postsQuery = postsQuery.Where(x => x.PostedDate.Year == year);
            }

            if (month > 0)
            {
                postsQuery = postsQuery.Where(x => x.PostedDate.Month == month);
            }

            if (!string.IsNullOrWhiteSpace(slug))
            {
                postsQuery = postsQuery.Where(x => x.UrlSlug == slug);
            }

            return await postsQuery.FirstOrDefaultAsync(cancellationToken);
        }

        /// <summary>
        /// Tăng số lượt xem của một bài viết
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task IncreaseViewCountAsync(int postId, CancellationToken cancellationToken = default)
        {
            await _context.Set<Post>()
                .Where(x => x.Id == postId)
                .ExecuteUpdateAsync(p => p.SetProperty(x => x.ViewCount, x => x.ViewCount + 1), cancellationToken);
        }

        /// <summary>
        /// Kiểm tra xem tên định danh của một bài viết đã có hay chưa
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="slug"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> IsPostSlugExistedAsync(int postId, string slug, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Post>()
                .AnyAsync(x => x.Id != postId && x.UrlSlug == slug, cancellationToken);
        }

        /// <summary>
        /// Lấy danh sách từ khoá/thẻ và phân trang theo
        /// các tham số pagingParams
        /// </summary>
        /// <param name="pagingParams"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IPagedList<TagItem>> GetPagedTagsAsync(IPagingParams pagingParams, CancellationToken cancellationToken = default)
        {
            var tagQuery = _context.Set<Tag>()
                .Select(x => new TagItem()
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlSlug = x.UrlSlug,
                    Description = x.Description,
                    PostCount = x.Posts.Count(p => p.Published)
                });

            return await tagQuery.ToPagedListAsync(pagingParams, cancellationToken);
        }


        /// <summary>
        /// Tìm một thẻ (Tag) theo tên định danh (slug)
        /// </summary>
        /// <param name="slug"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Tag> FindTagBySlugAsync(string slug, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Tag>()
                .Where(x => x.UrlSlug == slug)
                .FirstOrDefaultAsync(cancellationToken);
        }

        /// <summary>
        /// Lấy danh sách tất cả các thẻ (Tag) kèm theo số bài viết chứa thẻ đó. Kết quả trả về kiểu IList<TagItem>
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IList<TagItem>> FindTagItemSlugAsync(CancellationToken cancellationToken = default)
        {
            var query = _context.Set<Tag>()
                       .Select(x => new TagItem()
                       {
                           Id = x.Id,
                           Name = x.Name,
                           UrlSlug = x.UrlSlug,
                           Description = x.Description,
                           PostCount = x.Posts.Count(p => p.Published)
                       });
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<bool> DeleteTagByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            //var tag = await _context.Set<Tag>()
            //    .Include(t => t.Posts)
            //    .Where(x => x.Id == id)
            //    .FirstOrDefaultAsync(cancellationToken);

            //_context.Set<Tag>().Remove(tag); 
            //_context.SaveChanges();


            return await _context.Set<Tag>()
                .Where(t => t.Id == id).ExecuteDeleteAsync(cancellationToken) > 0;
        }

        public async Task<Category> FindCategoryBySlugAsync(string slug, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Category>()
                    .FirstOrDefaultAsync(c => c.UrlSlug.Equals(slug), cancellationToken);
        }

        public async Task<Category> FindCategoryByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Category>()
                .FindAsync(id, cancellationToken);
        }

        public async Task<bool> AddOrEditCategoryAsync(Category newCategory, CancellationToken cancellationToken = default)
        {
            _context.Entry(newCategory).State = newCategory.Id == 0 ? EntityState.Added : EntityState.Modified;
            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<bool> DeleteCategoryByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Category>()
                .Where(c => c.Id == id).ExecuteDeleteAsync(cancellationToken) > 0;
        }

        public async Task<bool> IsSlugOfCategoryExist(string slug, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Category>().AnyAsync(c => c.UrlSlug.Equals(slug), cancellationToken);
        }

        public async Task<IPagedList<CategoryItem>> GetPagedCategoriesAsync(IPagingParams pagingParams, CancellationToken cancellationToken = default)
        {
            var categoriesQuery = _context.Set<Category>()
                .Select(x => new CategoryItem()
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlSlug = x.UrlSlug,
                    Description = x.Description,
                    ShowOnMenu = x.ShowOnMenu,
                    PostCount = x.Posts.Count(p => p.Published)
                });

            return await categoriesQuery.ToPagedListAsync(pagingParams, cancellationToken);
        }

        public Task<object> CountByMostRecentMonthAsync(int month, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<Post> FindPostByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Post>().FindAsync(id, cancellationToken);
        }

        public async Task<bool> AddOrUpdatePostAsync(Post post, CancellationToken cancellationToken = default)
        {
            _context.Entry(post).State = post.Id == 0 ? EntityState.Added : EntityState.Modified;
            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}
