using AutenticaB.Constants;
using AutoMapper;
using MediatR;
using System.IO.Compression;
using System.Net;
using TMan.Entities.LocationEntity;

namespace TMan.Entities.User.Commands
{
    public class GetExtremeLocations : IRequestHandler<GetExtremeLocations.Command, ExtremeLocationsDto>
    {
        private readonly IMapper _mapper;
        public GetExtremeLocations(IMapper mapper)
        {
            _mapper = mapper;
        }

        public class Command : IRequest<ExtremeLocationsDto> { }

        public async Task<ExtremeLocationsDto> Handle(Command request, CancellationToken cancellationToken)
        {
            bool csvFileIsMissing = !File.Exists(Const.FilePath + "/AW_VIETU_CENTROIDI.CSV");

            if (csvFileIsMissing)
            {
                bool zipFileIsMissing = !File.Exists(Const.FilePath + "/data.zip");

                if (zipFileIsMissing)
                {
                    using (WebClient webClient = new WebClient())
                    {
                        webClient.DownloadFile(Const.DownloadFileUrl, Const.FilePath + "/data.zip");
                    }
                }

                using (ZipArchive archive = ZipFile.Open(Const.FilePath + "/data.zip", ZipArchiveMode.Read))
                {
                    archive.ExtractToDirectory(Const.FilePath);
                }
            }

            var reader = new StreamReader(Const.FilePath + "/AW_VIETU_CENTROIDI.CSV");
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
            LocationDto northLocation = _mapper.Map<LocationDto>(locations.MaxBy(l => l.Latitude));
            LocationDto southLocation = _mapper.Map<LocationDto>(locations.MinBy(l => l.Latitude));
            LocationDto eastLocation = _mapper.Map<LocationDto>(locations.MaxBy(l => l.Longitude));
            LocationDto westLocation = _mapper.Map<LocationDto>(locations.MinBy(l => l.Longitude));



            return new ExtremeLocationsDto()
            {
                EastLocation = eastLocation,
                NorthLocation = northLocation,
                SouthLocation = southLocation,
                WestLocation = westLocation
            };
        }
    }
}
