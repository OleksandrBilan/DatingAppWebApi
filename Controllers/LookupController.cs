using AutoMapper;
using DatingApp.DTOs;
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
    }
}
