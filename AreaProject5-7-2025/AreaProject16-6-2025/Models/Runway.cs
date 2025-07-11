using System.Xml.Linq;

namespace AreaProject16_6_2025.Models
{
    public class Runway
    {
        public int Id { get; set; }

        #region 8 Column For Properties of runWay Region

        public int airpotId { get; set; }
        public string RWName { get; set; } // اسم المدرج (مثلاً RW17L تعني المدرج 17 يسار)
        public string Status { get; set; } // حالة المدرج (0: مغلق، 1: مفتوح، إلخ)
        public double LandingCourse { get; set; } // اتجاه الهبوط بالنسبة للبوصلة (بالدرجات)
        public double RWLength { get; set; } // طول المدرج بالأمتار
        public double RWWidth { get; set; } // عرض المدرج بالأمتار

        public string ShowSSA { get; set; } // هل يتم عرض منطقة السلامة السطحية (SSA) لهذا المدرج؟
                                            //Here                                       
        public string ThresholdCoordinates { get; set; } // إحداثيات نقطة بداية المدرج (العرض، الطول، الارتفاع)
        public string ShowMeteoOnRunway { get; set; } // هل يتم عرض بيانات الطقس على المدرج؟
        #endregion
         
        #region Function To Return List<Runway>  Load From Xml file Region

        public static List<Runway> LoadRunways(XElement runwaysElement)
        {
            return runwaysElement?.Elements("RW")
                .Select(rw => new Runway
                {
                    RWName = rw.Element("RWName")?.Value,
                    Status = rw.Element("Status")?.Value,
                    ShowSSA = rw.Element("ShowSSA")?.Value,
                    ThresholdCoordinates = rw.Element("ThresholdCoordinates")?.Value,
                    LandingCourse = double.TryParse(rw.Element("LandingCourse")?.Value, out double LandingCourse) ? LandingCourse : 0.0,
                    RWLength = double.TryParse(rw.Element("RWLength")?.Value, out double rwLength) ? rwLength : 0.0,
                    RWWidth = double.TryParse(rw.Element("RWWidth")?.Value, out double RWWidth) ? RWWidth : 0.0,
                    ShowMeteoOnRunway = rw.Element("ShowMeteoOnRunway")?.Value,
                     
                })
                .ToList() ?? new List<Runway>();
        }
        #endregion
         
    }

}