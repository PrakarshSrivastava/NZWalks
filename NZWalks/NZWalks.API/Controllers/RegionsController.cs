using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Repositories;
namespace NZWalks.API.Controllers
{
    [ApiController]
    //[Route("Regions")]  For Urls This will the end points to call this controller
    [Route("[controller]")] // this is another way wahich will autopopulate
    public class RegionsController : Controller
    {
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        //We Will initialize the interface in the Constructor
        // Which will give the implementation of RegionRepository with the help of Service
        // Level of abstraction using Repository Method
        public RegionsController(IRegionRepository regionRepository, IMapper mapper)
        {
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllRegionsAsync()
        {
            //Static List
            /* var regions = new List<Region>()
             {
                 new Region
                 {
                     Id = Guid.NewGuid(),
                     Name = "Wellington",
                     Code = "WLG",
                     Area = 227755,
                     Lat =  -1.8822,
                     Long = 299.88,
                     Population = 55000
                 },
                 new Region
                 {
                     Id = Guid.NewGuid(),
                     Name = "Auckland",
                     Code = "AUCK",
                     Area = 227755,
                     Lat =  -1.8822,
                     Long = 299.88,
                     Population = 55000
                 }
             };*/
            // See Without going implementation class we access GetALL method
            // Magic of Repository Method
            //var regions=regionRepository.GetAll();
            //Return DTO Regions to adstract data model
            // Only DTO model will exposed to outside world
            // return DTO regions
            /*var regionsDTO = new List<Models.DTO.Region>();
            regions.ToList().ForEach(region =>
            {
                var regionDTO = new Models.DTO.Region()
                {
                    Id = region.Id,
                    Code = region.Code,
                    Name = region.Name,
                    Area = region.Area,
                    Lat = region.Lat,
                    Long = region.Long,
                    Population = region.Population
                };

                regionsDTO.Add(regionDTO);
            });*/
            // To make it Async we will be using await
            // and because of this await we will Async before the function name
            var regions = await regionRepository.GetAllAsync();
            var regionsDTO=mapper.Map<List<Models.DTO.Region>>(regions);
            return Ok(regionsDTO);
        }

        [HttpGet]
        [Route("{id:guid}")] //Only Guid values allowed
        [ActionName("GetRegionAsync")]
        public async Task<IActionResult> GetRegionAsync(Guid id)
        {
            var region = await regionRepository.GetAsync(id);//It May be null by FirstOrDefaultAsync
            if (region == null)
                return NotFound();
            var regionDTO = mapper.Map<Models.DTO.Region>(region);
            return Ok(regionDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddRegionAsync(Models.DTO.AddRegionRequest addRegionRequest)
        {
            // We Will First convert the request(DTO) to Domain Model
            // We Will be using Conventional Method
            var region = new Models.Domain.Region()
            {
                Code = addRegionRequest.Code,
                Area = addRegionRequest.Area,
                Lat = addRegionRequest.Lat,
                Long = addRegionRequest.Long,
                Name = addRegionRequest.Name,
                Population = addRegionRequest.Population,

            };
            // Then We will pass the details to  Repository
            region = await regionRepository.AddAsync(region);
            // Convert Back To DTO
            // because we don't want to pass data directly coming from the domain model
            // we pass the data in the form of DTO model

            // We Are using Conventional Method here and not DTOs
            var regionDTO = new Models.DTO.Region()
            {
                Id = region.Id,
                Code = region.Code,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Name = region.Name,
                Population = region.Population,
            };
            return CreatedAtAction(nameof(GetRegionAsync), new {id= regionDTO.Id}, regionDTO);//[ActionName("GetRegionAsync")] as this will shows up and passing Id as a parameter
            // regionDTO is used because we want to send whole data back
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteRegionAsync(Guid id)
        {
            var region = await regionRepository.DeleteAsync(id);
            if (region == null)
                return NotFound();
            var regionDTO = mapper.Map<Models.DTO.Region>(region);
            return Ok(regionDTO);
        }

        [HttpPut]
        [Route("{id:guid}")]//means that its clear that id is coming from route url
        public async Task<IActionResult> UpdateRegionAsync([FromRoute]Guid id,[FromBody] Models.DTO.UpdateRegionRequest updateRegionRequest)
        {

            // Convert DTO to Domain model
            var region = new Models.Domain.Region()
            {
                Code = updateRegionRequest.Code,
                Area = updateRegionRequest.Area,
                Lat = updateRegionRequest.Lat,
                Long = updateRegionRequest.Long,
                Name = updateRegionRequest.Name,
                Population = updateRegionRequest.Population
            };

            // Update Region using repository
            region = await regionRepository.UpdateAsync(id, region);

            // If Null then NotFound
            if (region == null)
            {
                return NotFound();
            }

            // Convert Domain back to DTO
            var regionDTO = mapper.Map<Models.DTO.Region>(region);

            // Return Ok response
            return Ok(regionDTO);

        }
    }
}
