//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FlickrServices.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class VertexDb
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public VertexDb()
        {
            this.LinkDb = new HashSet<LinkDb>();
            this.LinkDb1 = new HashSet<LinkDb>();
            this.VertexFactorsDb = new HashSet<VertexFactorsDb>();
        }
    
        public long id { get; set; }
        public string identifier { get; set; }
        public string name { get; set; }
        public int service_id { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LinkDb> LinkDb { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LinkDb> LinkDb1 { get; set; }
        public virtual ServiceDb ServiceDb { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VertexFactorsDb> VertexFactorsDb { get; set; }
    }
}
