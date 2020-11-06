using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Util.Functions;
using Org.Json;

namespace trackMe.BL
{
    class ApiService
    {
        const string BASE_URL = "http://www.ravgioraredler.co.il/api/tracks/getTracksData?url=";


        public async Task<ApiResponse> GetDataFromApi(string urlParam)
        {
            string apiUrl = BASE_URL + urlParam;
            string data = "";

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Accept.Clear();

                HttpResponseMessage response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    data = await response.Content.ReadAsStringAsync();
                    data = data.Replace("-version", "version");
                    ApiResponse table = Newtonsoft.Json.JsonConvert.DeserializeObject<ApiResponse>(data);

                    return table;
                }

            }
            return null;

        }

    }
}