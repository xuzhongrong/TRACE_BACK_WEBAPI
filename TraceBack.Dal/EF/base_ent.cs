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
    
    public partial class base_ent
    {
        public System.Guid ent_id { get; set; }
        public string ent_name { get; set; }
        public string ent_code { get; set; }
        public string ent_business_address { get; set; }
        public string ent_registered_address { get; set; }
        public int row_state { get; set; }
        public Nullable<System.Guid> create_user { get; set; }
        public Nullable<System.DateTime> create_time { get; set; }
        public Nullable<System.Guid> upd_user { get; set; }
        public Nullable<System.DateTime> upd_time { get; set; }
    }
}
