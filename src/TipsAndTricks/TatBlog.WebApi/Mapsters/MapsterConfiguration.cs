using Mapster;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.WebApi.Models;
using TatBlog.WebApi.Models.Post;

namespace TatBlog.WebApi.Mapsters
{
    public class MapsterConfiguration : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Author, AuthorDto>();
            config.NewConfig<Author, AuthorItem>()
                .Map(dest => dest.PostCount,
                src => src.Posts == null ? 0 : src.Posts.Count);

            config.NewConfig<AuthorEditModel, Author>();
            config.NewConfig<Category, CategoryItem>()
                .Map(dest => dest.PostCount,
                src => src.Posts == null ? 0 : src.Posts.Count);
            config.NewConfig<PostFilterModel, PostQuery>()
                .Map(dest => dest.KeyWord, src => src.Keyword);

            config.NewConfig<Post, PostItem>()
                .Map(dst => dst.AuthorName, src => src.Author.FullName)
                .Map(dst => dst.CategoryName, src => src.Category.Name)
                .Map(dst => dst.Tags, src => src.Tags.Select(t => t.Name));

            config.NewConfig<Post, PostDto>();
            config.NewConfig<PostEditModel, Post>()
                .Ignore(dest => dest.ImageUrl);
            config.NewConfig<Post, PostDetail>();
        }
    }
}
