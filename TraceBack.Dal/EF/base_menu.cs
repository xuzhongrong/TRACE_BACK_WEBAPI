//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace TraceBack.Dal.EF
{
    using System;
    using System.Collections.Generic;
    
    public partial class base_menu
    {
        public System.Guid menu_id { get; set; }
        public string menu_name_en { get; set; }
        public string menu_name_cn { get; set; }
        public string menu_url { get; set; }
        public string menu_icon { get; set; }
        public int menu_level { get; set; }
        public System.Guid menu_parent { get; set; }
        public int menu_display_order { get; set; }
        public string menu_title { get; set; }
        public string menu_remark { get; set; }
        public int row_state { get; set; }
        public Nullable<System.Guid> create_user { get; set; }
        public Nullable<System.DateTime> create_time { get; set; }
        public Nullable<System.Guid> upd_user { get; set; }
        public Nullable<System.DateTime> upd_time { get; set; }
    }
}
