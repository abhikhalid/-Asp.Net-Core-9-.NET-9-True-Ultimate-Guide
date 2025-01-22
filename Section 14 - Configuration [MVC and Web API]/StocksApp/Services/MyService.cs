namespace StocksApp.Services
{
    public class MyService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public MyService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task method()
        {
            //it creates and creates object of HTTP client class automatically.
            using(HttpClient httpClient =  _httpClientFactory.CreateClient())
            {
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
                {
                    RequestUri = new Uri("https://finnhub.io/api/v1/stock/candle?symbol=AAPL&resolution=1&from=1693493346&to=1693752546&token=ckc5md9r01qirs4v92ngckc5md9r01qirs4v92o0"),
                    Method = HttpMethod.Get,
                };
 
                HttpResponseMessage httpResponseMessage =  await httpClient.SendAsync(httpRequestMessage);

                Stream stream = httpResponseMessage.Content.ReadAsStream();

                StreamReader streamReader = new StreamReader(stream);

                string response = streamReader.ReadToEnd();
            }
        }
    }
}
