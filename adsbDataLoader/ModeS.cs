//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace adsbDataLoader
{
    using System;
    using System.Collections.Generic;
    
    public partial class ModeS
    {
        public int ID { get; set; }
        public string icao_id { get; set; }
        public string callsign { get; set; }
        public Nullable<int> altitude { get; set; }
        public Nullable<int> gndspeed { get; set; }
        public Nullable<int> gndtrack { get; set; }
        public Nullable<decimal> lat { get; set; }
        public Nullable<decimal> lon { get; set; }
        public Nullable<int> vrate { get; set; }
        public Nullable<int> squawk { get; set; }
        public Nullable<System.DateTime> timestamp { get; set; }
        public Nullable<System.DateTime> timestampDate { get; set; }
        public Nullable<System.DateTime> timestampTime { get; set; }
    }
}
