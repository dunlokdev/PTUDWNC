using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;

namespace TatBlog.Services.Blogs
{
    public interface ICategoryRepository
    {
        Task<Category> GetCategoryByIdAsync(int categoryId);
        Task<Category> GetCachedCategoryByIdAsync(int categoryId);
        Task<bool> IsCategorySlugExistedAsync(int id, string slug, CancellationToken cancellationToken = default);
        Task<IPagedList<Category>> GetCategoriesByQuery(
        CategoryQuery condition,
        int pageNumber = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default);

        /// <summary>
        /// Tìm một chuyên mục (Category) theo tên định danh (slug).
        /// </summary>
        /// <param name="slug"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Category> FindCategoryBySlugAsync(string slug, CancellationToken cancellationToken = default);

        /// <summary>
        /// Thêm hoặc cập nhật một chuyên mục/chủ đề.
        /// </summary>
        /// <param name="newCategory"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> AddOrEditCategoryAsync(Category newCategory, CancellationToken cancellationToken = default);

        /// <summary>
        /// Tìm một chuyên mục theo mã số cho trước
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Category> FindCategoryByIdAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Xóa một chuyên mục theo mã số cho trước
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> DeleteCategoryByIdAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Kiểm tra tên định danh (slug) của một chuyên mục đã tồn tại hay chưa.
        /// </summary>
        /// <param name="slug"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> IsSlugOfCategoryExist(string slug, CancellationToken cancellationToken = default);

        /// <summary>
        /// Lấy và phân trang danh sách chuyên mục, kết quả trả về kiểu IPagedList<CategoryItem>
        /// </summary>
        /// <param name="pagingParams"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IPagedList<CategoryItem>> GetPagedCategoriesAsync(IPagingParams pagingParams,
            string name = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Lấy danh sách chuyên mục và số lượng bài viết nằm thuộc từng chuyên mục chủ đề
        /// </summary>
        /// <param name="showOnMenu"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IList<CategoryItem>> GetCategoriesAsync(
            bool showOnMenu = false,
            CancellationToken cancellationToken = default);
    }
}
