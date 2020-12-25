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
        // bus stop
        public string GetSrcUrl(string stationNum)
        {
            const string AND_SIGN = "%26";
            const string STATION_PARAM = "MonitoringRef=";
            const string CALLS = "StopVisitDetailLevel=calls";

            return STATION_PARAM + stationNum + AND_SIGN + CALLS;
        }

        // train station
        public string GetSrcUrl(int stationNumber)
        {
            const string AND_SIGN = "%26";
            const string STATION_PARAM = "MonitoringRef=";
            const string CALLS = "StopVisitDetailLevel=calls";

            return STATION_PARAM + stationNumber + AND_SIGN + CALLS;
        }

        // bus line
        public string GetSrcUrl(Activity activity, int routeIdOfDirectionChoosen)
        {
            const string AND_SIGN = "%26";
            const string STATION_PARAM = "MonitoringRef=all";
            const string LINE_PARAM = "LineRef=";
            const string CALLS = "StopVisitDetailLevel=calls";

            if (routeIdOfDirectionChoosen == 0)
            {
                Alert.AlertMessage(activity, "מספר הקו לא מופיע במערכת");
                return null;
            }

            else
            {
                return STATION_PARAM + AND_SIGN + LINE_PARAM + routeIdOfDirectionChoosen + AND_SIGN + CALLS;
            }
        }

    }
}