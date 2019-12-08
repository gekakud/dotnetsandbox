using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WpfTest.Data
{
    enum StatusCheck
    {
        Ok = 0,
        FuckedUp = 1
    }

    class DataModel
    {
        private const string URL = "https://rideondev.westeurope.cloudapp.azure.com/api";
        private const string allZones = "/zones/allzones";
        private const string createZone = "/zones/create";

        private const string getHeights = "/getheights";
        private const string getMap = "/getmap";

        private static DataModel s_instance;
        private ZonesData _zonesData;

        private DataModel()
        {
            _zonesData = new ZonesData();
        }

        public static DataModel Instance
        {
            get { return s_instance ?? (s_instance = new DataModel()); }
        }


        public async Task<string> AddNewZone(Zone p)
        {
            _zonesData.AllZones.Add(p);

            using (var wc = new WebClient())
            {
                try
                {
                    wc.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                    Uri uri = new Uri(URL + createZone);
                    var bodyData = JsonConvert.SerializeObject(p.Extent);
                    var json = wc.UploadStringTaskAsync(uri + "/" + p.Name, "POST", bodyData);
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }

            return "OK";
        }

        private async Task FetchAllZones()
        {
            using (var wc = new WebClient())
            {
                try
                {
                    Uri uri = new Uri(URL + allZones);
                    var json = await wc.DownloadStringTaskAsync(uri);
                    _zonesData.AllZones = JsonConvert.DeserializeObject<List<Zone>>(json);
                }
                catch (Exception)
                {
                    
                }
            }

        }

        public async Task<string> FetchZoneZipped(string zoneName = "Canillo")
        {
            if (_zonesData.AllZones.Exists(z => z.Name == zoneName))
            {
                

                var infoFile = "info.json";
                var heightsFile = "heights.json";
                var mapdata = "mapdata.json";

                var zoneDirName = zoneName;

                var allFilesDir = Path.Combine(Directory.GetCurrentDirectory(), zoneDirName);
                if (!Directory.Exists(allFilesDir))
                {
                    Directory.CreateDirectory(allFilesDir);
                }

                System.IO.DirectoryInfo di = new DirectoryInfo(allFilesDir);
                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }

                var zone =_zonesData.AllZones.First(z => z.Name == zoneName);
                // Write the string array to a new file
                // INFO
                using (StreamWriter outputFile = new StreamWriter(Path.Combine(allFilesDir, infoFile)))
                {
                    var zoneToFetchJson = JsonConvert.SerializeObject(zone);
                    outputFile.Write(zoneToFetchJson);
                }

                // Write the string array to a new file
                //HEIGHTS
                using (StreamWriter outputFile = new StreamWriter(Path.Combine(allFilesDir, heightsFile)))
                {
                    using (var wc = new WebClient())
                    {
                        try
                        {
                            wc.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                            //Uri uri = new Uri(URL + getHeights + $"/{zone.Id}");
                            //var json = await wc.DownloadStringTaskAsync(uri);
                            Uri uri = new Uri(URL + getHeights + "/" + zone.Id.ToString());
                            var json = await wc.DownloadStringTaskAsync(uri);
                            outputFile.Write(json);
                        }
                        catch (Exception ex)
                        {
                            var t = 5;
                        }
                    }
                }

                
                // Write the string array to a new file
                //MAPDATA
                using (StreamWriter outputFile = new StreamWriter(Path.Combine(allFilesDir, mapdata)))
                {
                    using (var wc = new WebClient())
                    {
                        try
                        {
                            wc.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                            Uri uri = new Uri(URL + getMap + "/" + zone.Id);
                            var json = await wc.DownloadStringTaskAsync(uri);
                            outputFile.Write(json);
                        }
                        catch (Exception)
                        {

                        }
                    }
                }


            }
            else
            {
                return "No such zone";
            }

            

            return "";
        }

        public async Task<string> GetUpdatedList()
        {
            await FetchAllZones();
            if (_zonesData.AllZones.Count == 0)
            {
                return "List is empty";
            }

            StringBuilder zonesList = new StringBuilder();
            foreach (var zone in _zonesData.AllZones)
            {
                zonesList.Append("Name:"+zone.Name+", Zoom:");
                zonesList.AppendLine(zone.Zoom.ToString());
            }
            return zonesList.ToString();
        }
    }

    class ZonesData
    {
        public List<Zone> AllZones;

        public ZonesData()
        {
            AllZones = new List<Zone>();
        }
    }

    public class Zone
    {
        public string Name { get; set; }
        public double CenterLatitude { get; set; }
        public double CenterLongitude { get; set; }
        public double DefaultHelmetLatitude { get; set; }
        public double DefaultHelmetLongitude { get; set; }
        public double DefaultHelmetAltitude { get; set; }
        public Extent Extent { get; set; }
        public string Country { get; set; }
        public string CountryCode { get; set; }
        public string State { get; set; }
        public string StateCode { get; set; }
        public int Zoom { get; set; }
        public Guid Id { get; set; }

    }

    public class Extent
    {
        public double Top { get; set; }
        public double Bottom { get; set; }
        public double Left { get; set; }
        public double Right { get; set; }
    }
}
