using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraceBack.Model
{
    [Serializable]
    public class base_ent : QueryEntity
    {
        public Guid? ent_id { get; set; }
        public string ent_name { get; set; }
        public string ent_code { get; set; }
        public string ent_business_address { get; set; }
        public string ent_registered_address { get; set; }

        public string ent_address { get; set; }

        public int? row_state { get; set; }
        public Guid? create_user { get; set; }
        public DateTime? create_time { get; set; }
        public Guid? upd_user { get; set; }
        public DateTime? upd_time { get; set; }
    }
}
