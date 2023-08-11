using UC_1.Services;
using Xunit;
using System.Net;
using NSubstitute;

namespace UC_1.Tests
{
    public class CountriesServiceTests
    {
        private readonly ICountriesService _countriesService;

        public CountriesServiceTests()
        {
            var mockFactory = Substitute.For<IHttpClientFactory>();
            var mockMessageHandler = new MockHttpMessageHandler();
            var httpClient = new HttpClient(mockMessageHandler);
            mockFactory.CreateClient().Returns(httpClient);
            _countriesService = new CountriesService(mockFactory);
        }

        [Fact]
        public async void GetCountries_NoParams_ReturnsAllCountries()
        {
            //Act
            var countries = await _countriesService.GetCountries(null, null, null, null);

            //Assert
            Assert.NotEmpty(countries);
            Assert.Equal(3, countries.Count);
            Assert.Equal("Saudi Arabia", countries[0]["name"]["common"]);
            Assert.Equal("Cameroon", countries[1]["name"]["common"]);
            Assert.Equal("Iran", countries[2]["name"]["common"]);
        }

        [Theory]
        [InlineData("oo")]
        [InlineData("ra")]
        public async void GetCountries_FilterByName_FiltersByName(string nameFilter)
        {
            //Act
            var countries = await _countriesService.GetCountries(nameFilter, null, null, null);

            //Assert
            Assert.NotEmpty(countries);
            Assert.All(countries, c => c["name"]["common"].ToString().Contains(nameFilter));
        }

        [Theory]
        [InlineData(40)]
        [InlineData(30)]
        public async void GetCountries_FilterByPopulation_FiltersByPopulation(int populationFilter)
        {
            //Act
            var countries = await _countriesService.GetCountries(null, populationFilter, null, null);

            //Assert
            Assert.NotEmpty(countries);
            foreach (var country in countries)
            {
                Assert.True((int)country["population"] / 1_000_000 <= populationFilter);
            }
        }

        [Fact]
        public async void GetCountries_SortByAscend_SortsByNameAscending()
        {
            //Act
            var countries = await _countriesService.GetCountries(null, null, "ascend", null);

            //Assert
            Assert.NotEmpty(countries);
            Assert.Equal(3, countries.Count);
            Assert.Equal("Cameroon", countries[0]["name"]["common"]);
            Assert.Equal("Iran", countries[1]["name"]["common"]);
            Assert.Equal("Saudi Arabia", countries[2]["name"]["common"]);
        }

        [Fact]
        public async void GetCountries_SortByDescend_SortsByNameDescending()
        {
            //Act
            var countries = await _countriesService.GetCountries(null, null, "descend", null);

            //Assert
            Assert.NotEmpty(countries);
            Assert.Equal(3, countries.Count);
            Assert.Equal("Saudi Arabia", countries[0]["name"]["common"]);
            Assert.Equal("Iran", countries[1]["name"]["common"]);
            Assert.Equal("Cameroon", countries[2]["name"]["common"]);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async void GetCountries_Limit_PaginatesByLimit(int limit)
        {
            //Act
            var countries = await _countriesService.GetCountries(null, null, null, limit);

            //Assert
            Assert.NotEmpty(countries);
            Assert.Equal(limit, countries.Count);
        }
    }

    public class MockHttpMessageHandler : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(Send(request, cancellationToken));
        }

        protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(
                    "[{\"name\":{\"common\":\"Saudi Arabia\",\"official\":\"Kingdom of Saudi Arabia\",\"nativeName\":{\"ara\":{\"official\":\"\",\"common\":\"\"}}},\"tld\":[\".sa\",\".\"],\"cca2\":\"SA\",\"ccn3\":\"682\",\"cca3\":\"SAU\",\"cioc\":\"KSA\",\"independent\":true,\"status\":\"officially-assigned\",\"unMember\":true,\"currencies\":{\"SAR\":{\"name\":\"Saudi riyal\",\"symbol\":\".\"}},\"idd\":{\"root\":\"+9\",\"suffixes\":[\"66\"]},\"capital\":[\"Riyadh\"],\"altSpellings\":[\"Saudi\",\"SA\",\"Kingdom of Saudi Arabia\",\"Al-Mamlakah al-Arabiyyah as-Sudiyyah\"],\"region\":\"Asia\",\"subregion\":\"Western Asia\",\"languages\":{\"ara\":\"Arabic\"},\"translations\":{\"ara\":{\"official\":\"\",\"common\":\"\"},\"bre\":{\"official\":\"Rouantelezh Arabia Saoudat\",\"common\":\"Arabia Saoudat\"},\"ces\":{\"official\":\"Sadskoarabsk krlovstv\",\"common\":\"Sadsk Arbie\"},\"cym\":{\"official\":\"Kingdom of Saudi Arabia\",\"common\":\"Saudi Arabia\"},\"deu\":{\"official\":\"Knigreich Saudi-Arabien\",\"common\":\"Saudi-Arabien\"},\"est\":{\"official\":\"Saudi Araabia Kuningriik\",\"common\":\"Saudi Araabia\"},\"fin\":{\"official\":\"Saudi-Arabian kuningaskunta\",\"common\":\"Saudi-Arabia\"},\"fra\":{\"official\":\"Royaume d'Arabie Saoudite\",\"common\":\"Arabie Saoudite\"},\"hrv\":{\"official\":\"Kraljevina Saudijska Arabija\",\"common\":\"Saudijska Arabija\"},\"hun\":{\"official\":\"Szad-Arbia\",\"common\":\"Szad-Arbia\"},\"ita\":{\"official\":\"Arabia Saudita\",\"common\":\"Arabia Saudita\"},\"jpn\":{\"official\":\"\",\"common\":\"\"},\"kor\":{\"official\":\" \",\"common\":\"\"},\"nld\":{\"official\":\"Koninkrijk van Saoedi-Arabi\",\"common\":\"Saoedi-Arabi\"},\"per\":{\"official\":\"  \",\"common\":\" \"},\"pol\":{\"official\":\"Krlestwo Arabii Saudyjskiej\",\"common\":\"Arabia Saudyjska\"},\"por\":{\"official\":\"Reino da Arbia Saudita\",\"common\":\"Arbia Saudita\"},\"rus\":{\"official\":\"  \",\"common\":\" \"},\"slk\":{\"official\":\"Saudskoarabsk krovstvo\",\"common\":\"Saudsk Arbia\"},\"spa\":{\"official\":\"Reino de Arabia Saudita\",\"common\":\"Arabia Saud\"},\"srp\":{\"official\":\"  \",\"common\":\" \"},\"swe\":{\"official\":\"Kungadmet Saudiarabien\",\"common\":\"Saudiarabien\"},\"tur\":{\"official\":\"Suudi Arabistan Krall\",\"common\":\"Suudi Arabistan\"},\"urd\":{\"official\":\"  \",\"common\":\" \"},\"zho\":{\"official\":\"\",\"common\":\"\"}},\"latlng\":[25.0,45.0],\"landlocked\":false,\"borders\":[\"IRQ\",\"JOR\",\"KWT\",\"OMN\",\"QAT\",\"ARE\",\"YEM\"],\"area\":2149690.0,\"demonyms\":{\"eng\":{\"f\":\"Saudi Arabian\",\"m\":\"Saudi Arabian\"},\"fra\":{\"f\":\"Saoudienne\",\"m\":\"Saoudien\"}},\"flag\":\"\\uD83C\\uDDF8\\uD83C\\uDDE6\",\"maps\":{\"googleMaps\":\"https://goo.gl/maps/5PSjvdJ1AyaLFRrG9\",\"openStreetMaps\":\"https://www.openstreetmap.org/relation/307584\"},\"population\":34813867,\"fifa\":\"KSA\",\"car\":{\"signs\":[\"SA\"],\"side\":\"right\"},\"timezones\":[\"UTC+03:00\"],\"continents\":[\"Asia\"],\"flags\":{\"png\":\"https://flagcdn.com/w320/sa.png\",\"svg\":\"https://flagcdn.com/sa.svg\",\"alt\":\"The flag of Saudi Arabia has a green field, at the center of which is an Arabic inscription  the Shahada  in white above a white horizontal sabre with its tip pointed to the hoist side of the field.\"},\"coatOfArms\":{\"png\":\"https://mainfacts.com/media/images/coats_of_arms/sa.png\",\"svg\":\"https://mainfacts.com/media/images/coats_of_arms/sa.svg\"},\"startOfWeek\":\"sunday\",\"capitalInfo\":{\"latlng\":[24.65,46.7]},\"postalCode\":{\"format\":\"#####\",\"regex\":\"^(\\\\d{5})$\"}},{\"name\":{\"common\":\"Cameroon\",\"official\":\"Republic of Cameroon\",\"nativeName\":{\"eng\":{\"official\":\"Republic of Cameroon\",\"common\":\"Cameroon\"},\"fra\":{\"official\":\"Rpublique du Cameroun\",\"common\":\"Cameroun\"}}},\"tld\":[\".cm\"],\"cca2\":\"CM\",\"ccn3\":\"120\",\"cca3\":\"CMR\",\"cioc\":\"CMR\",\"independent\":true,\"status\":\"officially-assigned\",\"unMember\":true,\"currencies\":{\"XAF\":{\"name\":\"Central African CFA franc\",\"symbol\":\"Fr\"}},\"idd\":{\"root\":\"+2\",\"suffixes\":[\"37\"]},\"capital\":[\"Yaound\"],\"altSpellings\":[\"CM\",\"Republic of Cameroon\",\"Rpublique du Cameroun\"],\"region\":\"Africa\",\"subregion\":\"Middle Africa\",\"languages\":{\"eng\":\"English\",\"fra\":\"French\"},\"translations\":{\"ara\":{\"official\":\" \",\"common\":\"\"},\"bre\":{\"official\":\"Republik Kameroun\",\"common\":\"Kameroun\"},\"ces\":{\"official\":\"Kamerunsk republika\",\"common\":\"Kamerun\"},\"cym\":{\"official\":\"Gweriniaeth Camern\",\"common\":\"Camern\"},\"deu\":{\"official\":\"Republik Kamerun\",\"common\":\"Kamerun\"},\"est\":{\"official\":\"Kameruni Vabariik\",\"common\":\"Kamerun\"},\"fin\":{\"official\":\"Kamerunin tasavalta\",\"common\":\"Kamerun\"},\"fra\":{\"official\":\"Rpublique du Cameroun\",\"common\":\"Cameroun\"},\"hrv\":{\"official\":\"Republika Kamerun\",\"common\":\"Kamerun\"},\"hun\":{\"official\":\"Kameruni Kztrsasg\",\"common\":\"Kamerun\"},\"ita\":{\"official\":\"Repubblica del Camerun\",\"common\":\"Camerun\"},\"jpn\":{\"official\":\"\",\"common\":\"\"},\"kor\":{\"official\":\" \",\"common\":\"\"},\"nld\":{\"official\":\"Republiek Kameroen\",\"common\":\"Kameroen\"},\"per\":{\"official\":\" \",\"common\":\"\"},\"pol\":{\"official\":\"Republika Kamerunu\",\"common\":\"Kamerun\"},\"por\":{\"official\":\"Repblica dos Camares\",\"common\":\"Camares\"},\"rus\":{\"official\":\" \",\"common\":\"\"},\"slk\":{\"official\":\"Kamerunsk republika\",\"common\":\"Kamerun\"},\"spa\":{\"official\":\"Repblica de Camern\",\"common\":\"Camern\"},\"srp\":{\"official\":\" \",\"common\":\"\"},\"swe\":{\"official\":\"Republiken Kamerun\",\"common\":\"Kamerun\"},\"tur\":{\"official\":\"Kamerun Cumhuriyeti\",\"common\":\"Kamerun\"},\"urd\":{\"official\":\" \",\"common\":\"\"},\"zho\":{\"official\":\"\",\"common\":\"\"}},\"latlng\":[6.0,12.0],\"landlocked\":false,\"borders\":[\"CAF\",\"TCD\",\"COG\",\"GNQ\",\"GAB\",\"NGA\"],\"area\":475442.0,\"demonyms\":{\"eng\":{\"f\":\"Cameroonian\",\"m\":\"Cameroonian\"},\"fra\":{\"f\":\"Camerounaise\",\"m\":\"Camerounais\"}},\"flag\":\"\\uD83C\\uDDE8\\uD83C\\uDDF2\",\"maps\":{\"googleMaps\":\"https://goo.gl/maps/JqiipHgFboG3rBJh9\",\"openStreetMaps\":\"https://www.openstreetmap.org/relation/192830\"},\"population\":26545864,\"gini\":{\"2014\":46.6},\"fifa\":\"CMR\",\"car\":{\"signs\":[\"CAM\"],\"side\":\"right\"},\"timezones\":[\"UTC+01:00\"],\"continents\":[\"Africa\"],\"flags\":{\"png\":\"https://flagcdn.com/w320/cm.png\",\"svg\":\"https://flagcdn.com/cm.svg\",\"alt\":\"The flag of Cameroon is composed of three equal vertical bands of green, red and yellow, with a yellow five-pointed star in the center.\"},\"coatOfArms\":{\"png\":\"https://mainfacts.com/media/images/coats_of_arms/cm.png\",\"svg\":\"https://mainfacts.com/media/images/coats_of_arms/cm.svg\"},\"startOfWeek\":\"monday\",\"capitalInfo\":{\"latlng\":[3.85,11.5]}},{\"name\":{\"common\":\"Iran\",\"official\":\"Islamic Republic of Iran\",\"nativeName\":{\"fas\":{\"official\":\"  \",\"common\":\"\"}}},\"tld\":[\".ir\",\".\"],\"cca2\":\"IR\",\"ccn3\":\"364\",\"cca3\":\"IRN\",\"cioc\":\"IRI\",\"independent\":true,\"status\":\"officially-assigned\",\"unMember\":true,\"currencies\":{\"IRR\":{\"name\":\"Iranian rial\",\"symbol\":\"\"}},\"idd\":{\"root\":\"+9\",\"suffixes\":[\"8\"]},\"capital\":[\"Tehran\"],\"altSpellings\":[\"IR\",\"Islamic Republic of Iran\",\"Iran, Islamic Republic of\",\"Jomhuri-ye Eslmi-ye Irn\"],\"region\":\"Asia\",\"subregion\":\"Southern Asia\",\"languages\":{\"fas\":\"Persian (Farsi)\"},\"translations\":{\"ara\":{\"official\":\"  \",\"common\":\"\"},\"bre\":{\"official\":\"Republik Islamek Iran\",\"common\":\"Iran\"},\"ces\":{\"official\":\"Islmsk republika rn\",\"common\":\"rn\"},\"cym\":{\"official\":\"Islamic Republic of Iran\",\"common\":\"Iran\"},\"deu\":{\"official\":\"Islamische Republik Iran\",\"common\":\"Iran\"},\"est\":{\"official\":\"Iraani Islamivabariik\",\"common\":\"Iraan\"},\"fin\":{\"official\":\"Iranin islamilainen tasavalta\",\"common\":\"Iran\"},\"fra\":{\"official\":\"Rpublique islamique d'Iran\",\"common\":\"Iran\"},\"hrv\":{\"official\":\"Islamska Republika Iran\",\"common\":\"Iran\"},\"hun\":{\"official\":\"Irni Iszlm Kztrsasg\",\"common\":\"Irn\"},\"ita\":{\"official\":\"Repubblica islamica dell'Iran\",\"common\":\"Iran\"},\"jpn\":{\"official\":\"\",\"common\":\"\"},\"kor\":{\"official\":\"\",\"common\":\"\"},\"nld\":{\"official\":\"Islamitische Republiek Iran\",\"common\":\"Iran\"},\"pol\":{\"official\":\"Islamska Republika Iranu\",\"common\":\"Iran\"},\"por\":{\"official\":\"Repblica Islmica do Ir\",\"common\":\"Iro\"},\"rus\":{\"official\":\"  \",\"common\":\"\"},\"slk\":{\"official\":\"Irnska islamsk republika\",\"common\":\"Irn\"},\"spa\":{\"official\":\"Repblica Islmica de Irn\",\"common\":\"Iran\"},\"srp\":{\"official\":\"  \",\"common\":\"\"},\"swe\":{\"official\":\"Islamiska republiken Iran\",\"common\":\"Iran\"},\"tur\":{\"official\":\"ran slam Cumhuriyeti\",\"common\":\"ran\"},\"urd\":{\"official\":\" \",\"common\":\"\"},\"zho\":{\"official\":\"\",\"common\":\"\"}},\"latlng\":[32.0,53.0],\"landlocked\":false,\"borders\":[\"AFG\",\"ARM\",\"AZE\",\"IRQ\",\"PAK\",\"TUR\",\"TKM\"],\"area\":1648195.0,\"demonyms\":{\"eng\":{\"f\":\"Iranian\",\"m\":\"Iranian\"},\"fra\":{\"f\":\"Iranienne\",\"m\":\"Iranien\"}},\"flag\":\"\\uD83C\\uDDEE\\uD83C\\uDDF7\",\"maps\":{\"googleMaps\":\"https://goo.gl/maps/dMgEGuacBPGYQnjY7\",\"openStreetMaps\":\"https://www.openstreetmap.org/relation/304938\"},\"population\":83992953,\"gini\":{\"2018\":42.0},\"fifa\":\"IRN\",\"car\":{\"signs\":[\"IR\"],\"side\":\"right\"},\"timezones\":[\"UTC+03:30\"],\"continents\":[\"Asia\"],\"flags\":{\"png\":\"https://flagcdn.com/w320/ir.png\",\"svg\":\"https://flagcdn.com/ir.svg\",\"alt\":\"The flag of Iran is composed of three equal horizontal bands of green, white and red. A red emblem of Iran is centered in the white band and Arabic inscriptions in white span the bottom edge of the green band and the top edge of the red band.\"},\"coatOfArms\":{\"png\":\"https://mainfacts.com/media/images/coats_of_arms/ir.png\",\"svg\":\"https://mainfacts.com/media/images/coats_of_arms/ir.svg\"},\"startOfWeek\":\"saturday\",\"capitalInfo\":{\"latlng\":[35.7,51.42]},\"postalCode\":{\"format\":\"##########\",\"regex\":\"^(\\\\d{10})$\"}}]"
                )
            };
        }
    }
}