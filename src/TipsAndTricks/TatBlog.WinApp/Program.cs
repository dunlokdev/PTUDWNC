// See https://aka.ms/new-console-template for more information

using System.Collections.Immutable;
using TatBlog.Data.Contexts;
using TatBlog.Data.Seeders;
using TatBlog.Services.Blogs;
using TatBlog.WinApp;

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

// Tìm 3 bài viết được xem/đọc nhiều nhất 
//IBlogRepository blogRepository = new BlogRepository(context);
//var posts = await blogRepository.GetPopularArticleAsync(3);

//foreach (var post in posts)
//{
//    Console.WriteLine("ID:     : {0}", post.Id);
//    Console.WriteLine("Title:  : {0}", post.Title);
//    Console.WriteLine("View:   : {0}", post.ViewCount);
//    Console.WriteLine("Date:   : {0:MM/dd/yyyy}", post.PostedDate);
//    Console.WriteLine("Author  : {0}", post.Author.FullName);
//    Console.WriteLine("Category: {0}", post.Category.Name);
//    Console.WriteLine("".PadRight(80, '-'));
//}

//IBlogRepository blogRepo = new BlogRepository(context);

//// Lấy danh sách chuyên mục
//var categories = await blogRepo.GetCategoriesAsync();

//Console.WriteLine("{0, -5}{1,-50},{2,10}", "ID", "Name", "Count");

//foreach (var item in categories)
//{
//    Console.WriteLine("{0,-5}{1,-50}{2,20}", item.Id, item.Name, item.PostCount);
//}

IBlogRepository blogRepo = new BlogRepository(context);
var pagingParams = new PagingParams
{
    PageNumber = 1,
    PageSize = 5,
    SortColumn = "Name",
    SortOrder = "DESC"
};

var tagsList = await blogRepo.GetPagedTagsAsync(pagingParams);

Console.WriteLine("{0, -5}{1,-50}{2,10}", "ID", "Name", "Count");

foreach (var item in tagsList)
{
    Console.WriteLine("{0, -5}{1,-50}{2,10}",
        item.Id, item.Name, item.PostCount);
}

// Tìm một thẻ (Tag) theo tên định danh (slug)
//string slug = "visual-studio";
//var tag = await blogRepo.FindTagBySlugAsync(slug);

//Console.WriteLine("{0, -5}{1,-20}{2,-20}", "ID", "Name", "Description");

//Console.WriteLine("{0, -5}{1,-20}{2,-20}",
//    tag.Id, tag.Name, tag.Description);

//Tạo lớp DTO có tên là TagItem để chứa các thông tin về thẻ và số lượng
//Lấy danh sách tất cả các thẻ (Tag) kèm theo số bài viết chứa thẻ đó. Kết
//quả trả về kiểu IList<TagItem>.

//var tags = await blogRepo.FindTagItemSlugAsync();
//Console.WriteLine("{0, -5}{1,-50}{2,-20}", "ID", "Name", "Post Count");

//foreach (var tag in tags)
//{
//    Console.WriteLine("{0, -5}{1,-50}{2,-20}",
//        tag.Id, tag.Name, tag.PostCount);
//}

//Xóa một thẻ theo mã cho trước
//int id = 2;
//await blogRepo.DeleteTagById(id);

