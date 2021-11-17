using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraceBack.Model.UserManage
{
    /// <summary>
    /// 用户查询接口
    /// </summary>
    [Serializable]
    public class QueryUser
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string user_name { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string user_email { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        public string user_role { get; set; }

        /// <summary>
        /// 所属单位名称
        /// </summary>
        public string ent_name { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int? user_state { get; set; }
    }
}
