using System.Text.Json;

partial class Features
{
    public static void CheckForAOsUpdates()
    {
        Console.WriteLine("Checking for Updates");

        string response = Utils.Https.CurlHttpsClient($"https://api.github.com/repos/Light-Lens/AOs/releases/latest");
        GitHubAPIJsonTemplate response_data = JsonSerializer.Deserialize<GitHubAPIJsonTemplate>(response);

        List<AssetsJsonTemplate> assets = response_data.assets;

        int current_version = Obsidian.build_no;
        int latest_version = -1;
        string download_link = "";
        foreach (AssetsJsonTemplate asset in assets)
        {
            if (IsDigit(asset.name))
            {
                download_link = asset.browser_download_url;
                int.TryParse(asset.name, out latest_version);
                break;
            }
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
        }
    }

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
