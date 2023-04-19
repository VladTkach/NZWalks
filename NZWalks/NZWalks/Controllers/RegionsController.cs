using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NZWalks.CustomActionFilters;
using NZWalks.Data;
using NZWalks.Models.Domain;
using NZWalks.Models.DTO;
using NZWalks.Repositories;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace NZWalks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext _dbContext;
        private readonly IRegionRepository _regionRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<RegionsController> _logger;

        public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository, IMapper mapper,
            ILogger<RegionsController> logger)
        {
            _dbContext = dbContext;
            _regionRepository = regionRepository;
            _mapper = mapper;
            _logger = logger;
        }

        // GET ALL REGIONS
        [HttpGet]
        // [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                throw new Exception("This is custom exception");
                
                var regions = await _regionRepository.GetAllAsync();

                var regionsDto = _mapper.Map<List<RegionDto>>(regions);

                _logger.LogInformation(
                    $"Finish Get All Regions request with data: {JsonSerializer.Serialize(regions)}");
                return Ok(regionsDto);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw;
            }
        }

        // GET REGION BY ID
        [HttpGet]
        [Authorize(Roles = "Reader")]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var region = await _regionRepository.GetByIdAsync(id);
            if (region == null)
            {
                return NotFound();
            }

            var regionDto = _mapper.Map<RegionDto>(region);
            return Ok(regionDto);
        }

        //POST new Region
        [HttpPost]
        [Authorize(Roles = "Writer")]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            var regionDomModel = _mapper.Map<Region>(addRegionRequestDto);

            regionDomModel = await _regionRepository.CreatedAsync(regionDomModel);

            var regionDto = _mapper.Map<RegionDto>(regionDomModel);

            return CreatedAtAction(nameof(GetById), new { id = regionDomModel.Id }, regionDto);
        }

        //PUT (Update) Region by ID
        [HttpPut]
        [Authorize(Roles = "Writer")]
        [Route("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id,
            [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            var regionDom = _mapper.Map<Region>(updateRegionRequestDto);

            regionDom = await _regionRepository.UpdateAsync(id, regionDom);

            if (regionDom == null)
            {
                return NotFound();
            }

            var regionDto = _mapper.Map<RegionDto>(regionDom);

            return Ok(regionDto);
        }

        // DELETE Region by ID
        [HttpDelete]
        [Authorize(Roles = "Writer")]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var regionDom = await _regionRepository.DeleteAsync(id);

            if (regionDom == null)
            {
                return NotFound();
            }

            var regionDto = _mapper.Map<RegionDto>(regionDom);
            return Ok(regionDto);
        }
    }
}