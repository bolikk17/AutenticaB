using AutenticaB.Constants;
using AutenticaB.Data;
using AutenticaB.Services;
using AutoMapper;
using MediatR;
using System.IO.Compression;
using System.Net;
using TMan.Entities.LocationEntity;

namespace TMan.Entities.User.Commands
{
    public class GetExtremeLocations : IRequestHandler<GetExtremeLocations.Command, ExtremeLocationsDto>
    {
        private DataContext _dataContext;
        private IFileManager _fileManager;
        private readonly IMapper _mapper;

        public GetExtremeLocations(
            IFileManager fileManager,
            IMapper mapper,
            DataContext DataContext
            )
        {
            _fileManager = fileManager;
            _dataContext = DataContext;
            _mapper = mapper;
        }

        public class Command : IRequest<ExtremeLocationsDto> { }

        public async Task<ExtremeLocationsDto> Handle(Command request, CancellationToken cancellationToken)
        {
            bool csvFileIsMissing = !_fileManager.FileExists(Const.FilePath + "/AW_VIETU_CENTROIDI.CSV");

            if (csvFileIsMissing)
            {
                bool zipFileIsMissing = !_fileManager.FileExists(Const.FilePath + "/data.zip");

                if (zipFileIsMissing)
                {
                    _fileManager.DownloadFileByUrl(Const.DownloadFileUrl, Const.FilePath + "/data.zip");
                }

                _fileManager.ExtractFile(Const.FilePath + "/data.zip", Const.FilePath + "/AW_VIETU_CENTROIDI.CSV");

            }

            List<Location> locations = _fileManager.ReadCsvFile(Const.FilePath + "/AW_VIETU_CENTROIDI.CSV");

            // Data may be saved in database
            // _dataContext.Locations.AddRange(locations);
            // await _dataContext.SaveChangesAsync();
            // _dataContext.Locations.ToList();

            // TODO: Add check for empty "locations" list
            // TODO: In case if we use this very often, will have to do serious research.
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
