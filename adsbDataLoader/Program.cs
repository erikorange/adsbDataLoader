using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace adsbDataLoader
{
    class Program
    {
        static void Main(string[] args)
        {
            const string ROOT_PATH = @"C:\Users\Erik\Documents\Raspberry Pi\rPi Model 3 B\adsb-logger\logs";
            adsbEntities modeSctx;
            HashSet<string> milIdSet;

            string[] adsbFiles1 = FileManager.getAdsbFiles(ROOT_PATH);
            long sum = 0;
            foreach (string theFile in adsbFiles1)
            {
                sum += File.ReadLines(theFile).Count() - 1; // subtract 1 line to ignore header row
            }
            Console.WriteLine(string.Format("{0:n0}", sum));
            Console.ReadKey();
            return;

            Console.CursorVisible = false;

            string[] adsbFiles = FileManager.getAdsbFiles(ROOT_PATH);
            int numFiles = adsbFiles.Length;
            if (numFiles == 0)
            {
                Logger.log("No files found.", ConsoleColor.Red, true);
                return;
            }

            DateTime start = DateTime.Now;
            Logger.log("Start: " + start.ToString(), ConsoleColor.Cyan);

            int idx = 1;

            using (adsbEntities fileCtx = new adsbEntities())
            {
                foreach (string filepath in adsbFiles)
                {
                    string filename = Path.GetFileName(filepath);
                    Logger.log(String.Format(@"[{0}/{1}] {2}\{3}",
                        idx++.ToString("00"), numFiles.ToString(), FileManager.getParentDir(filepath), filename),
                        ConsoleColor.Green);

                    if (fileCtx.DataFiles.FirstOrDefault(d => d.filename == filename) is null)
                    {
                        SaveFilename(fileCtx, filename);

                        long totalLines = File.ReadLines(filepath).Count() - 1; // subtract 1 line to ignore header row

                        // Pass 1 - get the ICAO Ids for Mil callsigns only
                        milIdSet = Utils.getMilIDs(filepath, totalLines);

                        if (milIdSet.Count > 0)
                        {
                            // Pass 2 - only save records with Mil ICAO IDs
                            string outMsg = "        Pass 2: Mil Mode S records: {0}  [{1}/{2}]";
                            using (var reader = new StreamReader(filepath))
                            {
                                string header = reader.ReadLine();  // skip header row
                                int batch = 0;
                                int milRecords = 0;
                                bool atLeastOne = false;

                                modeSctx = new adsbEntities();
                                modeSctx.Configuration.AutoDetectChangesEnabled = false;     // better performance for bulk load
                                long currentLine = 0;
                                while (!reader.EndOfStream)
                                {
                                    var line = reader.ReadLine();
                                    currentLine++;

                                    if (Utils.isValidModeS(line))
                                    {
                                        if (milIdSet.Contains(Utils.getIcaoID(line)))
                                        {
                                            milRecords++;
                                            atLeastOne = true;
                                            SaveModeSRec(modeSctx, line);

                                            batch++;
                                            if (batch % 100000 == 0)
                                            {
                                                Logger.logSameLineNoCr(String.Format(outMsg + " batch saving...", milRecords.ToString(), currentLine.ToString(), totalLines.ToString()), ConsoleColor.Magenta);

                                                modeSctx.SaveChanges();
                                                modeSctx.Dispose();
                                                Logger.logSameLineNoCr(String.Format(outMsg + "                ", milRecords.ToString(), currentLine.ToString(), totalLines.ToString()), ConsoleColor.Magenta);

                                                modeSctx = new adsbEntities();
                                                modeSctx.Configuration.AutoDetectChangesEnabled = false;
                                                atLeastOne = false;
                                                batch = 0;
                                            }

                                        }
                                    }
                                    Logger.logSameLineNoCr(String.Format(outMsg, milRecords.ToString(), currentLine.ToString(), totalLines.ToString()), ConsoleColor.Magenta);
                                }

                                if (atLeastOne)
                                {
                                    modeSctx.SaveChanges();
                                }
                                modeSctx.Dispose();
                                Console.WriteLine();
                                Logger.log("        Done", ConsoleColor.Yellow);
                            }
                        }
                    }
                    else
                    {
                        Logger.log("        Already loaded", ConsoleColor.Yellow);
                    }
                }
            }
            DateTime end = DateTime.Now;
            Logger.log("End: " + start.ToString(), ConsoleColor.Cyan);
            Logger.log("Elapsed time: " + (end - start).ToString(@"hh\:mm\:ss"), ConsoleColor.Cyan);

            Logger.log("Press any key...", ConsoleColor.Red, true);
        }

        private static void SaveModeSRec(adsbEntities modeSctx, string line)
        {
            modeSRec msr = Utils.createModeSRec(line);
            ModeS ms = new ModeS();
            ms.icao_id = msr.icao_id;
            ms.callsign = msr.callsign;
            ms.altitude = msr.altitude;
            ms.gndspeed = msr.gndspeed;
            ms.gndtrack = msr.gndtrack;
            ms.lat = msr.lat;
            ms.lon = msr.lon;
            ms.squawk = msr.squawk;
            ms.timestamp = msr.timestamp;
            ms.timestampDate = msr.timestampDate;
            ms.timestampTime = msr.timestampTime;
            modeSctx.ModeS.Add(ms);
        }

        private static void SaveFilename(adsbEntities fileCtx, string filename)
        {
            DataFile newDf = new DataFile();
            newDf.filename = filename;
            fileCtx.DataFiles.Add(newDf);
            fileCtx.SaveChanges();
        }
    }
}
