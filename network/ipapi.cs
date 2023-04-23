using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace network
{
    internal class GeoIPData
    {
        [JsonPropertyName("query")]
        public string Query { get; set; }
        [JsonPropertyName("status")]
        public string Status { get; set; }
        [JsonPropertyName("country")]
        public string Country { get; set; }
        [JsonPropertyName("countryCode")]
        public string CountryCode { get; set; }

        [JsonPropertyName("regionName")]
        public string RegionName { get; set; }
        [JsonPropertyName("city")]
        public string City { get; set; }
        [JsonPropertyName("zip")]
        public string Zip { get; set; }
        
        [JsonPropertyName("lat")]
        public float Lat { get; set; }
        [JsonPropertyName("lon")]
        public float Lon { get; set; }
        [JsonPropertyName("timezone")]
        public string Timezone { get; set; }
        [JsonPropertyName("isp")]
        public string ISP { get; set; }
        [JsonPropertyName("org")]
        public string Organization { get; set; }
        [JsonPropertyName("as")]
        public string AS { get; set; }

        public GeoIPData()
        {
            Status = String.Empty;
            Country = String.Empty;
            CountryCode = String.Empty;
            RegionName = String.Empty;
            City = String.Empty;
            Zip = String.Empty;
            Lat = 0;
            Lon = 0;
            Timezone = String.Empty;
            ISP = String.Empty;
            Organization = String.Empty;
            AS = String.Empty;
        }
    }


    internal class IPClientApi
    {
        protected string url = "http://ip-api.com/json/";
        protected HttpClient client;
        public string LastError { get; set; }

        internal IPClientApi()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<GeoIPData> SearchIP(string value)
        {
            LastError= String.Empty;
            try
            {
                Debug.WriteLine("Connecting to ip-api.com");
                HttpResponseMessage response = await client.GetAsync(value);
                var json = await response.Content.ReadAsStringAsync();
                if ((int)response.StatusCode == 200)
                {
                    var data = JsonSerializer.Deserialize<GeoIPData>(json.ToString());
                    Debug.WriteLine("Return data code=200");
                    return data;
                }
                else
                {
                    Debug.WriteLine("Return StatusCode=" + response.StatusCode.ToString());
                    LastError = "Error status code "+response.StatusCode.ToString(); 
                    return null;
                }

            }
            catch (Exception error)
            {
                Debug.WriteLine("Exception:" + error.Message);
                LastError = error.Message;
                return null;
            }
        }
    }
}
