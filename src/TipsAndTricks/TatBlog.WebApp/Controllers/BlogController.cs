using Microsoft.AspNetCore.Mvc;
using TatBlog.Core.DTO;
using TatBlog.Services.Blogs;

namespace TatBlog.WebApp.Controllers
{
    public class BlogController : Controller
    {
        private readonly IBlogRepository _blogRepository;

        public BlogController(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }
        public async Task<IActionResult> Index(
            [FromQuery(Name = "k")] string keyword = null,
            [FromQuery(Name = "p")] int pageNumber = 1,
            [FromQuery(Name = "ps")] int pageSize = 5)
        {
            var postQuery = new PostQuery()
            {
                PublishedOnly = true,
                KeyWord = keyword,
            };

            var postsList = await _blogRepository.GetPagedPostsAsync(postQuery, pageNumber, pageSize);

            ViewBag.PostQuery = postQuery;

            return View(postsList);
        }

        public IActionResult About() => View();
        public IActionResult Contact() => View();
        public IActionResult Rss() => Content("Nội dung sẽ được cập nhật");

        public async Task<IActionResult> Category(
            string slug,
            [FromQuery(Name = "p")] int pageNumber = 1,
            [FromQuery(Name = "ps")] int pageSize = 3)
        {

            var postQuery = new PostQuery()
            {
                CategorySlug = slug,
                PublishedOnly = true,
            };

            var posts = await _blogRepository.GetPagedPostsAsync(postQuery, pageNumber, pageSize);
            var categorys = await _blogRepository
                            .FindCategoryBySlugAsync(slug);

            ViewBag.NameCat = categorys.Name;
            ViewBag.PostQuery = postQuery;

            return View(posts);
        }

        public async Task<IActionResult> Author(
           string slug,
           [FromQuery(Name = "p")] int pageNumber = 1,
           [FromQuery(Name = "ps")] int pageSize = 3)
        {

            var postQuery = new PostQuery()
            {
                AuthorSlug = slug,
                PublishedOnly = true,
            };

            var posts = await _blogRepository.GetPagedPostsAsync(postQuery, pageNumber, pageSize);
            ViewBag.PostQuery = postQuery;

            var author = await _blogRepository.FindAuthorBySlugAsync(slug);

            ViewBag.Author = author.FullName;

            return View(posts);
        }

        public async Task<IActionResult> Tag(
           string slug,
           [FromQuery(Name = "p")] int pageNumber = 1,
           [FromQuery(Name = "ps")] int pageSize = 3)
        {

            var postQuery = new PostQuery()
            {
                TagSlug = slug,
                PublishedOnly = true,
            };

            var posts = await _blogRepository.GetPagedPostsAsync(postQuery, pageNumber, pageSize);
            ViewBag.PostQuery = postQuery;

            var tag = await _blogRepository.FindTagBySlugAsync(slug);

            ViewBag.Tag = tag.Name;

            return View(posts);
        }

        public async Task<IActionResult> Post(
                string slug,
                int year,
                int month,
                int day)
        {

            var post = await _blogRepository
                .GetPostAsync(year, month, slug);
            await _blogRepository.IncreaseViewCountAsync(post.Id);

            return View(post);
        }
    }
}
