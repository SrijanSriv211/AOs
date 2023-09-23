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
                EntryPoint.CrashreportLogging(e.ToString());
            }

            return "";
        }
    }
}
