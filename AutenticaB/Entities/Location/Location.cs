namespace TMan.Entities.Location
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
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }

    public class ExtremeLocationsDTO
    {
        public Location? NorthLocation { get; set; }
        public Location? EastLocation { get; set; }
        public Location? WestLocation { get; set; }
        public Location? SouthLocation { get; set; }
    }
}





