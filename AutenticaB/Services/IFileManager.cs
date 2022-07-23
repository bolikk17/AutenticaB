using System.IO.Compression;
using System.Net;
using TMan.Entities.LocationEntity;

namespace AutenticaB.Services
{

    public interface IFileManager
    {
        bool DownloadFileByUrl(string url, string savePath);
        bool FileExists(string path);
        bool ExtractFile(string path, string savePath);
        List<Location> ReadCsvFile(string path);
    }

    public class FileManagerService : IFileManager
    {
        public bool DownloadFileByUrl(string url, string savePath)
        {
            using (WebClient webClient = new WebClient())
            {
                webClient.DownloadFile(url, savePath);
            }

            return true;
        }

        public bool ExtractFile(string path, string savePath)
        {
            using (ZipArchive archive = ZipFile.Open(path, ZipArchiveMode.Read))
            {
                ZipArchiveEntry? entry = archive.Entries.Where(e => e.FullName.Contains("CENTROIDI")).FirstOrDefault();

                if (entry != null)
                {
                    entry.ExtractToFile(savePath);
                    return true;
                }
                
                return false;
            }
        }

        public bool FileExists(string path)
        {
            return File.Exists(path);
        }

        public List<Location> ReadCsvFile(string path)
        {
            var reader = new StreamReader(path);
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
            reader.Close();

            return locations;
        }
    }
}