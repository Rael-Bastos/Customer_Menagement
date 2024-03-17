namespace Domain.Ports.Settings
{
    public interface ITradeContextSettings
    {
        string DatabaseName { get; set; }
        string ConnectionString { get; set; }
    }
}
