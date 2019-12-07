using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
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


        public async Task<StatusCheck> AddNewZone(Zone p)
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
                    var t = ex.Message;
                }
            }

            return StatusCheck.Ok;
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
    }

    public class Extent
    {
        public double Top { get; set; }
        public double Bottom { get; set; }
        public double Left { get; set; }
        public double Right { get; set; }
    }
}
