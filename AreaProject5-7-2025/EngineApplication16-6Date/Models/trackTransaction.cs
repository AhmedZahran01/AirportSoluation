namespace EngineApplication16_6Date.Models
{
    public class trackTransaction
    {
        public int Id { get; set; }
        public double trackLatitude { get; set; }
        public double trackLongitude { get; set; }
        public double trackspeed { get; set; } = 30;   
        public double altitude { get; set; } = 200;
        public double trackHeading { get; set; }
        
        public int TrackId { get; set; }  // 🔹 Foreign Key reference to Track
       
    }
}
