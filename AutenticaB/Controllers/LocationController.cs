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
            using (WebClient wc = new WebClient())
            {
                wc.DownloadFile("https://data.gov.lv/dati/dataset/0c5e1a3b-0097-45a9-afa9-7f7262f3f623/resource/1d3cbdf2-ee7d-4743-90c7-97d38824d0bf/download/aw_csv.zip", "test.zip");
            }

            using (ZipArchive archive = ZipFile.Open("test.zip", ZipArchiveMode.Read))
            {
                archive.ExtractToDirectory("Files");
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

                string latitudeString = values[8];
                string longitudeString = values[9];
                Location location = new Location()
                {
                    Id = Guid.NewGuid(),
                    Latitude = Convert.ToDouble(latitudeString.Substring(1, latitudeString.Length - 2)),
                    Longitude = Convert.ToDouble(longitudeString.Substring(1, longitudeString.Length - 2)),
                };
                locations.Add(location);
            }

            // TODO: Add check for empty "locations" list
            Location northLocation = locations.MinBy(l => l.Latitude)!;
            Location southLocation = locations.MaxBy(l => l.Latitude)!;
            Location eastLocation = locations.MinBy(l => l.Longitude)!;
            Location westLocation = locations.MaxBy(l => l.Longitude)!;


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