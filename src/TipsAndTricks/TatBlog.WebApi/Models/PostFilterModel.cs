namespace TatBlog.WebApi.Models
{
    public class PostFilterModel : PagingModel
    {
        public string Keyword { get; set; }
        public string CategoryName { get; set; }
        public string AuthorName { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
