using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UC_1.Services;

namespace UC_1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CountriesController : ControllerBase
    {
        private ICountriesService _countriesService;

        public CountriesController(ICountriesService countriesService) {
            _countriesService = countriesService;
        }

        /// <summary>
        /// Retrieves a list of countries filtered by name and population, sorted by name and paginated with a custom limit
        /// </summary>
        /// <param name="nameFilter">Country name or part of name for filtering</param>
        /// <param name="populationFilter">Max country population for filtering</param>
        /// <param name="sortBy">Sorting method, "ascend" or "descend"</param>
        /// <returns>List of countries</returns>
        [HttpGet]
        public async Task<string> Get(string? nameFilter, int? populationFilter, string? sortBy)
        {
            var json = await _countriesService.GetCountries(nameFilter, populationFilter, sortBy);
            string response = JsonConvert.SerializeObject(json);
            return response;
        }
    }
}