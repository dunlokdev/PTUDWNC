﻿using Mapster;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.Mapsters
{
    public class MapsterConfiguration : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Post, PostItem>()
                .Map(dest => dest.CategoryName, src => src.Category.Name)
                .Map(dest => dest.Tags, src => src.Tags.Select(tag => tag.Name));

            config.NewConfig<PostFilterModel, PostQuery>()
                .Map(dest => dest.PublishedOnly, src => false);
        }
    }
}
