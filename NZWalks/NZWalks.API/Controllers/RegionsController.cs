using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "reader")]
        public async Task<IActionResult> GetRegionAsync(Guid id)
        {
            var region = await regionRepository.GetAsync(id);//It May be null by FirstOrDefaultAsync
            if (region == null)
                return NotFound();
            var regionDTO = mapper.Map<Models.DTO.Region>(region);
            return Ok(regionDTO);
        }

        [HttpPost]
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> AddRegionAsync(Models.DTO.AddRegionRequest addRegionRequest)
        {
            // we will be First Validation what Client Sends
            // Validate The Request
                //ctrl + k + c - for comment in one 
                //ctrl + k + U - for uncomment in one
            /*//if (!ValidateAddRegionAsync(addRegionRequest))
            //{
            //    //Pass Model State To show error messages
            //    return BadRequest(ModelState);
            //}*/

             //We Will First convert the request(DTO) to Domain Model
             //We Will be using Conventional Method
            var region = new Models.Domain.Region()
            {
                Code = addRegionRequest.Code,
                Area = addRegionRequest.Area,
                Lat = addRegionRequest.Lat,
                Long = addRegionRequest.Long,
                Name = addRegionRequest.Name,
                Population = addRegionRequest.Population,

            };
            // Pass domain object to Repository to persist this
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
        [Authorize(Roles = "writer")]
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
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> UpdateRegionAsync([FromRoute]Guid id,[FromBody] Models.DTO.UpdateRegionRequest updateRegionRequest)
        {
            // Validate the incoming request
            /*if (!ValidateUpdateRegionAsync(updateRegionRequest))
            {
                return BadRequest(ModelState);
            }*/

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

        /*#region Private Methods
        private bool ValidateAddRegionAsync(Models.DTO.AddRegionRequest addRegionRequest)
        {
            if (addRegionRequest == null)
            {
                ModelState.AddModelError(nameof(addRegionRequest),
                    $"Add Region Data is required.");
                return false;
            }
            if (string.IsNullOrEmpty(addRegionRequest.Code))
            {
                // I want to tell the client of what went wrong in the request
                // I am using model state attribute
                // $ is used string interpolation
                ModelState.AddModelError(nameof(addRegionRequest.Code),
                    $"{nameof(addRegionRequest.Code)} cannot be null or empty or white space.");
            }
            if (string.IsNullOrWhiteSpace(addRegionRequest.Name))
            {
                ModelState.AddModelError(nameof(addRegionRequest.Name),
                    $"{nameof(addRegionRequest.Name)} cannot be null or empty or white space.");
            }
            if (addRegionRequest.Area <= 0)
            {
                ModelState.AddModelError(nameof(addRegionRequest.Area),
                    $"{nameof(addRegionRequest.Area)} cannot be less than or equal to zero.");
            }
            if (addRegionRequest.Lat <= 0)
            {
                ModelState.AddModelError(nameof(addRegionRequest.Lat),
                    $"{nameof(addRegionRequest.Lat)} cannot be less than or equal to zero.");
            }
            if (addRegionRequest.Long <= 0)
            {
                ModelState.AddModelError(nameof(addRegionRequest.Long),
                    $"{nameof(addRegionRequest.Long)} cannot be less than or equal to zero.");
            }
            if (addRegionRequest.Population < 0)
            {
                ModelState.AddModelError(nameof(addRegionRequest.Population),
                    $"{nameof(addRegionRequest.Population)} cannot be less than zero.");
            }
            // It check is any error occurred
            if (ModelState.ErrorCount > 0)
            {
                return false;
            }
            return true;
        }

        private bool ValidateUpdateRegionAsync(Models.DTO.UpdateRegionRequest updateRegionRequest)
        {
            if (updateRegionRequest == null)
            {
                ModelState.AddModelError(nameof(updateRegionRequest),
                    $"Add Region Data is required.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(updateRegionRequest.Code))
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Code),
                    $"{nameof(updateRegionRequest.Code)} cannot be null or empty or white space.");
            }

            if (string.IsNullOrWhiteSpace(updateRegionRequest.Name))
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Name),
                    $"{nameof(updateRegionRequest.Name)} cannot be null or empty or white space.");
            }

            if (updateRegionRequest.Area <= 0)
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Area),
                    $"{nameof(updateRegionRequest.Area)} cannot be less than or equal to zero.");
            }

            if (updateRegionRequest.Population < 0)
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Population),
                    $"{nameof(updateRegionRequest.Population)} cannot be less than zero.");
            }

            if (ModelState.ErrorCount > 0)
            {
                return false;
            }

            return true;
        }

        #endregion*/
    }
}
