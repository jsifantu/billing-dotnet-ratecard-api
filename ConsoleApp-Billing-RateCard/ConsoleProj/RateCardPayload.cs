using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ARMAPI_Test
{
    public class RateCardPayload
    {
        public List<AzureOfferTerms> OfferTerms { get; set; }
        public List<Resource> Meters { get; set; }
        public string Currency { get; set; }
        public string Locale { get; set; }
        public string RatingDate { get; set; }
        public bool IsTaxIncluded { get; set; }

        public void ToCSV(StreamWriter writer)
        {
            writer.NewLine = Environment.NewLine;
            foreach (var term in OfferTerms) {
                term.ToCSV(writer);
            }
            writer.WriteLine(Resource.GetCSVHeaders());
            foreach(var meter in Meters) {
                writer.WriteLine(meter.ToCSV());
            }
        }
    }
    
}
