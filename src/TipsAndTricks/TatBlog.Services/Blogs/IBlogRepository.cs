using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;

namespace TatBlog.Services.Blogs
{
    public interface IBlogRepository
    {
        /// <summary>
        /// Tìm bài viết có tên định danh là 'slug' và được đăng vào tháng 'month' năm 'year'
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="slug"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Post> GetPostAsync(
            int year,
            int month,
            string slug,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Tìm Top N bài viết phổ được nhiều người xem nhất
        /// </summary>
        /// <param name="numPosts"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IList<Post>> GetPopularArticleAsync(int numPosts, CancellationToken cancellationToken = default);

        /// <summary>
        /// Kiểm tra xem tên định danh của bài viết đã có hay chưa
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="slug"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> IsPostSlugExistedAsync(
            int postId, string slug,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Tăng số lượt xem của một bài viết
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task IncreaseViewCountAsync(
            int postId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Lấy danh sách từ khoá/thẻ và phân trang theo
        /// các tham số pagingParams
        /// </summary>
        /// <param name="pagingParams"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
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
        Task<Post> GetPostByIdAsync(int postId, bool includeDetails = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// Thêm hay cập nhật một bài viết
        /// </summary>
        /// <param name="post"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Post> CreateOrUpdatePostAsync(Post post, IEnumerable<string> tags, CancellationToken cancellationToken = default);

        /// <summary>
        /// Chuyển đổi trạng thái Published của bài viết
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task ChangeStatusPublishedOfPostAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Lấy ngẫu nhiên N bài viết. N là tham số đầu vào
        /// </summary>
        /// <param name="num"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IList<Post>> GetPostsByQualAsync(int num, CancellationToken cancellationToken = default);

        /// <summary>
        /// Tìm tất cả bài viết thỏa mãn điều kiện tìm kiếm được cho trong đối tượng PostQuery (kết quả trả về kiểu IList<Post>)
        /// </summary>
        /// <param name="query"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IList<Post>> FindPostByPostQueryAsync(PostQuery query, CancellationToken cancellationToken = default);

        /// <summary>
        /// Đếm số lượng bài viết thỏa mãn điều kiện tìm kiếm được cho trong đối tượng PostQuery
        /// </summary>
        /// <param name="query"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> CountPostsOfPostQueryAsync(PostQuery query, CancellationToken cancellationToken = default);

        /// <summary>
        /// Tìm và phân trang các bài viết thỏa mãn điều kiện tìm kiếm được cho trong đối tượng PostQuery(kết quả trả về kiểu IPagedList<Post>)
        /// </summary>
        /// <param name=""></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IPagedList<Post>> GetPagedPostByPostQueryAsync(IPagingParams pagingParams, PostQuery query, CancellationToken cancellationToken = default);


        // Version of teacher
        Task<IPagedList<Post>> GetPagedPostsAsync(
        PostQuery condition,
        int pageNumber = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default);

        Task<IList<TagItem>> GetListTagItemAsync(TagQuery tagQuery = null, CancellationToken cancellationToken = default);
        Task<bool> DeletePostByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<bool> IsTagSlugExistedAsync(int id, string slug, CancellationToken cancellationToken = default);
        Task<Tag> FindTagById(int id, CancellationToken cancellationToken = default);
        Task<bool> AddOrEditTagAsync(Tag tag, CancellationToken cancellationToken = default);
        Task<IPagedList<T>> GetPagedPostsByQueryAsync<T>(Func<IQueryable<Post>, IQueryable<T>> mapper, PostQuery query, IPagingParams pagingParams, CancellationToken cancellationToken = default);
    }
}
