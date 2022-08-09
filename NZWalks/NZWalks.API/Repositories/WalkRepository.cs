using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class WalkRepository : IWalkRepository
    {
        private readonly NZWalksDbContext nZWalksDbContext;

        public WalkRepository(NZWalksDbContext nZWalksDbContext)
        {
            this.nZWalksDbContext = nZWalksDbContext;
        }

        public async Task<Walk> AddAsync(Walk walk)
        {
            // Assigning New Id because we are not taking it from client
            walk.Id = Guid.NewGuid();
            await nZWalksDbContext.Walks.AddAsync(walk);
            await nZWalksDbContext.SaveChangesAsync();
            return walk;
        }

        public async Task<Walk> DeleteAsync(Guid id)
        {
            var walk = await nZWalksDbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if(walk == null)
                return null;
            nZWalksDbContext.Walks.Remove(walk);
            await nZWalksDbContext.SaveChangesAsync();
            return walk;
        }

        public async Task<IEnumerable<Walk>> GetAllAsync()
        {
            //return await nZWalksDbContext.Walks.ToListAsync();
            // now fetching Navigation properties in Walk Domain
            // we will be using .include and it will tell enitity framework what to include while fetching
            return await 
                nZWalksDbContext.Walks
                .Include(x=>x.Region)
                .Include(x=>x.WalkDifficulty)
                .ToListAsync();
        }

        public async Task<Walk> GetAsync(Guid id)
        {
            var walk = await nZWalksDbContext.Walks
                .Include(x=>x.Region)
                .Include(x=>x.WalkDifficulty)
                .FirstOrDefaultAsync(x => x.Id == id);
            if(walk == null)
                return null;
            return walk;
        }

        public async Task<Walk> UpdateAsync(Guid id, Walk walk)
        {
            var ExistingWalk = await nZWalksDbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if (ExistingWalk == null)
                return null;
            ExistingWalk.Name = walk.Name;
            ExistingWalk.Length = walk.Length;
            ExistingWalk.RegionId = walk.RegionId;
            ExistingWalk.WalkDifficultyId = walk.WalkDifficultyId;
            await nZWalksDbContext.SaveChangesAsync();
            return ExistingWalk;
        }
    }
}
