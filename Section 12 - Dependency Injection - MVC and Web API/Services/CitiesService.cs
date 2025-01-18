using ServiceContracts;

namespace Services
{
    public class CitiesService : ICitiesService, IDisposable
    {
        private List<string> _cities;

        private Guid _serviceInstanceId;

        public Guid ServiceInstanceId { 
            
            get
            {
                return this._serviceInstanceId;
            }
        }

        public CitiesService()
        {
            _serviceInstanceId = Guid.NewGuid();

            _cities = new List<string>()
            {
                "New York",
                "London",
                "Paris",
                "Berlin",
                "Tokyo"
            };

            //TO DO: Add logic to open the db connection
        }

        public List<string> GetCities()
        {
            return _cities;
        }

        public void Dispose()
        {
            // TO DO: add logic top clode db connection
        }
    }
}
