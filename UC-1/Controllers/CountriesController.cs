using Microsoft.AspNetCore.Mvc;
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

        [HttpGet]
        public async Task<IList<JToken>> Get()
        {
            var response = await _countriesService.GetCountries();
            return response;
        }
    }
}