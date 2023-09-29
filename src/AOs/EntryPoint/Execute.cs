partial class EntryPoint
{
    private void Execute()
    {
        while (true)
        {
            try
            {
                this.run_method(this.AOs, this.parser, AOs.TakeInput());
            }

            catch (Exception e)
            {
                CrashreportLog(e.ToString());
            }
        }
    }

    private void Execute(string input)
    {
        try
        {
            if (!Utils.String.IsEmpty(input))
                this.run_method(this.AOs, this.parser, AOs.TakeInput(input));
        }

        catch (Exception e)
        {
            CrashreportLog(e.ToString());
        }
    }

    private void Execute(string[] inputs)
    {
        foreach (string input in inputs)
        {
            try
            {
                if (!Utils.String.IsEmpty(input))
                    this.run_method(this.AOs, this.parser, AOs.TakeInput(input));
            }

            catch (Exception e)
            {
                CrashreportLog(e.ToString());
            }
        }
    }
}
