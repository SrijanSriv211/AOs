using System.Net.Http.Json;
using System.Text.Json;

namespace Utils
{
    public class Https
    {
        private readonly HttpClient client = new();

        public string HttpsClient(string url)
        {
            try
            {
                HttpResponseMessage response = client.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                    return response.Content.ReadAsStringAsync().Result;

                else
                    return "";
            }

            catch (Exception e)
            {
                new Error(e.Message);
            }

            return "";
        }

        public T HttpsClientJson<T>(string url)
        {
            try
            {
                HttpResponseMessage response = client.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                    return JsonSerializer.Deserialize<T>(response.Content.ReadAsStringAsync().Result);

                else
                    return default;
            }

            catch (Exception e)
            {
                new Error(e.Message);
            }

            return default;
        }
    }
}
