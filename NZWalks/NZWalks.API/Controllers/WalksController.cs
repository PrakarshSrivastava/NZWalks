using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalksController : Controller
    {
        private readonly IWalkRepository walkRepository;
        private readonly IMapper mapper;

        public WalksController(IWalkRepository walkRepository, IMapper mapper)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWalksAsync()
        {
            // Fetch data from database - domain walks
            var walksDomain = await walkRepository.GetAllAsync();

            // Convert domain walks to DTO Walks
            var walksDTO = mapper.Map<List<Models.DTO.Walk>>(walksDomain);

            // Return response
            return Ok(walksDTO);
        }

        [HttpGet]
        [Route("{id:guid}")] //Only Guid values allowed
        [ActionName("GetWalkAsync")]
        public async Task<IActionResult> GetWalkAsync(Guid id)
        {
            var walk = await walkRepository.GetAsync(id);
            if (walk == null)
                return NotFound();
            var walkDTO = mapper.Map<Models.DTO.Walk>(walk);
            return Ok(walkDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddWalkAsync([FromBody] Models.DTO.AddWalkRequest addWalkRequest)
        {
            // Convert DTO to Domain Object
            var walkDomain = new Models.Domain.Walk()
            {
                Length = addWalkRequest.Length,
                Name = addWalkRequest.Name,
                RegionId = addWalkRequest.RegionId,
                WalkDifficultyId = addWalkRequest.WalkDifficultyId,
            };

            // Pass domain object to Repository to persist this
            var WalkDomain = await walkRepository.AddAsync(walkDomain);

            // Convert the Domain object back to DTO
            var walkDTO = mapper.Map<Models.DTO.Walk>(walkDomain);

            // Send DTO response back to Client
            return CreatedAtAction(nameof(GetWalkAsync), new { id = walkDTO.Id }, walkDTO);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkAsync([FromRoute] Guid id, [FromBody] Models.DTO.UpdateWalkRequest updateWalkRequest)
        {
            // Convert DTO to Domain object
            var walkDomain = new Models.Domain.Walk()
            {
                Length = updateWalkRequest.Length,
                Name = updateWalkRequest.Name,
                RegionId = updateWalkRequest.RegionId,
                WalkDifficultyId = updateWalkRequest.WalkDifficultyId
            };

            // Pass details to Repository - Get Domain object in response (or null)
            walkDomain = await walkRepository.UpdateAsync(id, walkDomain);
            // Handle Null (not found)
            if (walkDomain == null)
                return NotFound();
            // Convert back Domain to DTO
            var walkDTO = mapper.Map<Models.DTO.Walk>(walkDomain);
            // Return Response
            return Ok(walkDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalkAsync(Guid id)
        {
            var walkDomain = await walkRepository.DeleteAsync(id);
            if (walkDomain == null)
                return NotFound();
            //mapper.Map<Destination>(Source);
            var walkDTO = mapper.Map<Models.DTO.Walk>(walkDomain);
            return Ok(walkDTO);
        }
    }
}
