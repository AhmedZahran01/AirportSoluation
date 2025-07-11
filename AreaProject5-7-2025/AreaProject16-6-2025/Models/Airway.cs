using AreaProject16_6_2025.Models;
using System;
using System.Globalization;
using System.Xml.Linq;

namespace AreaProject16_6_2025.Models
{
    public class Airway
    {
        public int Id { get; set; }
        public string Name { get; set; } // مثال: UL859
        public string Type { get; set; } // High or Low
        public double Width { get; set; } // in Nautical Miles
        public List<AirwaySegment> Segments { get; set; }

        public static Airway ParseAirwayFromXml(string xmlPath = null)
        {
            if (string.IsNullOrEmpty(xmlPath))
            {
                xmlPath = "C:\\Users\\Click\\Desktop\\Area Projects\\AreaProject13-5-2025\\AreaProject13-5-2025VersionOne\\wwwroot\\AirportXmlFile\\AirwaysData.xml";
            }

            XDocument doc = XDocument.Load(xmlPath);

            var airwayElement = doc.Element("Airway");
            if (airwayElement == null)
                throw new Exception("Invalid XML: Missing <Airway> element");

            var airway = new Airway
            {
                Id = (int)airwayElement.Attribute("Id"),
                Name = (string)airwayElement.Attribute("name"),
                Type = (string)airwayElement.Attribute("type"),
                Width = ParseWidth((string)airwayElement.Attribute("width")),
                Segments = new List<AirwaySegment>()
            };

            var segments = airwayElement
                .Element("Route")?
                .Elements("AirwaySegment");

            if (segments != null)
            {
                foreach (var seg in segments)
                {
                    var fromFix = seg.Element("FromFix");
                    var toFix = seg.Element("ToFix");

                    var segment = new AirwaySegment
                    {
                        FromFix = (string)fromFix?.Attribute("name"),
                        ToFix = (string)toFix?.Attribute("name"),
                        MinAltitude = (double?)seg.Element("MinAltitude") ?? 0,
                        MaxAltitude = (double?)seg.Element("MaxAltitude") ?? 0,
                        Distance = CalculateDistance(
                            lat1: (double?)fromFix?.Attribute("lat") ?? 0,
                            lon1: (double?)fromFix?.Attribute("lon") ?? 0,
                            lat2: (double?)toFix?.Attribute("lat") ?? 0,
                            lon2: (double?)toFix?.Attribute("lon") ?? 0
                        )
                    };

                    airway.Segments.Add(segment);
                }
            }

            return airway;
        }

        private static double ParseWidth(string widthStr)
        {
            if (widthStr != null && widthStr.EndsWith("NM", StringComparison.OrdinalIgnoreCase))
            {
                var numberPart = widthStr.Substring(0, widthStr.Length - 2);
                if (double.TryParse(numberPart, NumberStyles.Float, CultureInfo.InvariantCulture, out double result))
                    return result;
            }
            return 0;
        }

        // حساب المسافة بين نقطتين بالإحداثيات (تقريبية)
        private static double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            double R = 3440.065; // نصف قطر الأرض بالميل البحري
            double dLat = DegreesToRadians(lat2 - lat1);
            double dLon = DegreesToRadians(lon2 - lon1);
            double a =
                Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(DegreesToRadians(lat1)) * Math.Cos(DegreesToRadians(lat2)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }

        private static double DegreesToRadians(double deg) => deg * Math.PI / 180;


    }
}




