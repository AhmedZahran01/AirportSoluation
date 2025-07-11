namespace AreaProject16_6_2025.ViewModel
{
    public class ControllerScreenViewModel
    {
        public double LatitudeCairoDecimal { get; set; }
        public double LongitudeCairoDecimal { get; set; }
        
        public double LatitudeAswanDecimal { get; set; }
        public double LongitudeAswanDecimal { get; set; }

        public double LatitudehurDecimal { get; set; }
        public double LongitudehurDecimal { get; set; }
        
        public string signOnSelection { get; set; }
        public List<RunwayViewModel> runwaysList { get; set; }
        public List<holdingPointsViewModel> holdingPointsViewModels { get; set; }


    }
}
