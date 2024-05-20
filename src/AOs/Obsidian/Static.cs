using System.Diagnostics;
using System.Security.Principal;

partial class Obsidian
{
    public static string default_else_shell = "cmd.exe";

    public readonly static string SessionTime = DateTime.Now.ToString("[dd-MM-yyyy HH:mm:ss]");
    public readonly static string about_AOs = "A command-line tool built to control your OS directly through the command-line";
    public readonly static string AOs_repo_link = "https://github.com/SrijanSriv211/AOs";

    public readonly static string root_dir = AppDomain.CurrentDomain.BaseDirectory;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "<Pending>")]
    public readonly static bool is_admin = new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);

    public readonly static string AOs_binary_path = Environment.ProcessPath;
    public readonly static int build_no = FileVersionInfo.GetVersionInfo(AOs_binary_path).FileBuildPart;
    public readonly static string version_no = FileVersionInfo.GetVersionInfo(AOs_binary_path).ProductVersion;
    public readonly static ConsoleColor original_foreground_color = Console.ForegroundColor;
}
