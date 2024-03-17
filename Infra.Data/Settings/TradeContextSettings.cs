using Domain.Ports.Settings;

namespace Infra.Data.Settings
{
    public  class TradeContextSettings : ITradeContextSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;
    }
}
