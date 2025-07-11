namespace AreaProject16_6_2025.ViewModel
{
    public class AirportViewModel
    {
        public int Id { get; set; }

        public string AirportName { get; set; }
        public string ICAOIdentifier { get; set; }
        public string AirportReferencePoint { get; set; }

        public List<RunwayViewModel> runWaysViewModel { get; set; }

    }
}
