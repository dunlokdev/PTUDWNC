using Mapster;
using MapsterMapper;
using TatBlog.Core.Collections;
using TatBlog.Core.DTO;
using TatBlog.Services.Blogs;
using TatBlog.WebApi.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TatBlog.WebApi.Endpoints
{
    public static class PostEndpoints
    {
        public static WebApplication MapPostsEndpoints(this WebApplication app)
        {
            var routeGroupBuilder = app.MapGroup("/api/posts");
            routeGroupBuilder.MapGet("/", GetPosts)
               .WithName("GetPosts")
               .Produces<PaginationResult<PostDto>>();

            routeGroupBuilder.MapGet("/featured/{limit:int}", GetPopularArticle)
                .WithName("GetPopularArticle")
                .Produces<ApiResponse<IList<PostDto>>>();

            return app;
        }
        /// <summary>
        /// Lấy danh sách bài viết. Hỗ trợ tìm theo từ khóa, chuyên mục, tác giả, ngày đăng, … và phân trang kết quả.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="categoryRepository"></param>
        /// <returns></returns>
        private static async Task<IResult> GetPosts(
            [AsParameters] PostQuery model,
            [AsParameters] PagingModel pagingModel,
            IBlogRepository blogRepository)
        {
            var posts = await blogRepository.GetPagedPostsByQueryAsync(
                posts => posts.ProjectToType<PostDto>(), model, pagingModel);

            var paginationResult = new PaginationResult<PostDto>(posts);
            return Results.Ok(paginationResult);
        }

        private static async Task<IResult> GetPopularArticle(int limit, IBlogRepository blogRepository, IMapper mapper)
        {
            var posts = await blogRepository.GetPopularArticleAsync(limit);

            return Results.Ok(value: mapper.Map<IList<PostDto>>(posts));
        }

    }
}
