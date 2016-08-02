using System;
using System.Configuration;
using RestSharp;

namespace EtsySpy.Classes
{
    public class EtsyApi
    {
        private string baseUrl = string.Empty;
        private string key = string.Empty;

        public EtsyApi()
        {
            
            baseUrl = ConfigurationManager.AppSettings.Get("etsy-baseurl");
            key = ConfigurationManager.AppSettings.Get("etsy-key");
        }

        public T Execute<T>(RestRequest request) where T : new()
        {
            var client = new RestClient(baseUrl);
            request.AddParameter("api_key", ConfigurationManager.AppSettings.Get("etsy-key"));

            var response = client.Execute<T>(request);

            if (response.ErrorException != null)
            {
                const string message = "Error retrieving response.  Check inner details for more info.";
                throw new ApplicationException(message, response.ErrorException);
            }
            return response.Data;
        }


        public EtsyProductResults GetEtsyProduct(string productId)
        {
            var request = new RestRequest();
            request.Resource = "listings/{id}";

            request.Method = Method.GET;

            //request.AddUrlSegment("id", "271402931");
            request.AddUrlSegment("id", productId);

            return Execute<EtsyProductResults>(request);
        }


        public EtsyShopResults GetEtsyShop(string shopQuery)
        {
            var request = new RestRequest();
            request.Resource = "shops/{shopid}";

            request.Method = Method.GET;

            request.AddUrlSegment("shopid", shopQuery);
            request.AddParameter("includes", "Listings:active:100:0");

            return Execute<EtsyShopResults>(request);
        }


    }
}
