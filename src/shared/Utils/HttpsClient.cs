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
                _ = new Error(e.Message, "HTTPS response error");
                EntryPoint.CrashreportLog(e.ToString());
            }

            return "";
        }
    }
}
