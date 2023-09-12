partial class Features
{
    public void Restart()
    {
        sys_utils.CommandPrompt("shutdown /r /t0");
    }
}
