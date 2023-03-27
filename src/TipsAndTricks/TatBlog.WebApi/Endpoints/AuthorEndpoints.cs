using TatBlog.Core.Collections;
using TatBlog.Core.DTO;
using TatBlog.Services.Blogs;
using TatBlog.WebApi.Models;

namespace TatBlog.WebApi.Endpoints
{
    public static class AuthorEndpoints
    {
        public static WebApplication MapAuthorEndpoints(this WebApplication app)
        {
            var routeGroupBuilder = app.MapGroup("/api/authors");

            routeGroupBuilder.MapGet("/", GetAuthors)
                .WithName("GetAuthors")
                .Produces<PaginationResult<AuthorItem>>();

            return app;
        }

        private static async Task<IResult> GetAuthors([AsParameters] AuthorFilterModel model, IAuthorRepository authorRepository)
        {
            var authorList = await authorRepository.GetPagedAuthorsAsync(model, model.Name);

            var paginationResult = new PaginationResult<AuthorItem>(authorList);

            return Results.Ok(paginationResult);
        }
    }
}
