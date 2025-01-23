using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StocksApp.Models;
using StocksApp.ServiceContracts;
using StocksApp.Services;

namespace StocksApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IFinhubService _finhubService;
        private readonly IOptions<TradingOptions> _tradingOptions;

        public HomeController(IFinhubService finhubService, IConfiguration configuration, IOptions<TradingOptions> tradingOptions)
        {
            _finhubService = finhubService;
            _tradingOptions = tradingOptions;
        }


        [Route("/")]
        public async  Task<IActionResult> Index()
        {
            if(_tradingOptions.Value.DefaultStockSymbol == null)
            {
                _tradingOptions.Value.DefaultStockSymbol = "MSFT";
            }

            Dictionary<string,object>? responseDictionary = await _finhubService.GetStockPriceQuote(_tradingOptions.Value.DefaultStockSymbol);

            Stocks stock = new Stocks()
            {
                StockSymbol = _tradingOptions.Value.DefaultStockSymbol,
                CurrentPrice = Convert.ToDouble(responseDictionary["c"].ToString()),
                HighestPrice = Convert.ToDouble(responseDictionary["h"].ToString()),
                LowestPrice = Convert.ToDouble(responseDictionary["l"].ToString()),
                OpenPrice = Convert.ToDouble(responseDictionary["o"].ToString()),
            };

            return View(stock);
        }
    }
}
