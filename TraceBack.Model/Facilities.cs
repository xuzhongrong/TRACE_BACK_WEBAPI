using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraceBack.Model
{
    public class Facilities: QueryEntity
    {
        public Guid? facilities_id { get; set; }
        public string facilities_name { get; set; }
        public string facilities_people { get; set; }
        public string facilities_address { get; set; }
        public DateTime? facilities_time_start { get; set; }
        public DateTime? facilities_time_end { get; set; }
    }
}
