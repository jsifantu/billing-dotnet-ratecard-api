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
        private static List<string> CsvHeaders;

        static Resource()
        {
            CsvHeaders = new List<string>() {
                "MeterId",
                "Meter Name",
                "Meter Category",
                "Meter Sub Category",
                "Unit",
                "Effective Date",
                "Region",
                "Included Quantity",
                "Meter Status",
                "Tags",
                "Rates"
            };
        }

        public static string GetCSVHeaders()
        {
            var headers = new StringBuilder();
            int index = 0;
            foreach(var header in CsvHeaders) {
                if (index++ < CsvHeaders.Count) {
                    headers.AppendFormat("{0},", header);
                } else {
                    headers.AppendFormat("{0}", header);
                }
            }
            return headers.ToString();
        }

        public string ToCSV()
        {
            var rates = new StringBuilder();
            var tags = new StringBuilder();
            var index = 0;
            
            foreach(var entry in MeterTags) 
            {
                if (index++ < MeterTags.Count) {
                    tags.AppendFormat("[{0}] ", entry);
                } else {
                    tags.AppendFormat("[{0}]", entry);
                }
            }
            index = 0;
            foreach (var entry in MeterRates) {
                if (index++ < MeterRates.Count) {
                    rates.AppendFormat("[{0}]{1} ", entry.Key, entry.Value);
                } else {
                    rates.AppendFormat("[{0}]{1}", entry.Key, entry.Value);
                }
            }

            var meterNameUncoded = MeterName.Replace(",", "");
            var unitUncoded = Unit.Replace(",", "");
            return string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}",
                MeterId, meterNameUncoded, MeterCategory, MeterSubCategory,
                unitUncoded, EffectiveDate, MeterRegion, IncludedQuantity, MeterStatus,
                tags.ToString(), rates.ToString());
        }

    }
}
