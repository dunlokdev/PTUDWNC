// See https://aka.ms/new-console-template for more information

using System.Collections.Immutable;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;
using TatBlog.Data.Seeders;
using TatBlog.Services.Blogs;
using TatBlog.WinApp;

var context = new BlogDbContext();
IBlogRepository blogRepo = new BlogRepository(context);

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

//IBlogRepository blogRepo = new BlogRepository(context);
//var pagingParams = new PagingParams
//{
//    PageNumber = 1,
//    PageSize = 5,
//    SortColumn = "Name",
//    SortOrder = "DESC"
//};

//var tagsList = await blogRepo.GetPagedTagsAsync(pagingParams);

//Console.WriteLine("{0, -5}{1,-50}{2,10}", "ID", "Name", "Count");

//foreach (var item in tagsList)
//{
//    Console.WriteLine("{0, -5}{1,-50}{2,10}",
//        item.Id, item.Name, item.PostCount);
//}

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

// Xóa một thẻ theo mã cho trước.
//var slug = "net-core";
//IBlogRepository blogRepo = new BlogRepository(context);
//var category = await blogRepo.FindCategoryBySlugAsync(slug);
//Console.WriteLine("{0, -5}{1,-20}{2,-20}", category.Id, category.Name, category.Description);

// Tìm một chuyên mục (Category) theo tên định danh (slug)
//IBlogRepository blogRepo = new BlogRepository(context);
//var item = await blogRepo.FindCategoryBySlugAsync("design-patterns");
//Console.WriteLine("{0,-5}{1,-50}{2,-50}", "ID", "Name", "Description");
//Console.WriteLine("{0,-5}{1,-50}{2,-50}", item.Id, item.Name, item.Description);

// Tìm một chuyên mục theo mã số cho trước
//IBlogRepository blogRepo = new BlogRepository(context);
//var item = await blogRepo.FindCategoryByIdAsync(2);
//Console.WriteLine("{0,-5}{1,-50}{2,-50}", "ID", "Name", "Description");
//Console.WriteLine("{0,-5}{1,-50}{2,-50}", item.Id, item.Name, item.Description);

// Thêm hoặc cập nhật một chuyên mục/chủ đề
//var newPost = new Category()
//{
//    Name = "Cloud",
//    Description = "Explore and assess Google Cloud with free usage of over 20 products, plus new customers get $300 in free credits on signup.",
//    UrlSlug = "cloud",
//};

//IBlogRepository blogRepo = new BlogRepository(context);
//var rowChange = await blogRepo.AddOrEditCategoryAsync(newPost);
//Console.WriteLine(rowChange ? "Update success" : "Failed, try again");

// Xóa một chuyên mục theo mã số cho trước.
//IBlogRepository blogRepo = new BlogRepository(context);
//var isSuccess = await blogRepo.DeleteCategoryByIdAsync(1002);
//Console.WriteLine(isSuccess ? "Delete success" : "Failed, try again");

// Kiểm tra tên định danh (slug) của một chuyên mục đã tồn tại hay chưa.
//IBlogRepository blogRepo = new BlogRepository(context);
//if (await blogRepo.IsSlugOfCategoryExist("net-core"))
//{
//    Console.WriteLine("Exist");
//} else
//{
//    Console.WriteLine("No Exist");
//}

//IBlogRepository blogRepo = new BlogRepository(context);
//if (await blogRepo.IsSlugOfCategoryExist("not-exist"))
//{
//    Console.WriteLine("Exist");
//}
//else
//{
//    Console.WriteLine("No Exist");
//}

// Lấy và phân trang danh sách chuyên mục, kết quả trả về kiểu IPagedList<CategoryItem>.
//IBlogRepository blogRepo = new BlogRepository(context);

//var paringParams = new PagingParams()
//{
//    PageNumber = 1,
//    PageSize = 5,
//    SortColumn = "PostCount",
//    SortOrder = "DESC"
//};

//var categoriesPage = await blogRepo.GetPagedCategoriesAsync(paringParams);
//Console.WriteLine("{0, -10}{1, -50}{2, 10}","ID", "Name", "Count");

//foreach (var category in categoriesPage)
//{
//    Console.WriteLine("{0, -10}{1, -50}{2, 10}",
//        category.Id, category.Name, category.PostCount);
//}

// Tìm một bài viết theo mã sốA
//var post = await blogRepo.FindPostByIdAsync(9);
//Console.WriteLine("{0, -10}{1, -50}{2, -50}",
//    post.Id, post.Title, post.ShortDescription);

//Thêm hoặc cập nhật một post 
//var newPost = new Post()
//{
//    Id = 1,
//    Title = "His mother had always taught him",
//    ShortDescription = "His mother had always taught him not to ever think of himself as better than others save changed.",
//    Description = "His mother had always taught him not to ever think of himself as better than others. He'd tried to live by this motto. He never looked down on those who were less fortunate or who had less money than him. But the stupidity of the group of people he was talking to made him change his mind.",
//    Meta = "post-01",
//    UrlSlug = "his-mother-had-always-taught-him",
//    Published = true,
//    PostedDate = new DateTime(2023, 2, 22, 1, 20, 0),
//    ModifiedDate = null,
//    ViewCount = 2,
//    AuthorId = 1,
//    CategoryId = 1,
//};


//var rowChange = await blogRepo.AddOrUpdatePostAsync(newPost);
//Console.WriteLine(rowChange ? "Update success" : "Failed, try again");

// Chuyển đổi trạng thái Published của bài viết
//await blogRepo.ChangeStatusPublishedOfPostAsync(1);

// Lấy ngẫu nhiên N bài viết. N là tham số đầu vào.
//var posts = await blogRepo.GetPostsByQualAsync(3);
//foreach (var post in posts)
//{
//    Console.WriteLine("{0, -5}{1, -50}{2, -50}",
//        post.Id, post.Title, post.ShortDescription);
//}

// Tìm tất cả bài viết thỏa mãn điều kiện tìm kiếm được cho trong đối tượng PostQuery (kết quả trả về kiểu IList<Post>)

//var query = new TatBlog.Core.DTO.PostQuery()
//{
//    AuthorId = 1,
//    CategoryId = 1,
//    SlugCategory = "angular",
//    TimeCreated = DateTime.Parse("2022-11-08"),
//    Tag = "ASP.NET MVC",
//};

//var posts = await blogRepo.FindPostByPostQuery(query);
//int count = 1;

//foreach (var post in posts)
//{
//    Console.WriteLine(count++);
//    Console.WriteLine("---------------------------------------------------\n");
//    Console.WriteLine("Author ID: " + post.AuthorId);
//    Console.WriteLine("Category ID: " + post.CategoryId);
//    Console.WriteLine("Category Slug: " + post.Category.UrlSlug);
//    Console.WriteLine("Month: " + post.PostedDate.Month);
//    Console.WriteLine("Year: " + post.PostedDate.Year);
//}

//Console.WriteLine("\n Count: " + await blogRepo.CountPostsOfPostQuery(query));

var paringParams = new PagingParams()
{
    PageNumber = 1,
    PageSize = 5,
    SortColumn = "ViewCount",
    SortOrder = "DESC"
};

var query = new TatBlog.Core.DTO.PostQuery()
{
    AuthorId = 1,
    CategoryId = 1,
    SlugCategory = "angular",
    TimeCreated = DateTime.Parse("2022-11-08"),
    Tag = "ASP.NET MVC",
};

var posts = await blogRepo.GetPagedPostByPostQuery(paringParams, query);
foreach (var post in posts)
{
    Console.WriteLine(post);
}
