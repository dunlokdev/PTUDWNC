// See https://aka.ms/new-console-template for more information

using System.Collections.Immutable;
using TatBlog.Data.Contexts;
using TatBlog.Data.Seeders;
using TatBlog.Services.Blogs;

var context = new BlogDbContext();
//var seeder = new DataSeeder(context);
//seeder.Initialize();

//var authors = context.Authors.ToList();
//foreach (var author in authors)
//{
//    Console.WriteLine("{0, -4} {1, -30} {2, -30} {3, 12:MM/dd/yyyy}",
//        author.Id, author.FullName, author.Email, author.JoinedDate  );
//}

//var context = new BlogDbContext();

//var posts = context.Posts
//    .Where(p => p.Published)
//    .OrderBy(p => p.Title)
//    .Select(p => new
//    {
//        Id = p.Id,
//        Title = p.Title,
//        ViewCount = p.ViewCount,
//        PostedDate = p.PostedDate,
//        Author = p.Author.FullName,
//        Category = p.Category.Name,
//    })
//    .ToList();

//foreach (var post in posts)
//{
//    Console.WriteLine("ID:     : {0}", post.Id);
//    Console.WriteLine("Title:  : {0}", post.Title);
//    Console.WriteLine("View:   : {0}", post.ViewCount);
//    Console.WriteLine("Date:   : {0:MM/dd/yyyy}", post.PostedDate);
//    Console.WriteLine("Author  : {0}", post.Author);
//    Console.WriteLine("Category: {0}", post.Category);
//    Console.WriteLine("".PadRight(80, '-'));
//}

IBlogRepository blogRepository = new BlogRepository(context);

// Tìm 3 bài viết được xem/đọc nhiều nhất 
var posts = await blogRepository.GetPopularArticleAsync(3);

foreach (var post in posts)
{
    Console.WriteLine("ID:     : {0}", post.Id);
    Console.WriteLine("Title:  : {0}", post.Title);
    Console.WriteLine("View:   : {0}", post.ViewCount);
    Console.WriteLine("Date:   : {0:MM/dd/yyyy}", post.PostedDate);
    Console.WriteLine("Author  : {0}", post.Author.FullName);
    Console.WriteLine("Category: {0}", post.Category.Name);
    Console.WriteLine("".PadRight(80, '-'));

}











