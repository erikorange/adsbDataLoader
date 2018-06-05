using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace adsbDataLoader
{
    public static class Utils
    {
        public static bool isValidModeS(string dataRow)
        {
            return dataRow.Split(',').Length == 11 ? true : false;
        }

        public static HashSet<string> getMilIDs(string filepath, long totalLines)
        {
            string line;

            long currentLine = 0;
            HashSet<string> hash = new HashSet<string>();
            using (StreamReader reader = new StreamReader(filepath))
            {
                line = reader.ReadLine();   // skip header row
                while (!reader.EndOfStream)
                {
                    line = reader.ReadLine();
                    if (isValidModeS(line))
                    {
                        if (isMilCallsign(getCallsign(line)))
                        {
                            hash.Add(getIcaoID(line));
                        }
                    }
                    currentLine++;
                    Logger.logSameLineNoCr(String.Format("        Pass 1: Mil callsigns: {0}  [{1}/{2}]",
                        hash.Count.ToString(), currentLine.ToString(), totalLines.ToString()), ConsoleColor.Magenta);
                }
            }
            Console.WriteLine();
            return hash;
        }

        public static string getCallsign(string dataRow)
        {
            return dataRow.Split(',')[3].Replace(" ", String.Empty);
        }

        public static string getIcaoID(string dataRow)
        {
            return dataRow.Split(',')[0];
        }

        //# starts with at least 4 letters, then at least 2 numbers; or starts with RCH or TOPCAT; or is GOTOFMS.  Remove spaces for VADER xx
        //match = re.search(r'(^[A-Z]{4,}[0-9]{2,}$)|(^RCH)|(^TOPCAT)|(GOTOFMS)', cs.replace(' ',''))
        public static bool isMilCallsign(string cs)
        {
            Match m = Regex.Match(cs, @"(^[A-Z]{4,}[0-9]{2,}$)|(^RCH)|(^TOPCAT)|(GOTOFMS)");
            return m.Success;
        }

        public static modeSRec createModeSRec(string dataRow)
        {
            string[] dataVals = dataRow.Split(',');
            modeSRec sRec = new modeSRec();
            sRec.icao_id = dataVals[0];
            sRec.callsign = dataVals[3];
            sRec.altitude = parseInt(dataVals[4]);
            sRec.gndspeed = parseInt(dataVals[5]);
            sRec.gndtrack = parseInt(dataVals[6]);
            sRec.lat = parseDecimal(dataVals[7]);
            sRec.lon = parseDecimal(dataVals[8]);
            sRec.vrate = parseInt(dataVals[9]);
            sRec.squawk = parseInt(dataVals[10]);
            DateTime dt = DateTime.Parse(dataVals[1] + " " + dataVals[2]);
            sRec.timestamp = dt;
            sRec.timestampDate = dt.Date;
            DateTime theTime = DateTime.Parse(dataVals[2]);
            sRec.timestampTime = theTime;
            return sRec;
        }

        private static Nullable<int> parseInt(string value)
        {
            int number;
            if (Int32.TryParse(value, out number))
            {
                return number;
            }
            else
            {
                return null;
            }
        }

        private static Nullable<decimal> parseDecimal(string value)
        {
            decimal number;
            if (Decimal.TryParse(value, out number))
            {
                return number;
            }
            else
            {
                return null;
            }
        }

    }
}

