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
    
    public partial class Role
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Role()
        {
            this.Staff = new HashSet<Staff>();
        }
    
        public int Role_id { get; set; }
        public string Role_Name { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public virtual Role Role1 { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public virtual Role Role2 { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Staff> Staff { get; set; }
    }
}
