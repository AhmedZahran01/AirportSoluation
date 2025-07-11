namespace AreaProject16_6_2025.ViewModel
{
    public class holdingPointsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } 

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public string Direction { get; set; } // Right أو Left
 
        public int AltitudeFeet { get; set; }

        public int LegDuration { get; set; } = 1;

    }
}
