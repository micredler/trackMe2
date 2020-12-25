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
        const string AND_SIGN = "%26";
        const string STATION_PARAM = "MonitoringRef=";
        const string CALLS = "StopVisitDetailLevel=calls";
        const string LINE_PARAM = "LineRef=";

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
        // bus stop
        public string GetSrcUrl(string stationNum)
        {
            return STATION_PARAM + stationNum + AND_SIGN + CALLS;
        }


        // bus line
        public string GetSrcUrl(Activity activity, int routeIdOfDirectionChoosen)
        {
            
            if (routeIdOfDirectionChoosen == 0)
            {
                Alert.AlertMessage(activity, "מספר הקו לא מופיע במערכת");
                return null;
            }

            else
            {
                return STATION_PARAM + "all" + AND_SIGN + LINE_PARAM + routeIdOfDirectionChoosen + AND_SIGN + CALLS;
            }
        }

        // train station
        public string GetSrcUrl(int stationNumber)
        {
            return STATION_PARAM + stationNumber + AND_SIGN + CALLS;
        }
    }
}