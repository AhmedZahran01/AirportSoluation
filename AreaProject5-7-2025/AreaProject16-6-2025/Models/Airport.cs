using System.Xml.Linq;

namespace AreaProject16_6_2025.Models
{
    public class Airport
    {
        public int Id { get; set; }

        #region Airport Properties Data Region

        #region  🏢 معلومات عامة عن المطار 

        public string AirportName { get; set; } // اسم المطار
        public string ICAOIdentifier { get; set; } // كود المطار حسب منظمة الطيران المدني الدولي (ICAO)
        public string MilICAOIdentifier { get; set; } // كود المطار العسكري حسب ICAO (إن وجد)
        public string IATADesignator { get; set; } // كود المطار حسب اتحاد النقل الجوي الدولي (IATA)
        public string TMAName { get; set; } // اسم منطقة تحكم المطار (Terminal Control Area)

        #endregion

        #region 🔹 بيانات التشغيل والمجال الجوي

        public string CivilMilitaryPrivate { get; set; } // نوع المطار: مدني، عسكري، أو خاص
        public string MagneticTrueIndicator { get; set; } // مؤشر يحدد استخدام الشمال المغناطيسي أو الحقيقي
        public string MagneticVariation { get; set; } // الفرق بين الشمال المغناطيسي والشمال الحقيقي بالمطار
        public string ActiveFlag { get; set; } // حالة تفعيل المطار (نشط أو غير نشط)
        public string AirportReferencePoint { get; set; } // نقطة مرجعية للمطار تشمل الإحداثيات والارتفاع
        public string AltitudeFor3DVisualization { get; set; } // الارتفاع المستخدم في التصور ثلاثي الأبعاد
        public string Type { get; set; } // نوع المطار (مثلاً: دولي، محلي، عسكري، إلخ)


        #endregion

        #region 🏗️ بيانات المدرجات والتوجيهات

        public string RunwayTaxiWeight { get; set; } // الحد الأقصى لوزن الطائرات على المدرج أو التاكسي
        public string TaxiSpeedApron { get; set; } // سرعة التاكسي داخل منطقة الوقوف (Apron)
        public string TaxiSpeedTWY { get; set; } // سرعة التاكسي داخل ممرات التاكسي (Taxiway)
        public string TaxiSpeedRunway { get; set; } // سرعة التاكسي على المدرج (Runway)
        public string CalcRouteByMinTime { get; set; } // تحديد المسار بناءً على أقل وقت ممكن
        public string AbleToChangeSIDWithProbeRoute { get; set; } // إمكانية تغيير مسار المغادرة القياسي (SID) عند فحص المسار
        public string AbleToChangeSTARAppWithProbeRoute { get; set; } // إمكانية تغيير مسار الوصول القياسي (STAR) عند فحص المسار

        #endregion
        public List<Runway> Runways { get; set; }
        #endregion


        #region Function To Return Airport Load From Xml file Region

        public static Airport LoadFromXml(string filePath = null)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                filePath = "C:\\Users\\Click\\Desktop\\Area Projects\\AreaProject5-5-2025VersionOne\\AreaProject5-5-2025VersionOne\\wwwroot\\AirportXmlFile\\HECA -CAIRO.xml";
            }
            XDocument doc = XDocument.Load(filePath);
            var airportElement = doc.Element("Airport");

            if (airportElement == null) return null;

            var x = new Airport
            {
                AirportName = airportElement.Element("AirportName")?.Value,
                ICAOIdentifier = airportElement.Element("AirportICAOIdentifier")?.Value,
                MilICAOIdentifier = airportElement.Element("AirportMilICAOIdentifier")?.Value,
                IATADesignator = airportElement.Element("AirportIATADesignator")?.Value,
                TMAName = airportElement.Element("TMAName")?.Value,

                CivilMilitaryPrivate = airportElement.Element("CivilMilitaryPrivate")?.Value,
                MagneticTrueIndicator = airportElement.Element("MagneticTrueIndicator")?.Value,
                MagneticVariation = airportElement.Element("MagneticVariation")?.Value,
                ActiveFlag = airportElement.Element("ActiveFlag")?.Value,
                AirportReferencePoint = airportElement.Element("AirportReferencePoint")?.Value,
                AltitudeFor3DVisualization = airportElement.Element("AltitudeFor3DVisualization")?.Value,
                Type = airportElement.Element("Type")?.Value,

                RunwayTaxiWeight = airportElement.Element("RunwayTaxiWeight")?.Value,
                TaxiSpeedApron = airportElement.Element("TaxiSpeedApron")?.Value,
                TaxiSpeedTWY = airportElement.Element("TaxiSpeedTWY")?.Value,
                TaxiSpeedRunway = airportElement.Element("TaxiSpeedRunway")?.Value,
                CalcRouteByMinTime = airportElement.Element("CalcRouteByMinTime")?.Value,
                AbleToChangeSIDWithProbeRoute = airportElement.Element("AbleToChangeSIDWithProbeRoute")?.Value,
                AbleToChangeSTARAppWithProbeRoute = airportElement.Element("AbleToChangeSTARAppWithProbeRoute")?.Value,

                Runways = Runway.LoadRunways(airportElement.Element("RunWays")),


            };

            return x;
        }
        #endregion



    }

}