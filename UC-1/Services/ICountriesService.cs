using Newtonsoft.Json.Linq;

namespace UC_1.Services
{
    public interface ICountriesService
    {
        Task<JArray> GetCountries(string? nameFilter, int? populationFilter, string? sortBy);
    }
}
