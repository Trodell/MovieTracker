//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MovieTracker
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserMovies
    {
        public decimal UserID { get; set; }
        public decimal MovieID { get; set; }
    
        public virtual Movie Movie { get; set; }
    }
}