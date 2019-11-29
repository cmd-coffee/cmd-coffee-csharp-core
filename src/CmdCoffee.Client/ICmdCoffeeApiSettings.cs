namespace CmdCoffee.Client
{
    public interface ICmdCoffeeApiSettings
    {
        string CmdCoffeeApiAddress { get;}
        string AccessKey { get; }
        string SettingsFile { get; }
    }
}