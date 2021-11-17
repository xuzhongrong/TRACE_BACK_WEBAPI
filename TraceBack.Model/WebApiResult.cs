using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraceBack.Model
{
    [Serializable]
   public class WebApiResult
    {
        /// <summary>
        /// WebApi 执行结果
        /// </summary>
        public int Result { get; set; }

        /// <summary>
        /// WebApi 消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// WebApi 数据
        /// </summary>
        public object Data { get; set; }

        public int RecordCount { get; set; }
    }
}
