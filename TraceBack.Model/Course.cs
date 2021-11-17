using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraceBack.Model
{
    public class Course: QueryEntity
    {
        public Guid? course_id { get; set;}
        public string course_name { get; set; }
        public string course_college { get; set; }
        public string teacher_name { get; set; }
        public string course_url { get; set; }
        public string course_img { get; set; }
    }
}
