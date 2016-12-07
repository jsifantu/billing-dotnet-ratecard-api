using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ARMAPI_Test
{
    public class AzureOfferTerms
    {
        public string Name { get; set; }
        public double Credit { get; set; }
        public List<string> ExcludedMeterIds { get; set; }
        public string EffectiveDate { get; set; }

        static public string GetCSVHeaders()
        {
            return "Name, Credit, Excluded Meter IDs, Effective Date";
        }

        public void ToCSV(StreamWriter writer)
        {
            writer.NewLine = Environment.NewLine;
            writer.WriteLine("Offer Terms");
            writer.WriteLine(GetCSVHeaders());
            var index = 0;
            if (ExcludedMeterIds != null) {
                foreach (var entry in ExcludedMeterIds) {
                    if (index == 0) {
                        writer.WriteLine(string.Format("{0},{1},{2},{3}", Name, Credit, entry, EffectiveDate));
                    } else {
                        writer.WriteLine(string.Format(" , ,{0}, ", entry));
                    }
                    index++;
                }
            }
            writer.WriteLine();
        }
    }
}
