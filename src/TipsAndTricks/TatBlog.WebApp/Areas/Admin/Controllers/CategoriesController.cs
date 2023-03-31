using FluentValidation;
using FluentValidation.AspNetCore;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.Services.Media;
using TatBlog.WebApp.Areas.Admin.Models;
using TatBlog.WebApp.Validations;

namespace TatBlog.WebApp.Areas.Admin.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CategoryEditModel> _validator;

        public CategoriesController(ICategoryRepository blogRepository, IMapper mapper)
        {
            _categoryRepository = blogRepository;
            _mapper = mapper;
            _validator = new CategoryValidator(_categoryRepository);
        }

        public async Task<IActionResult> Index(CategoryFilterModel model,
            [FromQuery(Name = "p")] int pageNumber = 1,
            [FromQuery(Name = "ps")] int pageSize = 10)
        {
            var query = _mapper.Map<CategoryQuery>(model);

            var categories = await _categoryRepository.GetCategoriesByQuery(query, pageNumber, pageSize);
            ViewBag.Categories = categories;

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var category = id > 0
                ? await _categoryRepository.FindCategoryByIdAsync(id)
                : null;

            var model = category == null
                ? new CategoryEditModel()
                : _mapper.Map<CategoryEditModel>(category);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CategoryEditModel model)
        {
            var isValidation = await _validator.ValidateAsync(model);

            if (!isValidation.IsValid)
            {
                isValidation.AddToModelState(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var category = model.Id > 0
                ? await _categoryRepository.FindCategoryByIdAsync(model.Id) : null;

            if (category == null)
            {
                category = _mapper.Map<Category>(model);
                category.Id = 0;
            }
            else
            {
                _mapper.Map(model, category);
            }

            await _categoryRepository.AddOrEditCategoryAsync(category);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteCategory(int id)
        {
            var post = await _categoryRepository.FindCategoryByIdAsync(id);
            await _categoryRepository.DeleteCategoryByIdAsync(post.Id);
            return RedirectToAction(nameof(Index));
        }

    }
}
