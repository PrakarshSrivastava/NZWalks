using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Repositories;
namespace NZWalks.API.Controllers
{
    [ApiController]
    //[Route("Regions")]  For Urlls This will the end points to call this controller
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
        public async Task<IActionResult> GetAllRegions()
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
    }
}
