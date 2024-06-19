using System.Text.Json;

partial class Features
{
    public static void CheckForAOsUpdates()
    {
        Console.WriteLine("Checking for Updates");

        // Send a request using curl and wait for the response containing a json text.
        string response = Utils.Https.CurlHttpsClient($"https://api.github.com/repos/SrijanSriv211/AOs/releases/latest");
        // Deserialize the response into a usable json object with the help of 'GitHubAPIJsonTemplate' class
        GitHubAPIJsonTemplate response_data = JsonSerializer.Deserialize<GitHubAPIJsonTemplate>(response);
        // Load details for all assests from the json object
        List<AssetsJsonTemplate> assets = response_data.assets;

        int current_version = Obsidian.build_no;
        int latest_version = -1;
        string download_link = "";

        // Loop through each assest and check whether the filename of any assest is a number or not.
        // If yes, then it means that file is the build number of that AOs version,
        // pull the build version in 'latest_version' var and download link of that AOs version.
        foreach (AssetsJsonTemplate asset in assets)
        {
            if (latest_version > -1 && !Utils.String.IsEmpty(download_link))
                break;

            else if (IsDigit(asset.name))
                int.TryParse(asset.name, out latest_version);

            else if (asset.name == "AOs.zip")
                download_link = asset.browser_download_url;
        }

        if (current_version >= latest_version)
            Console.WriteLine("You're up to date.");

        else
        {
            Console.WriteLine("Updates are available.");
            Console.Write("Your version: ");
            Terminal.Print(current_version.ToString(), ConsoleColor.White);
            Console.Write("Latest version: ");
            Terminal.Print(latest_version.ToString(), ConsoleColor.White);
            Console.Write("Get the Latest version here: ");
            Terminal.Print(download_link, ConsoleColor.Cyan);
            // Download the latest version of AOs
            SystemUtils.StartApp(download_link);
        }
    }

    // Loop through each letter in a string and check whether the string is a number or not.
    private static bool IsDigit(string str)
    {
        foreach (char c in str)
        {
            if (!char.IsDigit(c))
                return false;
        }
        return true;
    }

    private class GitHubAPIJsonTemplate
    {
        public string url { get; set; }
        public string assets_url { get; set; }
        public string html_url { get; set; }
        public string tag_name { get; set; }
        public List<AssetsJsonTemplate> assets { get; set; }
    }

    private class AssetsJsonTemplate
    {
        public string name { get; set; }
        public string browser_download_url { get; set; }
    }
}
