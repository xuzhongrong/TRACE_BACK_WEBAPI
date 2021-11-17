using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraceBack.Model
{
    public class Column: QueryEntity
    {
        public Guid? column_id { get; set; }
        public string column_name { get; set; }
        public int column_level { get; set; }
        public Guid? column_parent { get; set; }
        public string column_explain { get; set; }
    }
}
