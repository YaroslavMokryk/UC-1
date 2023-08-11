using Newtonsoft.Json.Linq;

namespace UC_1.Services
{
    public class CountriesService : ICountriesService
    {
        public CountriesService() { }

        public async Task<JArray> GetCountries(string? nameFilter, int? populationFilter, string? sortBy, int? limit)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync("https://restcountries.com/v3.1/all");
            if (response == null || !response.IsSuccessStatusCode)
            {
                throw new BadHttpRequestException($"Error when retrieving country data. Status: {response?.StatusCode} Reason: {response?.ReasonPhrase}");
            }
            var content = await response.Content.ReadAsStringAsync();
            var countries = JArray.Parse(content);

            if (nameFilter != null)
                countries = FilterByName(countries, nameFilter);

            if (populationFilter != null)
                countries = FilterByPopulation(countries, populationFilter.Value);

            if (sortBy != null)
            {
                if (sortBy.Equals("ascend", StringComparison.CurrentCultureIgnoreCase))
                    countries = SortByName(countries, true);
                else if (sortBy.Equals("descend", StringComparison.CurrentCultureIgnoreCase))
                    countries = SortByName(countries, false);
            }

            if (limit != null)
                countries = PaginateByLimit(countries, limit.Value);

            return countries;
        }

        private JArray FilterByName(JArray countries, string nameFilter)
        {
            return new JArray(countries.Where(c => c["name"]?["common"] != null && c["name"]["common"].ToString()
                .Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase)));
        }

        private JArray FilterByPopulation(JArray countries, int populationFilter)
        {
            return new JArray(countries.Where(c => c["population"] != null && (int)c["population"] / 1_000_000 <= populationFilter));
        }

        private JArray SortByName(JArray countries, bool ascending)
        {
            if (ascending) {
                return new JArray(countries.OrderBy(c => c["name"]["common"].ToString()));
            }
            return new JArray(countries.OrderByDescending(c => c["name"]["common"].ToString()));
        }

        private JArray PaginateByLimit(JArray countries, int limit)
        {
            return new JArray(countries.Take(limit));
        }
    }
}
