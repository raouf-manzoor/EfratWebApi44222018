//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EfratWebApi.DataBase
{
    using System;
    using System.Collections.Generic;
    
    public partial class RentsInfo
    {
        public int Id { get; set; }
        public int UserID { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public Nullable<System.DateTime> ReturnDate { get; set; }
        public Nullable<int> CarID { get; set; }
        public Nullable<double> TotalCost { get; set; }
        public Nullable<double> CurrentKM { get; set; }
    
        public virtual CarsForRent CarsForRent { get; set; }
        public virtual User User { get; set; }
    }
}
