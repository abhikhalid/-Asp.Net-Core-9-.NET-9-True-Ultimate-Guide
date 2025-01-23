namespace StocksApp.ServiceContracts
{
    public interface IFinhubService
    {
        Task<Dictionary<string,object>?> GetStockPriceQuote(string stockSymbol);
    }
}
