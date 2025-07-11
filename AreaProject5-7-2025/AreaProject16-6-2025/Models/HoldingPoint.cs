using System.Xml.Linq;

namespace AreaProject16_6_2025.Models
{

    public class HoldingPoint
    {
        public int Id { get; set; }
        public string Name { get; set; } // اسم الـ Fix
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public string Direction { get; set; } // Right أو Left
        public TimeSpan LegDuration { get; set; } // مثلاً دقيقة
        public int AltitudeFeet { get; set; }

        public int AirportId { get; set; }

        public static List<HoldingPoint> LoadHoldingPointsFromXml()
        {
            string filePath = 
                "C:\\Users\\Click\\Desktop\\Area Projects\\AreaProject13-5-2025\\AreaProject13-5-2025VersionOne\\wwwroot\\AirportXmlFile\\holdingPoint.xml";
            var holdingPoints = new List<HoldingPoint>();

            if (!File.Exists(filePath))
                throw new FileNotFoundException("The XML file was not found.", filePath);

            XDocument doc = XDocument.Load(filePath);

            foreach (var element in doc.Descendants("HoldingPoint"))
            {
                try
                {
                    var hp = new HoldingPoint
                    {
                        Id = int.Parse(element.Element("Id")?.Value ?? "0"),
                        Name = element.Element("Name")?.Value,
                        Latitude = double.Parse(element.Element("Latitude")?.Value ?? "0"),
                        Longitude = double.Parse(element.Element("Longitude")?.Value ?? "0"),
                        Direction = element.Element("Direction")?.Value,
                        LegDuration = TimeSpan.Parse(element.Element("LegDuration")?.Value ?? "00:01:00"),
                        AltitudeFeet = int.Parse(element.Element("AltitudeFeet")?.Value ?? "0"),
                        AirportId = int.Parse(element.Element("AirportId")?.Value ?? "0")
                    };

                    holdingPoints.Add(hp);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error reading HoldingPoint: {ex.Message}");
                }
            }

            return holdingPoints;
        }


    }
}
