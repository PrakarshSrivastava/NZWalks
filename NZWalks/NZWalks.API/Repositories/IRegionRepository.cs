using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public interface IRegionRepository
    {
        // With the help of task we will make From Sync to Ansync Programming
        Task<IEnumerable<Region>> GetAllAsync();
    }
}
