using System.Diagnostics;
using System.Text;

class SystemUtils
{
    public Process process = new();

    public void StartApp(string app_name, string[] app_args=null, bool is_admin=false)
    {
        process.StartInfo.UseShellExecute = true;
        process.StartInfo.FileName = app_name;
        process.StartInfo.CreateNoWindow = false;

        if (app_args != null)
            process.StartInfo.Arguments = string.Join("", app_args);

        if (is_admin)
            process.StartInfo.Verb = "runas";

        try
        {
            process.Start();
        }

        catch (Exception e)
        {
            new Error($"Error: Cannot open the app.\n{e}");
        }
    }

    public void CommandPrompt(string cmd_args)
    {
        process.StartInfo.FileName = Obsidian.default_else_shell;
        process.StartInfo.UseShellExecute = false;

        process.StartInfo.Arguments = $"/C {cmd_args}";

        process.Start();
        process.WaitForExit();

        // process.StartInfo.FileName = Obsidian.default_else_shell;
        // process.StartInfo.UseShellExecute = false;
        // process.StartInfo.RedirectStandardOutput = true;
        // process.StartInfo.RedirectStandardError = true;
        // process.StartInfo.RedirectStandardInput = true;

        // process.StartInfo.Arguments = $"/C {cmd_args}";

        // StringBuilder output = new StringBuilder();

        // process.OutputDataReceived += (sender, e) =>
        // {
        //     if (e.Data != null)
        //     {
        //         output.AppendLine(e.Data);
        //         Console.WriteLine(e.Data);
        //     }
        // };

        // process.ErrorDataReceived += (sender, e) =>
        // {
        //     if (e.Data != null)
        //     {
        //         output.AppendLine(e.Data);
        //         Console.WriteLine(e.Data);
        //     }
        // };

        // process.Start();
        // process.BeginOutputReadLine();
        // process.BeginErrorReadLine();
        // process.WaitForExit();

        // return output.ToString().Trim();

        // process.StartInfo.UseShellExecute = false;
        // process.StartInfo.RedirectStandardOutput = true;
        // process.StartInfo.RedirectStandardError = true;
        // process.StartInfo.Arguments = $"/C {cmd_args}";
        // string output = "";

        // // Event handler for capturing the output
        // void output_handler(object sender, DataReceivedEventArgs e)
        // {
        //     if (!Utils.String.IsEmpty(e.Data))
        //     {
        //         output += e.Data + "\n";

        //         // Display the output in real-time
        //         Console.WriteLine(e.Data);
        //     }
        // }

        // // Redirect and handle standard output
        // process.OutputDataReceived += output_handler;

        // process.Start();

        // // Begin asynchronous reading of the standard output
        // process.BeginOutputReadLine();

        // // Wait for the process to exit
        // process.WaitForExit();

        // // Remove the event handler
        // process.OutputDataReceived -= output_handler;
        // return output.Trim();
    }
}
