namespace TatBlog.WebApi.Models.Post
{
    public class PostFilterModel
    {
        public string Keyword { get; set; }
        public int? AuthorId { get; set; }
        public int? CategoryId { get; set; }
        public bool? Published { get; set; } 
        public string CategorySlug { get; set; }
        public string TitleSlug { get; set; }
        public string TagSlug { get; set; }
        public string AuthorSlug { get; set; }
        public int? Year { get; set; }
        public int? Month { get; set; }
    }
}
