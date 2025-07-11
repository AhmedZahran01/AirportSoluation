namespace AreaProject16_6_2025.Models
{
    public class AirwaySegment
    {
        public int Id { get; set; }
        public string FromFix { get; set; }
        public string ToFix { get; set; }
        public double MinAltitude { get; set; }
        public double MaxAltitude { get; set; }
        public double Distance { get; set; } // optional
    }

}
