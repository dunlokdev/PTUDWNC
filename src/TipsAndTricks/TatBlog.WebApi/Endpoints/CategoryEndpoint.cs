using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using System.Net.NetworkInformation;
using TatBlog.Core.Collections;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.Services.Media;
using TatBlog.WebApi.Extensions;
using TatBlog.WebApi.Filters;
using TatBlog.WebApi.Models;
using TatBlog.WebApi.Validations;

namespace TatBlog.WebApi.Endpoints
{
    public static class CategoryEndpoints
    {
        public static WebApplication MapCategoryEndpoints(this WebApplication app)
        {
            var routeGroupBuilder = app.MapGroup("/api/categories");

            routeGroupBuilder.MapGet("/", GetCategories)
                .WithName("GetCategories")
                .Produces<PaginationResult<CategoryItem>>();

            routeGroupBuilder.MapGet("/{id:int}", GetCategoyDetails)
                .WithName("GetCategoryById")
                .Produces<CategoryItem>()
                .Produces(404);

            routeGroupBuilder.MapGet("/{slug:regex(^[a-z0-9_-]+$)}/posts", GetPostsByCategorySlug)
                .WithName("GetPostsByCategorySlug")
                .Produces<PaginationResult<PostDto>>()
                .Produces(404);

            routeGroupBuilder.MapPost("/", AddCategory)
                .WithName("AddCategory")
                .AddEndpointFilter<ValidatorFilter<CategoryEditModel>>()
                .Produces(201)
                .Produces(400)
                .Produces(409);

            routeGroupBuilder.MapPut("/{id:int}", UpdateCategory)
                .WithName("UpdateACategory")
                .AddEndpointFilter<ValidatorFilter<CategoryEditModel>>()
                .Produces(204)
                .Produces(400)
                .Produces(409);


            return app;
        }

        private static async Task<IResult> GetCategories([AsParameters] CategoryFilterModel model, IBlogRepository blogRepository)
        {
            // model kế thừa từ PagingModel
            var categories = await blogRepository.GetPagedCategoriesAsync(model, model.Name);

            var paginationResult = new PaginationResult<CategoryItem>(categories);
            return Results.Ok(paginationResult);
        }

        private static async Task<IResult> GetCategoyDetails(int id, IBlogRepository blogRepository, IMapper mapper)
        {
            var category = await blogRepository.GetCachedCategoryByIdAsync(id);
            return category == null
                ? Results.NotFound($"Không tìm thấy tiêu đề có mã số {id}")
                : Results.Ok(mapper.Map<CategoryItem>(category));
        }

        private static async Task<IResult> GetPostsByCategorySlug(
           [FromRoute] string slug,
           [AsParameters] PagingModel pagingModel,
           IBlogRepository blogRepository)
        {
            var postQuery = new PostQuery()
            {
                CategorySlug = slug,
                PublishedOnly = true
            };
            var posts = await blogRepository.GetPagedPostsByQueryAsync(
                posts => posts.ProjectToType<PostDto>(),
                postQuery,
                pagingModel);
            var paginationResult = new PaginationResult<PostDto>(posts);

            return paginationResult.Items.Count() <= 0 
                ? Results.NotFound($"Không tìm thấy tiêu đề có slug '{slug}'")
                : Results.Ok(paginationResult);
        }

        private static async Task<IResult> AddCategory(
            CategoryEditModel model, IBlogRepository blogRepository, IMapper mapper)
        {
            if (await blogRepository.IsCategorySlugExistedAsync(0, model.UrlSlug))
            {
                return Results.Conflict($"Slug '{model.UrlSlug}' đã được sử dụng");
            }

            var category = mapper.Map<Category>(model);
            await blogRepository.AddOrEditCategoryAsync(category);

            return Results.CreatedAtRoute(
                "GetCategoryById",
                new { category.Id },
                mapper.Map<CategoryItem>(category));
        }

        private static async Task<IResult> UpdateCategory(
            int id,
            CategoryEditModel model,
            IBlogRepository blogRepository,
            IMapper mapper)
        {
            if (await blogRepository.IsCategorySlugExistedAsync(id, model.UrlSlug))
            {
                return Results.Conflict($"Slug '{model.UrlSlug}' đã được sử dụng");
            }

            var category = mapper.Map<Category>(model);
            category.Id = id;

            return await blogRepository.AddOrEditCategoryAsync(category)
                ? Results.NoContent()
                : Results.NotFound();
        }

    }
}
