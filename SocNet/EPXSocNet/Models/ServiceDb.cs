//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EPXSocNet.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ServiceDb
    {
        public ServiceDb()
        {
            this.Credentials = new HashSet<Credentials>();
            this.NetworkDb = new HashSet<NetworkDb>();
            this.VertexDb = new HashSet<VertexDb>();
        }
    
        public int id { get; set; }
        public string name { get; set; }
    
        public virtual ICollection<Credentials> Credentials { get; set; }
        public virtual ICollection<NetworkDb> NetworkDb { get; set; }
        public virtual ICollection<VertexDb> VertexDb { get; set; }
    }
}
