using System.Diagnostics;
using System.Security.Principal;

partial class Obsidian
{
    public static string default_else_shell = "cmd.exe";

    public readonly static string about_AOs = "A Developer Command-line Tool Built for Developers by a Developer.";
    public readonly static string AOs_repo_link = "https://github.com/Light-Lens/AOs";

    public readonly static string root_dir = AppDomain.CurrentDomain.BaseDirectory;
    public readonly static bool is_admin = new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
    public readonly static string AOs_binary_path = Process.GetCurrentProcess().MainModule.FileName;
    public readonly static int build_no = FileVersionInfo.GetVersionInfo(AOs_binary_path).FileBuildPart;
    public readonly static string version_no = FileVersionInfo.GetVersionInfo(AOs_binary_path).ProductVersion;
    public readonly static ConsoleColor original_foreground_color = Console.ForegroundColor;
}
