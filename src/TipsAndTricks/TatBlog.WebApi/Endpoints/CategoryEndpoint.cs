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

        //private static async Task<IResult> GetPostsByAuthorId(int id, [AsParameters] PagingModel pagingModel, IBlogRepository blogRepository)
        //{
        //    var postQuery = new PostQuery()
        //    {
        //        AuthorId = id,
        //        PublishedOnly = true,
        //    };

        //    var postsList = await blogRepository.GetPagedPostsByQueryAsync(
        //        posts => posts.ProjectToType<PostDto>(), postQuery, pagingModel);

        //    var paginationResult = new PaginationResult<PostDto>(postsList);

        //    return Results.Ok(paginationResult);
        //}


    }
}
