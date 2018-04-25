using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Net.Http;

namespace WeatherApp
{
    class DataService
    {
        public static async Task<dynamic> getDataFromService(string pQueryString)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(pQueryString);
            dynamic data = null;
            if (response != null)
            {
                string json = response.Content.ReadAsStringAsync().Result;
                data = JsonConvert.DeserializeObject(json);
            }

            return data;
        }
    }
}
