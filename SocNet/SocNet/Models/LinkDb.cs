//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SocNet.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class LinkDb
    {
        public int id { get; set; }
        public long network_id { get; set; }
        public long source_id { get; set; }
        public long target_id { get; set; }
        public System.DateTime date_modified { get; set; }
    
        public virtual NetworkDb NetworkDb { get; set; }
        public virtual VertexDb VertexDb { get; set; }
        public virtual VertexDb VertexDb1 { get; set; }
    }
}
