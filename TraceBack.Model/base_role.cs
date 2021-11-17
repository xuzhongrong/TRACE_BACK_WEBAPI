using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraceBack.Model
{
    [Serializable]
    public class base_role : QueryEntity
    {
        public Guid? role_id { get; set; }
        public string role_name { get; set; }
        public string role_remark { get; set; }
        public int row_state { get; set; }
        public Guid? create_user { get; set; }
        public DateTime? create_time { get; set; }
        public Guid? upd_user { get; set; }
        public DateTime? upd_time { get; set; }
    }
}
