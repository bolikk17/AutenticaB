using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;
using System.Net;
using TMan.Entities.Location;

namespace AutenticaB.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LocationController : ControllerBase
    {

        private readonly ILogger<LocationController> _logger;

        public LocationController(ILogger<LocationController> logger)
        {
            _logger = logger;
        }

        [HttpGet("[action]")]
        public ExtremeLocationsDTO Get()
        {

            bool downloadIsNeeded = !System.IO.File.Exists("Files/AW_VIETU_CENTROIDI.CSV");

            if (downloadIsNeeded)
            {
                using (WebClient wc = new WebClient())
                {
                    wc.DownloadFile("https://data.gov.lv/dati/dataset/0c5e1a3b-0097-45a9-afa9-7f7262f3f623/resource/1d3cbdf2-ee7d-4743-90c7-97d38824d0bf/download/aw_csv.zip", "Archive/data.zip");
                }

                using (ZipArchive archive = ZipFile.Open("Archive/data.zip", ZipArchiveMode.Read))
                {
                    var zipFile = ZipFile.OpenRead("Archive/data.zip");
                    if (zipFile == null)
                    {
                        archive.ExtractToDirectory("Files");
                    }
                }
            }

            var reader = new StreamReader("Files/AW_VIETU_CENTROIDI.CSV");
            // Skip 1st line
            reader.ReadLine();

            List<Location> locations = new List<Location>();

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if (line == null) continue;
                    
                var values = line.Split(';');
                if (values.Length != 10) continue;


                string latitudeString = values[8].Substring(1, values[8].Length - 2);
                string longitudeString = values[9].Substring(1, values[9].Length - 2);
                string name = values[2].Substring(1, values[2].Length - 2);

                Location location = new Location()
                {
                    Id = Guid.NewGuid(),
                    Name = name,
                    Latitude = Convert.ToDouble(latitudeString),
                    Longitude = Convert.ToDouble(longitudeString),
                };
                locations.Add(location);
            }

            // TODO: Add check for empty "locations" list
            // TODO: In case if we use this very ofen, will have to do serious research.
            // Maybe it will be better to sort data by long/lat (https://en.wikipedia.org/wiki/Quicksort) and save two separated data strtuctures,
            // so that max/min elements are first/last and no need to find them. 
            // Also binary search will be able in case we would like to find location with specific lat/log.
            Location northLocation = locations.MaxBy(l => l.Latitude)!;
            Location southLocation = locations.MinBy(l => l.Latitude)!;
            Location eastLocation = locations.MaxBy(l => l.Longitude)!;
            Location westLocation = locations.MinBy(l => l.Longitude)!;


            return new ExtremeLocationsDTO() 
            { 
                EastLocation = eastLocation, 
                NorthLocation = northLocation, 
                SouthLocation = southLocation, 
                WestLocation = westLocation 
            };
        }
    }
}