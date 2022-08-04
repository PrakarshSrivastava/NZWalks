using AutoMapper;

namespace NZWalks.API.Profiles
{
    public class RegionsProfile:Profile
    {
        public RegionsProfile()
        {
            // in create map we have two parameters which are source and destination
            // This ReverseMap will convert the the source model to the DTO Model
            CreateMap<Models.Domain.Region, Models.DTO.Region>()
                .ReverseMap();
        }
    }
}
