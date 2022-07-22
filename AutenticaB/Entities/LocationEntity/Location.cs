using AutoMapper;

namespace TMan.Entities.LocationEntity
{
    public class Location
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }

    public class LocationDto
    {
        public string? Name { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }

    public class ExtremeLocationsDto
    {
        public LocationDto? NorthLocation { get; set; }
        public LocationDto? EastLocation { get; set; }
        public LocationDto? WestLocation { get; set; }
        public LocationDto? SouthLocation { get; set; }
    }

    public class LocationProfile : Profile
    {
        public LocationProfile()
        {
            CreateMap<Location, LocationDto>();
        }
    }

}





