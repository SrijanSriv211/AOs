partial class ExperimentalFeatures
{
    private void ItsMagic()
    {
        // Rickroll!!
        // https://youtu.be/dQw4w9WgXcQ

        sys_utils.StartApp("https://youtu.be/dQw4w9WgXcQ");
    }

    private void SwitchApp(string appid)
    {
        if (Utils.String.IsEmpty(appid))
        {
            System.Diagnostics.Process[] all_process = System.Diagnostics.Process.GetProcesses();
            foreach (System.Diagnostics.Process p in all_process)
            {
                if (!Utils.String.IsEmpty(p.MainWindowTitle))
                {
                    Console.Write("{0," + -Utils.Maths.CalculatePadding(100, 100) + "}", p.MainWindowTitle);
                    TerminalColor.Print($"Process ID: {p.Id}", ConsoleColor.DarkGray);
                }
            }
        }

        else
        {
            if (int.TryParse(appid, out int int_appid))
            {
                System.Diagnostics.Process process = System.Diagnostics.Process.GetProcessById(int_appid);
                WindowManager.SetForegroundWindow(process.MainWindowHandle);
            }

            else
            {
                new Error($"Invalid App ID: {appid}. App ID must be a number.");
                TerminalColor.Print("Please try '@switch' to get an App ID", ConsoleColor.White);
            }
        }
    }

    private void StartStudybyte()
    {
        sys_utils.StartApp("https://light-lens.github.io/Studybyte");
    }

    private void StartCpix()
    {
        sys_utils.StartApp("https://github.com/Light-Lens/Cpix");
    }
}
