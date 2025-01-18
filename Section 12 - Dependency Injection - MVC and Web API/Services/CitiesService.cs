using ServiceContracts;

namespace Services
{
    public class CitiesService : ICitiesService
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
        }

        public List<string> GetCities()
        {
            return _cities;
        }
    }
}
