using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;
using NZWalks.API.CustomActionFilters;

namespace NZWalks.API.Controllers
{
     //https://localhost:1234/api/regions
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalkDbContext db;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController(NZWalkDbContext dbContext, IRegionRepository regionRepository,
            IMapper mapper)
        {
            db = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }

        // GET: https://localhost:portnumber/api/regions
        [HttpGet]
        public async Task<IActionResult> GetAllRegions()
        {
            // Get Data From Database - Domain models
            var regionsDomain = await regionRepository.GetAllAsync();

            // Map Domain model to DTO
            //var regionsDTO = new List<RegionDto>();

            //foreach (var regionDomain in regionsDomain)
            //{
            //    regionsDTO.Add(new RegionDto()
            //    {
            //        Id = regionDomain.Id,
            //        Code = regionDomain.Code,
            //        Name = regionDomain.Name,
            //        RegionImageUrl = regionDomain.RegionImageUrl
            //    }
            //    );
            //}

            //// return DTO to client
            //return Ok(regionsDTO);

            // Return DTOs
            return Ok(mapper.Map<List<RegionDto>>(regionsDomain));
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetRegionById([FromRoute] Guid id)
        {
            //var region = db.Regions.FirstOrDefault(x => x.Id == id);
            // Get Region Domain model from database
            var regionDomain = await regionRepository.GetRegionByIdAsync(id);

            if (regionDomain == null)
            {
                return NotFound();
            }

            // Map/Convert Domain model to DTO
            //var regionDTO = new RegionDto
            //{
            //    Id = regionDomain.Id,
            //    Code = regionDomain.Code,
            //    Name = regionDomain.Name,
            //    RegionImageUrl = regionDomain.RegionImageUrl
            //};

            // Return DTO back to client
            //return Ok(regionDTO);

            return Ok(mapper.Map<RegionDto>(regionDomain));
        }

        [HttpPost]
        [ValidateModelAttributes]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto regionRequestDto)
        {
            // Map or Convert DTO to Domain Model
            var regionDomainModel = mapper.Map<Region>(regionRequestDto);

            // Use Domain Model to create Region
            regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);

            // Map Domain model back to DTO
            var regionDto = mapper.Map<RegionDto>(regionDomainModel);

            return CreatedAtAction(nameof(GetRegionById), new { id = regionDto.Id }, regionDto);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModelAttributes]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            // Map DTO to Domain Model
            var regionDomainModel = mapper.Map<Region>(updateRegionRequestDto);

            // Check if region exists
            regionDomainModel = await regionRepository.UpdateAsync(id, regionDomainModel);

            if (regionDomainModel == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<RegionDto>(regionDomainModel));
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var regionDomainModel = await regionRepository.DeleteAsync(id);

            if (regionDomainModel == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<RegionDto>(regionDomainModel));
        }
    }
}
