using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatBlog.Core.DTO
{
    public class PostQuery
    {
        public int AuthorId { get; set; }
        public int CategoryId { get; set; }
        public string SlugCategory { get; set; }
        public int MonthCreated { get; set; }
        public int YearCreated { get; set; }
        public string Tag { get; set; }
    }
}
