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
    public interface IBlogRepository
    {
        // Tìm bài viết có tên định danh là 'slug'
        // và được đăng vào tháng 'month' năm 'year'

        Task<Post> GetPostAsync(
            int year,
            int month,
            string slug,
            CancellationToken cancellationToken = default);

        // Tìm Top N bài viết phổ được nhiều người xem nhất
        Task<IList<Post>> GetPopularArticleAsync(int numPosts, CancellationToken cancellationToken = default);

        // Kiểm tra xem tên định danh của bài viết đã có hay chưa
        Task<bool> IsPostSlugExistedAsync(
            int postId, string slug,
            CancellationToken cancellationToken = default);

        // Tăng số lượt xem của một bài viết
        Task IncreaseViewCountAsync(
            int postId,
            CancellationToken cancellationToken = default);

        Task<IList<CategoryItem>> GetCategoriesAsync(
            bool showOnMenu = false,
            CancellationToken cancellationToken = default);

        // Lấy danh sách từ khoá/thẻ và phân trang theo
        // các tham số pagingParams
        Task<IPagedList<TagItem>> GetPagedTagsAsync(
            IPagingParams pagingParams, CancellationToken cancellationToken = default);

        /// <summary>
        /// Tìm một thẻ (Tag) theo tên định danh (slug)
        /// </summary>
        /// <param name="slug"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Tag> FindTagBySlugAsync(string slug, CancellationToken cancellationToken = default);

        /// <summary>
        /// Lấy danh sách tất cả các thẻ (Tag) kèm theo số bài viết chứa thẻ đó. Kết quả trả về kiểu IList<TagItem>.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IList<TagItem>> FindTagItemSlugAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Xóa một thẻ theo mã cho trước
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> DeleteTagByIdAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Tìm một chuyên mục (Category) theo tên định danh (slug).
        /// </summary>
        /// <param name="slug"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Category> FindCategoryBySlugAsync(string slug, CancellationToken cancellationToken = default);

        /// <summary>
        /// Tìm một chuyên mục theo mã số cho trước
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Category> FindCategoryByIdAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Thêm hoặc cập nhật một chuyên mục/chủ đề.
        /// </summary>
        /// <param name="newCategory"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> AddOrEditCategoryAsync(Category newCategory, CancellationToken cancellationToken = default);

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
        Task<IPagedList<CategoryItem>> GetPagedCategoriesAsync(IPagingParams pagingParams, CancellationToken cancellationToken = default);

        /// <summary>
        /// Đếm số lượng bài viết trong N tháng gần nhất. N là tham số đầu vào. Kết
        /// quả là một danh sách các đối tượng chứa các thông tin sau: Năm, Tháng, Số
        /// bài viết
        /// </summary>
        /// <param name="month"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Object> CountByMostRecentMonthAsync(int month, CancellationToken cancellationToken = default);

        /// <summary>
        /// Tìm một bài viết theo mã số
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Post> FindPostByIdAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Thêm hay cập nhật một bài viết
        /// </summary>
        /// <param name="post"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> AddOrUpdatePostAsync(Post post, CancellationToken cancellationToken = default);




    }
}
