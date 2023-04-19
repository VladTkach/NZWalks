using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.CustomActionFilters;
using NZWalks.Models.Domain;
using NZWalks.Models.DTO;
using NZWalks.Repositories;

namespace NZWalks.Controllers
{
    //api/walks
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IWalkRepository _walkRepository;

        public WalksController(IMapper mapper, IWalkRepository walkRepository)
        {
            _mapper = mapper;
            _walkRepository = walkRepository;
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto)
        {

                var walkDom = _mapper.Map<Walk>(addWalkRequestDto);
                await _walkRepository.CreateAsync(walkDom);

                var walkDto = _mapper.Map<WalkDto>(walkDom);
                return Ok(walkDto);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery, 
            [FromQuery] string? sortBy, [FromQuery] bool? isAscending, [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 1000)
        {
            var walksDom = await _walkRepository.GetAllAsync(filterOn, filterQuery, 
                sortBy, isAscending ?? true, pageNumber, pageSize);

            var walksDto = _mapper.Map<List<WalkDto>>(walksDom);

            return Ok(walksDto);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var walkDom = await _walkRepository.GetByIdAsync(id);

            if (walkDom == null)
            {
                return NotFound();
            }

            var walkDto = _mapper.Map<WalkDto>(walkDom);

            return Ok(walkDto);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, UpdateWalkRequestDto updateWalkRequestDto)
        {
            var walkDom = _mapper.Map<Walk>(updateWalkRequestDto);

                walkDom = await _walkRepository.UpdateAsync(id, walkDom);

                if (walkDom == null)
                {
                    return NotFound();
                }

                var walkDto = _mapper.Map<WalkDto>(walkDom);

                return Ok(walkDto);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var walkDom = await _walkRepository.DeleteAsync(id);
            if (walkDom == null)
            {
                return NotFound();
            }

            var walkDto = _mapper.Map<WalkDto>(walkDom);
            return Ok(walkDto);
        }
    }
}