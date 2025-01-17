namespace Services
{
    public class CitiesService
    {
        private List<string> _cities;

        public CitiesService()
        {
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
