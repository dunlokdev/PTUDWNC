using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Writers;
using SlugGenerator;
using System.Collections.Generic;
using System.Net;
using TatBlog.Core.Collections;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.Services.Media;
using TatBlog.WebApi.Filters;
using TatBlog.WebApi.Models;
using TatBlog.WebApi.Models.Post;
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
               .Produces<ApiResponse<PaginationResult<PostDto>>>();

            routeGroupBuilder.MapGet("/get-filter", GetFilter)
               .WithName("GetFilter")
               .Produces<ApiResponse<PostFilterModel>>();


            routeGroupBuilder.MapGet("/get-posts-filter", GetFilteredPosts)
               .WithName("GetFilteredPosts")
               .Produces<ApiResponse<PostDto>>();

            routeGroupBuilder.MapGet("/featured/{limit:int}", GetPopularArticle)
                .WithName("GetPopularArticle")
                .Produces<ApiResponse<IList<PostDto>>>();

            routeGroupBuilder.MapGet("/random/{limit:int}", GetRandomPosts)
                .WithName("GetRandomPosts")
                .Produces<ApiResponse<IList<PostDto>>>();

            routeGroupBuilder.MapGet("archives/{limit:int}", GetPostsInMonthly)
                .WithName("GetPostsInMonthly")
                .Produces<ApiResponse<IList<MonthlyPostCountItem>>>();

            routeGroupBuilder.MapGet("{id:int}", GetPostDetailById)
                .WithName("GetPostDetailById")
                .Produces<ApiResponse<PostDetail>>();

            routeGroupBuilder.MapGet("/byslug/{slug:regex(^[a-z0-9_-]+$)}", GetPostDetailBySlug)
                .WithName("GetPostDetailBySlug")
                .Produces<ApiResponse<PostDetail>>();

            routeGroupBuilder.MapPost("/", AddPost)
                  .WithName("AddPost")
                  //.AddEndpointFilter<ValidatorFilter<PostEditModel>>()
                  .Accepts<PostEditModel>("multipart/form-data")
                  .Produces(401)
                  .Produces<ApiResponse<PostItem>>();

            routeGroupBuilder.MapPost("{id:int}/picture", SetImagePost)
                .WithName("SetImagePost")
                .Accepts<IFormFile>("multipart/form-data")
                .Produces<ApiResponse<string>>();

            //routeGroupBuilder.MapPut("/{id:int}", UpdatePost)
            //    .WithName("UpdateAPost")
            //    .AddEndpointFilter<ValidatorFilter<PostEditModel>>()
            //    .Produces(401)
            //    .Produces<ApiResponse<string>>();

            routeGroupBuilder.MapDelete("/{id:int}", DeletePost)
                .WithName("DeletePost")
                .Produces<ApiResponse<string>>();

            return app;
        }

        // Lấy danh sách bài viết. Hỗ trợ tìm theo từ khóa, chuyên mục, tác giả, ngày đăng, … và phân trang kết quả.
        private static async Task<IResult> GetPosts(
            [AsParameters] PostFilterModel model,
            [AsParameters] PagingModel pagingModel,
            IBlogRepository blogRepository,
            IMapper mapper)
        {
            // Tạo điều kiện truy vấn
            var postQuery = mapper.Map<PostQuery>(model);

            var posts = await blogRepository.GetPagedPostsByQueryAsync<PostDto>(
                posts => posts.ProjectToType<PostDto>(),
                postQuery,
                pagingModel);

            var paginationResult = new PaginationResult<PostDto>(posts);
            return Results.Ok(ApiResponse.Success(paginationResult));
        }

        // Lấy danh sách các tác giả và chủ đề mà không cần phân trang
        private static async Task<IResult> GetFilter(IBlogRepository blogRepository, IAuthorRepository authorRepository, ICategoryRepository categoryRepository)
        {
            var model = new PostFilterModel()
            {
                AuthorList = (await authorRepository.GetAuthorsAsync())
                    .Select(a => new SelectListItem()
                    {
                        Text = a.FullName,
                        Value = a.Id.ToString()
                    }),

                CategoryList = (await categoryRepository.GetCategoriesAsync())
                    .Select(c => new SelectListItem()
                    {
                        Text = c.Name,
                        Value = c.Id.ToString()
                    }),
            };

            return Results.Ok(ApiResponse.Success(model));
        }

        // Lấy các bài viết theo các yêu cầu khác nhau và có phân trang
        private static async Task<IResult> GetFilteredPosts(
            [AsParameters] PostFilterModel model,
            [AsParameters] PagingModel pagingModel,
            IBlogRepository blogRepository)
        {
            var postQuery = new PostQuery()
            {
                KeyWord = model.Keyword,
                CategoryId = model.CategoryId,
                AuthorId = model.AuthorId,
                Year = model.Year,
                Month = model.Month,
            };

            var postList = await blogRepository.GetPagedPostsByQueryAsync(
               posts => posts.ProjectToType<PostDto>(), postQuery, pagingModel);

            var paginationResult = new PaginationResult<PostDto>(postList);

            return Results.Ok(ApiResponse.Success(paginationResult));
        }

        // Lấy danh sách N (limit) bài viết nhiều người đọc nhất.
        private static async Task<IResult> GetPopularArticle(int limit, IBlogRepository blogRepository, IMapper mapper)
        {
            var posts = await blogRepository.GetPopularArticleAsync(limit);

            //return Results.Ok(value: mapper.Map<IList<PostDto>>(posts));
            return Results.Ok(ApiResponse.Success(mapper.Map<IList<PostDto>>(posts)));
        }

        // Lấy ngẫu nhiên một danh sách N (limit) bài viết.
        private static async Task<IResult> GetRandomPosts(int limit, IBlogRepository blogRepository, IMapper mapper)
        {
            var posts = await blogRepository.GetRandomsPostsAsync(limit);

            return Results.Ok(ApiResponse.Success(mapper.Map<IList<PostDto>>(posts)));
        }

        //  Lấy danh sách thống kê số lượng bài viết trong N(limit) tháng gần nhất.
        private static async Task<IResult> GetPostsInMonthly(
            int limit,
            IBlogRepository blogRepository)
        {
            var posts = await blogRepository.CountMonthlyPostsAsync(limit);
            return Results.Ok(ApiResponse.Success(posts));
        }

        // Lấy thông tin chi tiết của bài viết có mã số(id) cho trước.  
        private static async Task<IResult> GetPostDetailById(int id, IMapper mapper, IBlogRepository blogRepository)
        {
            var post = await blogRepository.GetPostByIdAsync(id, true);

            return post == null
                ? Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không tìm thấy id = {id}"))
                : Results.Ok(ApiResponse.Success(mapper.Map<PostDetail>(post)));
        }

        // Lấy thông tin chi tiết bài viết có tên định danh(slug) cho trước.
        private static async Task<IResult> GetPostDetailBySlug([FromRoute] string slug, IBlogRepository blogRepository, IMapper mapper)
        {
            var post = await blogRepository.GetPostBySlugAsync(slug, true);

            return post == null
                ? Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không tìm thấy slug '{slug}'"))
                : Results.Ok(ApiResponse.Success(mapper.Map<PostDetail>(post)));
        }

        // Thêm một bài viết mới
        private static async Task<IResult> AddPost(
              HttpContext context,
              IMapper mapper,
              IBlogRepository blogRepository,
              IMediaManager mediaManager)
        {

            var model = await PostEditModel.BindAsync(context);
            var slug = model.Title.GenerateSlug();
            if (await blogRepository.IsPostSlugExistedAsync(model.Id, slug))
            {
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict, $"Slug '{slug}' đã được sử dụng cho bài viết khác"));
            }

            var post = model.Id > 0 ? await blogRepository.GetPostByIdAsync(model.Id, true) : null;

            if (post == null)
            {
                post = new Post()
                {
                    PostedDate = DateTime.Now,
                };
            }

            post.Title = model.Title;
            post.AuthorId = model.AuthorId;
            post.CategoryId = model.CategoryId;
            post.ShortDescription = model.ShortDescription;
            post.Description = model.Description;
            post.Meta = model.Meta;
            post.Published = model.Published;
            post.ModifiedDate = DateTime.Now;
            post.UrlSlug = model.Title.GenerateSlug();

            if (model.ImageFile?.Length > 0)
            {
                string hostname = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.PathBase}/";
                var uploadedPath = await mediaManager.SaveFileAsync(
                    model.ImageFile.OpenReadStream(),
                    model.ImageFile.FileName,
                    model.ImageFile.ContentType);

                if (!string.IsNullOrWhiteSpace(uploadedPath))
                {
                    post.ImageUrl = hostname + uploadedPath;
                }
            }
            await blogRepository.CreateOrUpdatePostAsync(post, model.GetSelectedTags());

            return Results.Ok(ApiResponse.Success(mapper.Map<PostItem>(post), HttpStatusCode.Created));
        }

        // Tải lên hình ảnh đại diện cho bài viết
        private static async Task<IResult> SetImagePost(int id, IFormFile file, IBlogRepository blogRepository, IMediaManager media)
        {
            var imgUrl = await media.SaveFileAsync(file.OpenReadStream(), file.FileName, file.ContentType);

            if (string.IsNullOrWhiteSpace(imgUrl))
            {
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.BadRequest, "Không lưu được tập tin"));
            }

            await blogRepository.SetImageUrlPostAsync(id, imgUrl);
            return Results.Ok(ApiResponse.Success(imgUrl));
        }

        //// Cập nhật thông tin của bài viết có mã số(id) cho trước
        //private static async Task<IResult> UpdatePost(int id, PostEditModel model, IBlogRepository blogRepository, IMapper mapper)
        //{

        //    if (await blogRepository.IsPostSlugExistedAsync(id, model.UrlSlug))
        //    {
        //        return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict, $"Slug '{model.UrlSlug}' đã được sử dụng"));

        //    }

        //    var post = await blogRepository.GetPostByIdAsync(id);
        //    mapper.Map(model, post);
        //    post.Id = id;
        //    post.ModifiedDate = DateTime.Now;

        //    return await blogRepository.CreateOrUpdatePostAsync(post, model.GetSelectedTags()) != null
        //        ? Results.Ok(ApiResponse.Success("Post is updated", HttpStatusCode.NoContent))
        //        : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, "Could not find post"));
        //}

        // Xóa bài viết có mã số (id) cho trước
        private static async Task<IResult> DeletePost(int id, IBlogRepository blogRepository)
        {
            return await blogRepository.DeletePostByIdAsync(id)
                    ? Results.Ok(ApiResponse.Success($"Xóa thành công bài viết có Id = {id}"))
                    : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không tìm thấy bài viết có Id = {id}"));
        }
    }
}
