//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AnalysisServices2.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Credentials
    {
        public int id { get; set; }
        public string key { get; set; }
        public string secret { get; set; }
        public int service_id { get; set; }
    
        public virtual ServiceDb ServiceDb { get; set; }
    }
}
