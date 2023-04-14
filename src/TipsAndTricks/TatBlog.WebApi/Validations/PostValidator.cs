using FluentValidation;
using TatBlog.WebApi.Models.Post;

namespace TatBlog.WebApi.Validations
{
    public class PostValidator : AbstractValidator<PostEditModel>
    {
        public PostValidator()
        {
            RuleFor(x => x.Title)
              .NotEmpty()
              .WithMessage("Tiêu đề không được để trống")
              .MaximumLength(500)
              .WithMessage("Độ dài tiêu đề nhỏ hơn 500 ký tự");

            RuleFor(x => x.ShortDescription)
              .NotEmpty()
              .WithMessage("Mô tả không được để trống");

            RuleFor(x => x.Description)
              .NotEmpty()
              .WithMessage("Nội dung không được để trống");

            RuleFor(x => x.Meta)
              .NotEmpty()
              .MaximumLength(1000);

            RuleFor(x => x.CategoryId)
              .NotEmpty()
              .WithMessage("Bạn phải chọn chủ đề cho bài viết");

            RuleFor(x => x.AuthorId)
              .NotEmpty()
              .WithMessage("Bạn phải chọn tác giả của bài viết");

            RuleFor(x => x.SelectedTags)
              .Must(HasLeastOneTag)
              .WithMessage("Bạn phải nhập ít nhất một thẻ");
        }

        private bool HasLeastOneTag(
          PostEditModel postModel, string selectedTags)
        {
            return postModel.GetSelectedTags().Any();
        }
    }
}
