using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
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
    public class CategoryRepository : ICategoryRepository
    {
        private readonly BlogDbContext _context;
        private readonly IMemoryCache _memoryCache;

        public CategoryRepository(BlogDbContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
        }

        public async Task<IPagedList<Category>> GetCategoriesByQuery(CategoryQuery condition, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
        {
            return await FilterCategories(condition).ToPagedListAsync(
                pageNumber, pageSize,
                nameof(Category.Name), "DESC",
                cancellationToken);
        }

        private IQueryable<Category> FilterCategories(CategoryQuery condition)
        {
            IQueryable<Category> categories = _context.Set<Category>();


            if (condition.ShowOnMenu)
            {
                categories = categories.Where(x => x.ShowOnMenu);
            }

            if (!string.IsNullOrWhiteSpace(condition.KeyWord))
            {
                categories = categories.Where(x => x.Name.Contains(condition.KeyWord) ||
                                         x.Description.Contains(condition.KeyWord));
            }
            return categories;
        }

        public async Task<bool> IsCategorySlugExistedAsync(int id, string slug, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Category>()
                .AnyAsync(x => x.Id != id && x.UrlSlug == slug, cancellationToken);
        }
        public async Task<Category> GetCachedCategoryByIdAsync(int categoryId)
        {
            return await _memoryCache.GetOrCreateAsync(
                $"category.by-id.{categoryId}",
                async (entry) =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                    return await GetCategoryByIdAsync(categoryId);
                });
        }

        public async Task<Category> GetCategoryByIdAsync(int categoryId)
        {
            return await _context.Set<Category>().FindAsync(categoryId);
        }

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

        public async Task<IPagedList<CategoryItem>> GetPagedCategoriesAsync(IPagingParams pagingParams, string name = null, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Category>()
                .Include(p => p.Posts)
                .AsNoTracking()
                .WhereIf(!string.IsNullOrWhiteSpace(name), x => x.Name.Contains(name))
                .Select(x => new CategoryItem
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlSlug = x.UrlSlug,
                    Description = x.Description,
                    ShowOnMenu = x.ShowOnMenu,
                    PostCount = x.Posts.Count(p => p.Published)
                })
                .ToPagedListAsync(pagingParams, cancellationToken);
        }
    }
}
