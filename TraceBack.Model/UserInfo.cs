using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraceBack.Model
{
    public class UserInfo : QueryEntity
    {
        public Guid? user_id { get; set; }
        public string user_name { get; set; }

        public string user_pwd { get; set; }
        public string user_email { get; set; }

        public string user_phone { get; set; }

        public Guid? user_ent { get; set; }

        public string user_address { get; set; }

        public string ent_name { get; set; }

        public int? row_state { get; set; }

        public string role_id { get; set; }

        public string role_name { get; set; }

        public Guid? create_user { get; set; }

        public DateTime? create_time { get; set; }
        public Guid? upd_user { get; set; }
        public DateTime? upd_time { get; set; }
    }
}
