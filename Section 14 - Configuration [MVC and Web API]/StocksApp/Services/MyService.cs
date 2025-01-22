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
                    RequestUri = new Uri("url"),
                    Method = HttpMethod.Get,
                };
 
                HttpResponseMessage httpResponseMessage =  await httpClient.SendAsync(httpRequestMessage);
            }
        }
    }
}
