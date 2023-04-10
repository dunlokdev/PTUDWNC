﻿using Mapster;
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
                .Map(dest => dest.PublishedOnly, src => src.Published == true ? true : false)
                .Map(dest => dest.KeyWord, src => src.Keyword)
                .Map(dest => dest.NotPublished, src => src.Published != true ? true : false);

            config.NewConfig<Post, PostDto>();
            config.NewConfig<PostEditModel, Post>()
                .Ignore(dest => dest.ImageUrl);
            config.NewConfig<Post, PostDetail>();
        }
    }
}
