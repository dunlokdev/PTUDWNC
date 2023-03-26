using Microsoft.AspNetCore.Mvc;
using TatBlog.Services.Blogs;

namespace TatBlog.WebApp.Components
{
    [ViewComponent(Name = "BestAuthorsWidget")] 
    public class BestAuthorsWidget: ViewComponent
    {
        private readonly IAuthorRepository _authorRepository;

        public BestAuthorsWidget(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var authors = await _authorRepository.GetAuthorsHasMostPost(4);
            return View(authors);
        }
    }
}
