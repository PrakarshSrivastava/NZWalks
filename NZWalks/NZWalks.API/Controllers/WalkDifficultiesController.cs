using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalkDifficultiesController : Controller
    {
        private readonly IWalkDifficultyRepository walkDifficultyRepository;
        private readonly IMapper mapper;

        public WalkDifficultiesController(IWalkDifficultyRepository walkDifficultyRepository, IMapper mapper)
        {
            this.walkDifficultyRepository = walkDifficultyRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWalkDifficulties()
        {
            var walkDifficultiesDomain = await walkDifficultyRepository.GetAllAsync();

            var walkDifficultiesDTO = mapper.Map<List<Models.DTO.WalkDifficulty>>(walkDifficultiesDomain);
            return Ok(walkDifficultiesDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkDifficultyAsync")]
        public async Task<IActionResult> GetWalkDifficultyAsync([FromRoute]Guid id)
        {
            var walkDifficultiesDomain = await walkDifficultyRepository.GetAsync(id);
            if (walkDifficultiesDomain == null)
                return NotFound();
            var walkDifficultiesDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficultiesDomain);
            return Ok(walkDifficultiesDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddWalkDifficultyAsync([FromBody] Models.DTO.AddWalkDifficultyRequest addWalkDifficultyRequest)
        {
            var walkDifficultiesDomain = new Models.Domain.WalkDifficulty()
            {
                Code = addWalkDifficultyRequest.Code,
            };
            walkDifficultiesDomain = await walkDifficultyRepository.AddAsync(walkDifficultiesDomain);
            var walkDifficultiesDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficultiesDomain);
            return CreatedAtAction(nameof(GetWalkDifficultyAsync), new { id = walkDifficultiesDTO.Id }, walkDifficultiesDTO);

        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkDifficultyAsync([FromRoute]Guid id,[FromBody] Models.DTO.UpdateWalkDifficultyRequest updateWalkDifficultyRequest)
        {
            var updateWalkDifficultyDomain = new Models.Domain.WalkDifficulty()
            {
                Code = updateWalkDifficultyRequest.Code,
            };

            updateWalkDifficultyDomain = await walkDifficultyRepository.UpdateAsync(id, updateWalkDifficultyDomain);

            if (updateWalkDifficultyDomain == null)
                return NotFound();

            var updateWalkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(updateWalkDifficultyDomain);
            return Ok(updateWalkDifficultyDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeteleWalkDifficultyAsync([FromRoute] Guid id)
        {
            var walkDifficultyDomain = await walkDifficultyRepository.DeleteAsync(id);
            if (walkDifficultyDomain == null)
                return NotFound();
            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficultyDomain);
            return Ok(walkDifficultyDTO);

        }
    }
}
