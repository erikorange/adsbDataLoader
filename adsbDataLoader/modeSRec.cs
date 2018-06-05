using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace adsbDataLoader
{
    public class modeSRec
    {
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
