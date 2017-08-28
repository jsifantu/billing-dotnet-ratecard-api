using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.EA.Sample
{
    class Program
    {
        const string GetUsageByMonthUrl =
        "https://ea.windowsazure.com/rest/{0}/usage-report?month={1}&type={2}";
        const string GetUsageListUrl =
        "https://ea.windowsazure.com/rest/{0}/usage-reports";
        static void Main(string[] args)
        {
            string EnrollmentNumber = /* Your enrollment number */;
            string AccessToken = /* Token can be created in Manage Access page */;
            // Retrieve a list of available reports
            string Url = string.Format(GetUsageListUrl, EnrollmentNumber);
            string ReportList = CallRestAPI(Url, AccessToken);
            // Directly download a monthly summary report,
            string UsageMonth = /* Request report month "2014-04" */;
            Url = string.Format(GetUsageByMonthUrl, EnrollmentNumber, UsageMonth, "summary");
            string SummaryUsageCSV = CallRestAPI(Url, AccessToken);
            // Directly download a monthly detail report,
            Url = string.Format(GetUsageByMonthUrl, EnrollmentNumber, UsageMonth, "detail");
            string DetailUsageCSV = CallRestAPI(Url, AccessToken);
        }

        static string CallRestAPI(string url, string token)
        {
            WebRequest request = WebRequest.Create(url);
            request.Headers.Add("authorization", "bearer " + token);
            request.Headers.Add("api-version", "1.0");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            return reader.ReadToEnd();
        }
    }
}