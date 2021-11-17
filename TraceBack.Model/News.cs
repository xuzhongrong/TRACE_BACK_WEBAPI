using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraceBack.Model
{

    public class News: QueryEntity
    {
        public Guid? news_id { get; set; }
        public string new_title { get; set; }
        public string news_url { get; set; }
        public DateTime? news_time { get; set; }
        public string news_author { get; set; }
        public string news_origin { get; set; }
    }
}
