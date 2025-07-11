using AreaProject16_6_2025.DisplayhelperFunctions;
using AreaProject16_6_2025.Models;
using AreaProject16_6_2025.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AreaProject16_6_2025.Controllers
{
    public class HomeController : Controller
    {
        #region Constractor Region
        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        #endregion

        #region Index Region
        public IActionResult Index()
        {
            return View();
        }
        #endregion

        #region Map Region Region

        #region Controller Region
        public IActionResult ControllerScreen(string signOnSelection)
        {
            (double LatitudecairoDecimal, double LongitudecairoDecimal) = DisplsyhelperFunctions.
                                                                  GetLatandLongFromairportData("cairo");

            (double LatitudeAswanDecimal, double LongitudeAswanDecimal) = DisplsyhelperFunctions.
                                                                  GetLatandLongFromairportData("Aswan");

            (double LatitudehurDecimal, double LongitudehurDecimal) = DisplsyhelperFunctions.
                                                                  GetLatandLongFromairportData("hur");

            List<RunwayViewModel> runwayViewModels = DisplsyhelperFunctions.retrieveRunwaysData();
            List<holdingPointsViewModel> holdingPointsViewModels = DisplsyhelperFunctions.retrieveholdingPointsViewModelData();

            ControllerScreenViewModel controllerScreenViewModel =
                                          new ControllerScreenViewModel()
                                          {
                                              LatitudeCairoDecimal = LatitudecairoDecimal,
                                              LongitudeCairoDecimal = LongitudecairoDecimal,

                                              LatitudeAswanDecimal = LatitudeAswanDecimal,
                                              LongitudeAswanDecimal = LongitudeAswanDecimal,

                                              LatitudehurDecimal = LatitudehurDecimal,
                                              LongitudehurDecimal = LongitudehurDecimal,

                                              signOnSelection = signOnSelection,
                                              runwaysList = runwayViewModels,
                                              holdingPointsViewModels = holdingPointsViewModels
                                          };
            ViewBag.controllerScreenViewModel = controllerScreenViewModel;
            return View();
        }
        #endregion

        #region Planner Region 
        public IActionResult PlannerScreen(string signOnSelection)
        {
            (double LatitudeDecimal, double LongitudeDecimal) = DisplsyhelperFunctions.GetLatandLongFromairportData("cairo"); 
            ControllerScreenViewModel controllerScreenViewModel =
                                          new ControllerScreenViewModel()
                                          { 
                                              LatitudeCairoDecimal = LatitudeDecimal,
                                              LongitudeCairoDecimal = LongitudeDecimal,
                                              signOnSelection = signOnSelection
                                          };
            ViewBag.controllerScreenViewModel = controllerScreenViewModel; 
            return View();
        }
        #endregion
        #endregion

        #region Privacy and Error Region

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #endregion

    }
}
