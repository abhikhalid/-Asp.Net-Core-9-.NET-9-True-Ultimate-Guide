using System.Text.Json;
using StocksApp.ServiceContracts;

namespace StocksApp.Services
{
    public class FinhubService : IFinhubService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public FinhubService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol)
        {
            //it creates and creates object of HTTP client class automatically.
            using (HttpClient httpClient = _httpClientFactory.CreateClient())
            {
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
                {
                    //RequestUri = new Uri($"https://finnhub.io/api/v1/stock/candle?symbol=${stockSymbol}&resolution=1&from=1693493346&to=1693752546&token={_configuration["FinhubToken"]}"),
                    RequestUri = new Uri($"https://finnhub.io/api/v1/stock/candle?symbol=${stockSymbol}&resolution=1&from=1693493346&to=1693752546&token=ckc5md9r01qirs4v92ngckc5md9r01qirs4v92o0"),
                    Method = HttpMethod.Get,
                };

                HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

                Stream stream = httpResponseMessage.Content.ReadAsStream();

                StreamReader streamReader = new StreamReader(stream);

                string response = streamReader.ReadToEnd();

                //We can easily convert json information into dictionary object
                Dictionary<string, object>? responseDictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(response);

                if(responseDictionary == null)
                {
                    throw new InvalidOperationException("No response from finhub server");
                }

                if (responseDictionary.ContainsKey("error"))
                {
                    throw new InvalidOperationException(Convert.ToString(responseDictionary["error"]));
                }
                    

                return responseDictionary;
            }
        }
    }
}

