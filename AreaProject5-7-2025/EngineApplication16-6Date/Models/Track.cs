using System.Globalization;
using System.Xml.Linq;

namespace EngineApplication16_6Date.Models
{
    public class Track
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<trackTransaction> trackTransactions { get; set; }

        #region **🔹 Load Tracks from XML 🔹**
        public static List<Track> LoadTracksFromXml(string filePath = "")
        {
            filePath = "C:\\Users\\Click\\Desktop\\Area Projects\\AreaProject5-5-2025VersionOne\\EngineApplication5-5Date\\trackXmlFile\\Track and trackTransactions  Data .xml";
            try
            {
                XDocument doc = XDocument.Load(filePath);
                var tracksElement = doc.Element("Tracks");

                if (tracksElement == null)
                    throw new Exception("❌ ملف XML لا يحتوي على العنصر <Tracks>");

                var tracks = tracksElement.Elements("Track")
                    .Select((track, index) => new Track
                    {
                        Id = index + 1, // تعيين ID بشكل تسلسلي
                        Name = track.Element("Name")?.Value,
                        trackTransactions = track.Element("trackTransactions")?.Elements("trackTransaction")
                            .Select(trx => new trackTransaction
                            {
                                trackLatitude = double.TryParse(trx.Element("trackLatitude")?.Value.Trim(),
                                                                NumberStyles.Any,
                                                                CultureInfo.InvariantCulture,
                                                                out double lat) ? lat : 0.0,

                                trackLongitude = double.TryParse(trx.Element("trackLongitude")?.Value.Trim(),
                                                                 NumberStyles.Any,
                                                                 CultureInfo.InvariantCulture,
                                                                 out double lon) ? lon : 0.0
                            })
                            .ToList() ?? new List<trackTransaction>() // تجنب القيم `null`
                    })
                    .ToList();

                return tracks;
            }
            catch (Exception ex)
            {
                Console.WriteLine($" : {ex.Message}");
                return new List<Track>(); // إرجاع قائمة فارغة في حالة الخطأ
            }
        }
        #endregion


    }
}
