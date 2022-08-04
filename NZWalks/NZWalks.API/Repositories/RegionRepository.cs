using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class RegionRepository: IRegionRepository
    { 
        // Talk with the database first
        private readonly NZWalksDbContext nZWalksDbContext;
        public RegionRepository(NZWalksDbContext nZWalksDbContext)
        {
            this.nZWalksDbContext = nZWalksDbContext;
        }

        // We Will Make this Function Async because It is in Interface
        public async Task<IEnumerable<Region>>GetAllAsync()
        {
            //return nZWalksDbContext.Regions.ToList();
            // Making it to Async
            return await nZWalksDbContext.Regions.ToListAsync();
        }
    }
}
