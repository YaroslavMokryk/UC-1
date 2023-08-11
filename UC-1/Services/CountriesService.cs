using Newtonsoft.Json.Linq;

namespace UC_1.Services
{
    public class CountriesService : ICountriesService
    {
        public CountriesService() { }

        public async Task<IList<JToken>> GetCountries()
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync("https://restcountries.com/v3.1/all");
            if (response == null || !response.IsSuccessStatusCode)
            {
                throw new BadHttpRequestException($"Error when retrieving country data. Status: {response?.StatusCode} Reason: {response?.ReasonPhrase}");
            }
            var content = await response.Content.ReadAsStringAsync();
            return JArray.Parse(content).Values().ToList();
        }
    }
}
