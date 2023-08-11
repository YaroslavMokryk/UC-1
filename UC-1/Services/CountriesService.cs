using Newtonsoft.Json.Linq;

namespace UC_1.Services
{
    public class CountriesService : ICountriesService
    {
        public CountriesService() { }

        public async Task<JArray> GetCountries(string? nameFilter, int? populationFilter)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync("https://restcountries.com/v3.1/all");
            if (response == null || !response.IsSuccessStatusCode)
            {
                throw new BadHttpRequestException($"Error when retrieving country data. Status: {response?.StatusCode} Reason: {response?.ReasonPhrase}");
            }
            var content = await response.Content.ReadAsStringAsync();
            var countries = JArray.Parse(content);

            return countries;
        }

        private JArray FilterByName(JArray countries, string nameFilter)
        {
            return JArray.FromObject(countries.Where(c => c["name"]?["common"] != null && c["name"]["common"].ToString()
                .Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase)).Select(o => (JObject)o).ToList());
        }

        private JArray FilterByPopulation(JArray countries, int populationFilter)
        {
            return JArray.FromObject(countries.Where(c => c["population"] != null && (int)c["population"] / 1_000_000 <= 10)
                .Select(o => (JObject)o).ToList());
        }
    }
}
