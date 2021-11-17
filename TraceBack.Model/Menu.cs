using System;

namespace TraceBack.Model
{
    [Serializable]
    public class Menu : QueryEntity, IComparable<Menu>
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid? menu_id { get; set; }

        public string menu_name_en { get; set; }

        public string menu_name_cn { get; set; }

        public string menu_url { get; set; }

        public string menu_icon { get; set; }

        public int? menu_level { get; set; }

        public Guid? menu_parent { get; set; }

        public string parent_name { get; set; }

        public int display_order { get; set; }

        public string menu_title { get; set; }

        public string menu_remark { get; set; }

        public int row_state { get; set; }

        public Guid? create_user { get; set; }

        public DateTime? create_time { get; set; }

        public Guid? upd_user { get; set; }

        public DateTime? upd_time { get; set; }

        public int CompareTo(Menu other)
        {
            if (other == null)
            {
                return 1;
            }
            else
            {
                return display_order.CompareTo(other.display_order);
            }
        }
    }
}