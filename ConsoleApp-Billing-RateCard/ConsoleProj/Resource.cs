using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARMAPI_Test
{
    public class Resource
    {
        public string MeterId { get; set; }
        public string MeterName { get; set; }
        public string MeterCategory { get; set; }
        public string MeterSubCategory { get; set; }
        public string Unit { get; set; }
        public Dictionary<string, double> MeterRates { get; set; }
        public string EffectiveDate { get; set; }
        public List<string> MeterTags { get; set; }
        public string MeterRegion { get; set; }
        public double IncludedQuantity { get; set; }
        public string MeterStatus { get; set; }

        static public string GetCSVHeaders()
        {
            return "MeterId, Meter Name, Meter Category, Meter Sub Category, "
                + "Unit, Rates, Effective Date, Tags, Region, Quantity, Meter Status";
        }

        public string ToCSV()
        {
            var rates = new StringBuilder();
            var tags = new StringBuilder();
            var index = 0;
            foreach(KeyValuePair<string, double> entry in MeterRates) {
                rates.AppendFormat("{0}:{1} ", entry.Key, entry.Value);
            }
            foreach(var entry in MeterTags) 
            {
                index++;
                if (index < MeterTags.Count) {
                    tags.AppendFormat("{0} |", entry);
                } else {
                    tags.AppendFormat("{0}", entry);
                }
            }

            return string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}",
                MeterId, MeterName, MeterCategory, MeterSubCategory,
                Unit, rates.ToString(), EffectiveDate, tags,
                MeterRegion, IncludedQuantity, MeterStatus);
        }

    }
}
