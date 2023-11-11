partial class ExperimentalFeatures
{
    private void LoadExperimentalFeatures()
    {
        this.parser.Add(
            new string[]{"itsmagic"}, "It's magic, it's magic.",
            is_flag: true, method: this.ItsMagic
        );

        this.parser.Add(
            new string[]{"switch"}, "Switch between applications using the App IDs",
            default_values: new string[]{""},
            max_args_length: 1,
            method: this.SwitchApp
        );

        this.parser.Add(
            new string[]{"studybyte"}, "Starts Studybyte",
            is_flag: true, method: this.StartStudybyte
        );

        this.parser.Add(
            new string[]{"cpix"}, "Starts Cpix",
            is_flag: true, method: this.StartCpix
        );
    }
}
