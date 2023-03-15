using TatBlog.Core.Contracts;

namespace TatBlog.Core.Entities
{
    public class Post : IEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string Meta { get; set; }
        public string UrlSlug { get; set; }
        public string ImageUrl { get; set; }
        public int ViewCount { get; set; }
        public bool Published { get; set; }
        public DateTime PostedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int CategoryId { get; set; }
        public int AuthorId { get; set; }
        public Category Category { get; set; }
        public Author Author { get; set; }
        public IList<Tag> Tags { get; set; }

        public override string ToString()
        {
            string line = "--------------------------------------------";
            string id = "\nID: " + this.Id;
            string title = "\nTilte: " + this.Title;
            string shortDesc = "\nShort description: " + this.ShortDescription;
            string urlSlug = "\nUrl slug: " + this.UrlSlug;
            string viewCount = "\nView count: " + this.ViewCount;
            return line + id + title + shortDesc + urlSlug + viewCount;
        }
    }
}
