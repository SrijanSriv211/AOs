partial class ExperimentalFeatures
{
    private void LoadExperimentalFeatures()
    {
        this.parser.Add(
            ["itsmagic"], "It's magic, it's magic.",
            is_flag: true, method: this.ItsMagic
        );

        this.parser.Add(
            ["switch"], "Switch between applications using the App IDs",
            default_values: [""],
            max_args_length: 1,
            method: this.SwitchApp
        );

        this.parser.Add(
            ["studybyte"], "Starts Studybyte",
            is_flag: true, method: this.StartStudybyte
        );

        this.parser.Add(
            ["cpix"], "Starts Cpix",
            is_flag: true, method: this.StartCpix
        );
    }
}
