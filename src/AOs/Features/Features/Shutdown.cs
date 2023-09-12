partial class Features
{
    public void Shutdown()
    {
        sys_utils.CommandPrompt("shutdown /s /t0");
    }
}
