using System.Diagnostics;
using System.Text;

namespace Utils
{
    public class Https
    {
        private static HttpClient Http_Client = new();
        private static Process process = new();

        public static string HttpsClient(string url)
        {
            try
            {
                HttpResponseMessage Response = Http_Client.GetAsync(url).Result;
                if (!Response.IsSuccessStatusCode)
                {
                    new Error($"response success status code: {Response}", "HTTPS response error");
                    return "";
                }

                return Response.Content.ReadAsStringAsync().Result;
            }

            catch (Exception e)
            {
                new Error(e.Message, "HTTPS response error");
                EntryPoint.CrashreportLog(e.ToString());
            }

            return "";
        }

        public static string CurlHttpsClient(string url)
        {
            process.StartInfo.FileName = "curl.exe";
            process.StartInfo.Arguments = url;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.RedirectStandardOutput = true;

            try
            {
                process.Start();

                StringBuilder str = new();
                while (!process.HasExited)
                    str.Append(process.StandardOutput.ReadToEnd());

                process.WaitForExit();
                return str.ToString();
            }

            catch (Exception e)
            {
                _ = new Error(e.Message, "runtime error");
                EntryPoint.CrashreportLog(e.ToString());
            }

            return "";
        }

        public static void Reset__()
        {
            Http_Client = new();
        }
    }
}
