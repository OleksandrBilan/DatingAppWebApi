using AutoMapper;
using DatingApp.DTOs.Lookup;
using DatingApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.Controllers
{
    [Route("lookup")]
    public class LookupController : Controller
    {
        private readonly ILookupService _lookupService;
        private readonly IMapper _mapper;

        public LookupController(ILookupService lookupService, IMapper mapper)
        {
            _lookupService = lookupService;
            _mapper = mapper;
        }

        [HttpGet("getSex")]
        public async Task<IActionResult> GetSexAsync()
        {
            var sex = await _lookupService.GetSexAsync();
            var result = _mapper.Map<IEnumerable<SexDto>>(sex);
            return Ok(result);
        }

        [HttpGet("getCountries")]
        public async Task<IActionResult> GetCountriesAsync()
        {
            var countries = await _lookupService.GetCountriesAsync();
            var result = _mapper.Map<IEnumerable<CountryDto>>(countries);
            return Ok(result);
        }

        [HttpGet("getCities")]
        public async Task<IActionResult> GetCitiesAsync()
        {
            var cities = await _lookupService.GetCitiesAsync();
            var result = _mapper.Map<IEnumerable<CityDto>>(cities);
            return Ok(result);
        }

        [HttpGet("getSubscriptionTypes")]
        public async Task<IActionResult> GetSubscriptionTypesAsync()
        {
            var subscriptionTypes = await _lookupService.GetSubscriptionTypesAsync();
            var result = _mapper.Map<IEnumerable<SubscriptionTypeDto>>(subscriptionTypes);
            return Ok(result);
        }
    }
}
