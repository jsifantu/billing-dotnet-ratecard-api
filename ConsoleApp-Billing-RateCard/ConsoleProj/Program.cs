using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Net;
using System.IO;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Newtonsoft.Json;
using System.Configuration; //BL

namespace ARMAPI_Test
{
// #error Please update the appSettings section in app.config, then remove this statement

    class Program
    {
        //This is a sample console application that shows you how to grab a User token from AAD for the current user of the app
        //The same caveat remains, that the current user of the app needs to be part of either the Owner, Reader or Contributor role for the requested AzureSubID.
        static void Main(string[] args)
        {
            try 
            {
                //Get the AAD User token to get authorized to make the call to the Usage API
                string token;
                var tokenFilePath = Environment.CurrentDirectory + "\\oathtoken.txt";
                if (File.Exists(tokenFilePath)) {
                    if (!IsOlderThanOneHour(File.GetCreationTime(tokenFilePath))) {
                        using (var reader = new StreamReader(tokenFilePath)) {
                            token = reader.ReadToEnd();
                        }
                    } else {
                        DeleteTokenFile(tokenFilePath);
                        token = GenerateTokenFile(tokenFilePath);                        
                    }
                } else {
                    token = GenerateTokenFile(tokenFilePath);
                }
                /*Setup API call to RateCard API
                 Callouts:
                 * See the App.config file for all AppSettings key/value pairs
                 * You can get a list of offer numbers from this URL: http://azure.microsoft.com/en-us/support/legal/offer-details/
                 * You can configure an OfferID for this API by updating 'MS-AZR-{Offer Number}'
                 * The RateCard Service/API is currently in preview; please use "2015-06-01-preview" or "2016-08-31-preview" for api-version (see https://msdn.microsoft.com/en-us/library/azure/mt219005 for details)
                 * Please see the readme if you are having problems configuring or authenticating: https://github.com/Azure-Samples/billing-dotnet-ratecard-api
                 */
                char[] separators = { ',', ';' };
                var offers = ConfigurationManager.AppSettings["Offers"].Split(separators);
                foreach (var offerName in offers) {
                    try {
                        ProcessOffer(token, offerName);
                    } catch (WebException we) {
                        Console.WriteLine(String.Format("{0} \n\n{1}",
                            we.Message, we.InnerException != null ? we.InnerException.Message : ""));
                    }
                }
            } catch(Exception e)  {
                Console.WriteLine(String.Format("{0} \n\n{1}", e.Message, e.InnerException != null ? e.InnerException.Message : ""));
            }
            Console.WriteLine("Press the Return key to exit.");
            Console.ReadLine();
        }

        private static void DeleteTokenFile(string tokenFilePath)
        {
            File.Delete(tokenFilePath);
        }

        private static string GenerateTokenFile(string tokenFilePath)
        {
            string token = GetOAuthTokenFromAAD();
            using (var writer = new StreamWriter(tokenFilePath)) {
                writer.WriteLine(token);
            }

            return token;
        }

        private static bool IsOlderThanOneHour(DateTime fileTime)
        {
            var span1 = DateTime.Now - fileTime;
            var span2 = TimeSpan.FromHours(1);
            return span1 > span2;
        }

        private static void ProcessOffer(string token, string offerName)
        {
            var url = string.Format("providers/Microsoft.Commerce/RateCard?api-version=2016-08-31-preview&$filter=OfferDurableId eq '{0}' and Currency eq 'USD' and Locale eq 'en-US' and RegionInfo eq 'US'",
                offerName);
            // Build up the HttpWebRequest
            string requestURL = String.Format("{0}/{1}/{2}/{3}",
                       ConfigurationManager.AppSettings["ARMBillingServiceURL"],
                       "subscriptions",
                       ConfigurationManager.AppSettings["SubscriptionID"],
                       url);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestURL);

            // Add the OAuth Authorization header, and Content Type header
            request.Headers.Add(HttpRequestHeader.Authorization, "Bearer " + token);
            // request.Date = DateTime.UtcNow;
            request.ContentType = "application/json";

            // Call the RateCard API, dump the output to the console window

            // Call the REST endpoint
            Console.WriteLine("Calling RateCard service...");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Console.WriteLine(String.Format("RateCard service response status: {0}", response.StatusDescription));
            Stream receiveStream = response.GetResponseStream();

            // Pipes the stream to a higher level stream reader with the required encoding format. 
            using (var readStream = new StreamReader(receiveStream, Encoding.UTF8)) {
                var rateCardResponse = readStream.ReadToEnd();
#if SHOW_OUTPUT
                    Console.WriteLine("RateCard stream received.  Press ENTER to continue with raw output.");
                    Console.ReadLine();
                    Console.WriteLine(rateCardResponse);
                    Console.WriteLine("Raw output complete.  Press ENTER to continue with JSON output.");
                    Console.ReadLine();
#endif
                var filePath = string.Format("{0}\\{1}.json", Environment.CurrentDirectory, offerName);
                Console.WriteLine("Writing response to JSON file " + filePath);
                using (var writer = new StreamWriter(filePath)) {
                    writer.WriteLine(rateCardResponse);
                }
                // Convert the Stream to a strongly typed RateCardPayload object.  
                // You can also walk through this object to manipulate the individuals member objects. 
                RateCardPayload payload = JsonConvert.DeserializeObject<RateCardPayload>(rateCardResponse);
                // Console.WriteLine(rateCardResponse.ToString());
                filePath = string.Format("{0}\\{1}.csv", Environment.CurrentDirectory, offerName);
                Console.WriteLine("Writing response to CSV file " + filePath);
                using (var writeStream = new StreamWriter(filePath)) {
                    payload.ToCSV(writeStream);
                }
            }
            response.Close();
            receiveStream.Close();
        }


        public static string GetOAuthTokenFromAAD()
        {
            var authenticationContext = new AuthenticationContext(  String.Format("{0}/{1}",
                                                                    ConfigurationManager.AppSettings["ADALServiceURL"],
                                                                    ConfigurationManager.AppSettings["TenantDomain"]));

            //Ask the logged in user to authenticate, so that this client app can get a token on his behalf
            var result = authenticationContext.AcquireToken(String.Format("{0}/",ConfigurationManager.AppSettings["ARMBillingServiceURL"]),
                                                            ConfigurationManager.AppSettings["ClientID"],
                                                            new Uri(ConfigurationManager.AppSettings["ADALRedirectURL"]),
                                                            PromptBehavior.Always);

            if (result == null)
            {
                throw new InvalidOperationException("Failed to obtain the JWT token");
            }

            return result.AccessToken;
        }
    }
}

