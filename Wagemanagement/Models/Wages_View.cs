//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Wagemanagement.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Wages_View
    {
        public int WR_id { get; set; }
        public int Staff_id { get; set; }
        public string Staff_Name { get; set; }
        public string Store_Name { get; set; }
        public string GradeName { get; set; }
        public decimal Grade_price { get; set; }
        public Nullable<decimal> SubsidyAmount { get; set; }
        public Nullable<decimal> WR_Bonus { get; set; }
        public Nullable<decimal> Deduction { get; set; }
        public Nullable<decimal> WR_Pay { get; set; }
        public Nullable<decimal> Real_Wage { get; set; }
        public string WR_remarks { get; set; }
        public string pay_of { get; set; }
    }
}
