namespace NZWalks.API.Models.DTO
{
    // Because We dont want Client To Ched Chad in Primary Key
    public class AddRegionRequest
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public double Area { get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }
        public long Population { get; set; }
    }
}
