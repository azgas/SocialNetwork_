//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SocNet_EPX.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class NetworkDb
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public NetworkDb()
        {
            this.LinkDb = new HashSet<LinkDb>();
            this.NetworkFactorsDb = new HashSet<NetworkFactorsDb>();
        }
    
        public long id { get; set; }
        public string name { get; set; }
        public System.DateTime date_created { get; set; }
        public System.DateTime last_modified { get; set; }
        public int service_id { get; set; }
        public bool directed { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LinkDb> LinkDb { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NetworkFactorsDb> NetworkFactorsDb { get; set; }
        public virtual ServiceDb ServiceDb { get; set; }
    }
}
