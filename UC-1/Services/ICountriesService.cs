using Newtonsoft.Json.Linq;

namespace UC_1.Services
{
    public interface ICountriesService
    {
        Task<IList<JToken>> GetCountries();
    }
}
