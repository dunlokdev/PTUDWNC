using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using SlugGenerator;
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
        private readonly IMemoryCache _memoryCache;

        public BlogRepository(BlogDbContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
        }

        
        public async Task<IList<Post>> GetPopularArticleAsync(int numPosts, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Post>()
                .Include(x => x.Author)
                .Include(x => x.Category)
                .OrderByDescending(p => p.ViewCount)
                .Take(numPosts)
                .ToListAsync(cancellationToken);
        }

        public async Task<Post> GetPostAsync(int year, int month, string slug, CancellationToken cancellationToken = default)
        {
            IQueryable<Post> postsQuery = _context.Set<Post>()
                .Include(x => x.Category)
                .Include(x => x.Author)
                .Include(x => x.Tags);

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

        public async Task IncreaseViewCountAsync(int postId, CancellationToken cancellationToken = default)
        {
            await _context.Set<Post>()
                .Where(x => x.Id == postId)
                .ExecuteUpdateAsync(p => p.SetProperty(x => x.ViewCount, x => x.ViewCount + 1), cancellationToken);
        }

        public async Task<bool> IsPostSlugExistedAsync(int postId, string slug, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Post>()
                .AnyAsync(x => x.Id != postId && x.UrlSlug == slug, cancellationToken);
        }

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

        public async Task<Tag> FindTagBySlugAsync(string slug, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Tag>()
                .Where(x => x.UrlSlug == slug)
                .FirstOrDefaultAsync(cancellationToken);
        }

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
       
        public Task<object> CountByMostRecentMonthAsync(int month, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<Post> GetPostByIdAsync(
     int postId, bool includeDetails = false,
     CancellationToken cancellationToken = default)
        {
            if (!includeDetails)
            {
                return await _context.Set<Post>().FindAsync(postId);
            }

            return await _context.Set<Post>()
                .Include(x => x.Category)
                .Include(x => x.Author)
                .Include(x => x.Tags)
                .FirstOrDefaultAsync(x => x.Id == postId, cancellationToken);
        }
        public async Task<bool> AddOrUpdatePostAsync(Post post, CancellationToken cancellationToken = default)
        {
            _context.Entry(post).State = post.Id == 0 ? EntityState.Added : EntityState.Modified;
            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task ChangeStatusPublishedOfPostAsync(int id, CancellationToken cancellationToken = default)
        {
            var post = await _context.Set<Post>().FindAsync(id);

            post.Published = !post.Published;
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<IList<Post>> GetPostsByQualAsync(int num, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Post>()
                .Include(a => a.Author)
                .Include(c => c.Category)
                .OrderBy(x => x.Id)
                .Take(num)
                .ToListAsync(cancellationToken);
        }

        public async Task<IList<Post>> FindPostByPostQueryAsync(PostQuery query, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Post>()
                .Include(a => a.Author)
                .Include(c => c.Category)
                .Include(t => t.Tags)
                .Where(
                    p => p.AuthorId == query.AuthorId
                    || p.CategoryId == query.CategoryId
                    || p.Category.UrlSlug.Equals(query.CategorySlug)
                    || p.PostedDate.Month == query.Month
                    || p.PostedDate.Year == query.Year
                    || p.Tags.Any(tagName => tagName.Name.Equals(query.Tag)))
                    .ToListAsync(cancellationToken);
        }

        public async Task<int> CountPostsOfPostQueryAsync(PostQuery query, CancellationToken cancellationToken = default)
        {
            var posts = await FindPostByPostQueryAsync(query);
            return posts.Count();
        }

        public async Task<IPagedList<Post>> GetPagedPostByPostQueryAsync(IPagingParams pagingParams, PostQuery query, CancellationToken cancellationToken = default)
        {
            var posts = _context.Set<Post>()
                .Include(a => a.Author)
                .Include(c => c.Category)
                .Include(t => t.Tags)
                .Where(
                    p => p.Published == query.PublishedOnly && p.AuthorId == query.AuthorId
                    || p.CategoryId == query.CategoryId
                    || p.Category.UrlSlug.Equals(query.CategorySlug)
                    || p.PostedDate.Month == query.Month
                    || p.PostedDate.Year == query.Year
                    || p.Tags.Any(tagName => tagName.Name.Equals(query.Tag)));

            return await posts.ToPagedListAsync(pagingParams, cancellationToken);

            // NOT WORKING
            //var posts = await FindPostByPostQueryAsync(query);
            //return await posts.AsQueryable<Post>().ToPagedListAsync(pagingParams, cancellationToken);
        }

        private IQueryable<Post> FilterPosts(PostQuery condition)
        {
            IQueryable<Post> posts = _context.Set<Post>()
                .Include(x => x.Category)
                .Include(x => x.Author)
                .Include(x => x.Tags);

            if (condition.PublishedOnly)
            {
                posts = posts.Where(x => x.Published);
            }

            if (condition.NotPublished)
            {
                posts = posts.Where(x => !x.Published);
            }

            if (condition.CategoryId > 0)
            {
                posts = posts.Where(x => x.CategoryId == condition.CategoryId);
            }

            if (!string.IsNullOrWhiteSpace(condition.CategorySlug))
            {
                posts = posts.Where(x => x.Category.UrlSlug == condition.CategorySlug);
            }

            if (condition.AuthorId > 0)
            {
                posts = posts.Where(x => x.AuthorId == condition.AuthorId);
            }

            if (!string.IsNullOrWhiteSpace(condition.AuthorSlug))
            {
                posts = posts.Where(x => x.Author.UrlSlug == condition.AuthorSlug);
            }

            if (!string.IsNullOrWhiteSpace(condition.TagSlug))
            {
                posts = posts.Where(x => x.Tags.Any(t => t.UrlSlug == condition.TagSlug));
            }

            if (!string.IsNullOrWhiteSpace(condition.Tag))
            {
                posts = posts.Where(x => x.Tags.Any(t => t.UrlSlug == condition.Tag));
            }

            if (!string.IsNullOrWhiteSpace(condition.KeyWord))
            {
                posts = posts.Where(x => x.Title.Contains(condition.KeyWord) ||
                                         x.ShortDescription.Contains(condition.KeyWord) ||
                                         x.Description.Contains(condition.KeyWord) ||
                                         x.Category.Name.Contains(condition.KeyWord) ||
                                         x.Tags.Any(t => t.Name.Contains(condition.KeyWord)));
            }

            if (condition.Year > 0)
            {
                posts = posts.Where(x => x.PostedDate.Year == condition.Year);
            }

            if (condition.Month > 0)
            {
                posts = posts.Where(x => x.PostedDate.Month == condition.Month);
            }

            if (!string.IsNullOrWhiteSpace(condition.TitleSlug))
            {
                posts = posts.Where(x => x.UrlSlug == condition.TitleSlug);
            }

            return posts;
        }

        public async Task<IPagedList<Post>> GetPagedPostsAsync(
        PostQuery condition,
        int pageNumber = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default)
        {
            return await FilterPosts(condition).ToPagedListAsync(
                pageNumber, pageSize,
                nameof(Post.PostedDate), "DESC",
                cancellationToken);
        }

        public async Task<IList<TagItem>> GetListTagItemAsync(TagQuery condition, CancellationToken cancellationToken = default)
        {
            var tagItems = _context.Set<Tag>().Select(x => new TagItem()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                UrlSlug = x.UrlSlug,
                PostCount = x.Posts.Count(p => p.Published)
            });

            if (condition != null)
            {
                if (!string.IsNullOrWhiteSpace(condition.KeyWord))
                {
                    tagItems = tagItems.Where(x => x.Name.Contains(condition.KeyWord) ||
                                              x.Description.Contains(condition.KeyWord));
                }
            }

            return await tagItems.ToListAsync(cancellationToken);
        }

        public async Task<Post> CreateOrUpdatePostAsync(Post post, IEnumerable<string> tags, CancellationToken cancellationToken = default)
        {
            if (post.Id > 0)
            {
                await _context.Entry(post).Collection(x => x.Tags).LoadAsync(cancellationToken);
            }
            else
            {
                post.Tags = new List<Tag>();
            }

            var validTags = tags.Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => new
                {
                    Name = x,
                    Slug = x.GenerateSlug()
                })
                .GroupBy(x => x.Slug)
                .ToDictionary(
                g => g.Key,
                g => g.First().Name);


            foreach (var kv in validTags)
            {
                if (post.Tags.Any(x => string.Compare(x.UrlSlug, kv.Key, StringComparison.InvariantCultureIgnoreCase) == 0)) continue;

                var tag = await FindTagBySlugAsync(kv.Key, cancellationToken) ?? new Tag()
                {
                    Name = kv.Value,
                    Description = kv.Value,
                    UrlSlug = kv.Key
                };

                post.Tags.Add(tag);
            }

            post.Tags = post.Tags.Where(t => validTags.ContainsKey(t.UrlSlug)).ToList();

            if (post.Id > 0)
                _context.Update(post);
            else
                _context.Add(post);

            await _context.SaveChangesAsync(cancellationToken);

            return post;
        }

        public async Task<bool> DeletePostByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Post>()
                .Where(t => t.Id == id).ExecuteDeleteAsync(cancellationToken) > 0;
        }


        public async Task<bool> IsTagSlugExistedAsync(int id, string slug, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Tag>().AnyAsync(x => x.Id != id && x.UrlSlug == slug, cancellationToken);
        }

        public async Task<Tag> FindTagById(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Tag>().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> AddOrEditTagAsync(Tag tag, CancellationToken cancellationToken = default)
        {
            _context.Entry(tag).State = tag.Id == 0 ? EntityState.Added : EntityState.Modified;
            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<IPagedList<T>> GetPagedPostsByQueryAsync<T>(Func<IQueryable<Post>, IQueryable<T>> mapper, PostQuery query, IPagingParams pagingParams, CancellationToken cancellationToken = default)
        {
            return await mapper(FilterPosts(query).AsNoTracking()).ToPagedListAsync(pagingParams, cancellationToken);
        }
    }
}
