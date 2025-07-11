using AreaProject16_6_2025.DisplayDataBaseHelper;
using AreaProject16_6_2025.Models;
using AreaProject16_6_2025.ViewModel;
using System.Runtime.InteropServices;
using System.Text;

namespace AreaProject16_6_2025.DisplayhelperFunctions
{
    public class DisplsyhelperFunctions
    {
        #region Return Airort View Model From Airport Region

        public static AirportViewModel ReturnAirortViewModelFromAirport(Airport airport) //E031235364
        {
            AirportViewModel airportViewModel = new AirportViewModel();
            List<RunwayViewModel> runwaysViewModel = new List<RunwayViewModel>();
            foreach (var item in airport.Runways)
            {
                runwaysViewModel.Add(new RunwayViewModel
                {
                    //Id = item.Id,
                    //airpotId = item.airpotId,
                    //RWWidth = item.RWWidth,
                    //RWLength = item.RWLength,
                    //RWName = item.RWName,
                    //ThresholdCoordinates = item.ThresholdCoordinates 

                    Id = item.Id,
                    airpotId = item.airpotId,
                    RunwayWidth = item.RWWidth,
                    RunwayLength = item.RWLength,
                    RunwayName = item.RWName,
                    ThresholdCoordinates = item.ThresholdCoordinates,
                });

            }

            airportViewModel = new AirportViewModel()
            {
                AirportName = airport.AirportName,
                AirportReferencePoint = airport.AirportReferencePoint,
                ICAOIdentifier = airport.ICAOIdentifier,
                Id = airport.Id,
                runWaysViewModel = runwaysViewModel,
            };
            return airportViewModel;
        }
        #endregion

        #region Airport Functions Region

        #region Convert Latitude To Decimal Region
        public static double ConvertLatitudeToDecimal(string dms)  //like N30064100
        {
            int degrees = int.Parse(dms.Substring(1, 2));
            int minutes = int.Parse(dms.Substring(3, 2));
            double seconds = dms.Length > 6 ? double.Parse(dms.Substring(5, 2)) : 0; // ✅ لو مش موجود خليها صفر

            var latitudeDecimalValue = degrees + (minutes / 60.0) + (seconds / 3600.0);

            return latitudeDecimalValue;
        }
        #endregion

        #region Convert Longitude To Decimal Region
        public static double ConvertLongitudeToDecimal(string dms)//like E031245000 شرق 
        {
            if (string.IsNullOrEmpty(dms) || dms.Length < 9)
                throw new Exception("Invalid longitude format! Expected: 'WDDDMMSSSS' or 'EDDDMMSSSS'");

            // Extract components
            char direction = dms[0]; // 'E' or 'W'
            int degrees = int.Parse(dms.Substring(1, 3)); // DDD
            int minutes = int.Parse(dms.Substring(4, 2)); // MM
            double seconds = double.Parse(dms.Substring(6, 4)) / 100.0; // SSSS -> Convert to SS.ss

            // Convert to decimal format
            double decimalValue = degrees + (minutes / 60.0) + (seconds / 3600.0);

            // Apply negative sign for West (W)
            return direction == 'W' ? -decimalValue : decimalValue;
        }
        #endregion

        #region  Extract Coordinates Region
        public static (string Latitude, string Longitude) ExtractCoordinates(string input)
        {
            // تقسيم النص إلى أجزاء بناءً على المسافات
            string[] parts = input.Split(' ');

            if (parts.Length < 2)
                throw new Exception("Invalid format! Expected: 'Nxxxxxxx Exxxxxxxx xxxxxx'");

            string latitude = parts[0];  // الجزء الأول هو خط العرض
            string longitude = parts[1]; // الجزء الثاني هو خط الطول

            return (latitude, longitude);


        }
        #endregion
        #endregion

        #region Get Lat and Long From airport Data Region
        public static (double Latitude, double Longitude) GetLatandLongFromairportData(string val)  //like N30064100
        {
            int valueOfAirport=22222;
            if (val == "cairo")
            { valueOfAirport = 0; }
            else if (val == "Aswan")
            { valueOfAirport = 1; }
            else if (val == "hur")
            { valueOfAirport = 2; }

            DisplayHelperClassDataBase displayDataBase = new DisplayHelperClassDataBase();
            Airport cairoAirport = displayDataBase.GetFullAirportsData()[valueOfAirport];
            string refPoint = cairoAirport.AirportReferencePoint;
            (string Latitude, string Longitude) = DisplsyhelperFunctions.ExtractCoordinates(refPoint);

            double LatitudeDecimal = DisplsyhelperFunctions.ConvertLatitudeToDecimal(Latitude);
            double LongitudeDecimal = DisplsyhelperFunctions.ConvertLongitudeToDecimal(Longitude);
            return (LatitudeDecimal, LongitudeDecimal);
             
        }
        #endregion

        #region Get Lat and Long From airport Data Region
        public static List<Runway> retrieveRunwaysDataFromairportData()
        {
            DisplayHelperClassDataBase displayDataBase = new DisplayHelperClassDataBase();
            List<Runway> runwaysList = new List<Runway>();
            runwaysList = displayDataBase.GetRunways(1);
            return runwaysList;

        }
        #endregion

        #region runWay Extract Coordinates Region

        public static (string Latitude, string Longitude) runWayExtractCoordinates(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new ArgumentException("Input cannot be null or empty!");

            string[] parts = input.Trim().Split(' ');

            if (parts.Length < 3)  // Ensure we have Lat, Long, and Altitude
                throw new FormatException("Invalid format! Expected: 'Nxxxxxxx Exxxxxxxx xxxxxx'");

            return (parts[0], parts[1]);
        }

        public static double runWayConvertLatitudeToDecimal(string dms)
        {
            if (string.IsNullOrWhiteSpace(dms) || dms.Length < 8)
                throw new FormatException("Invalid latitude format! Expected: 'NDDMMSSSS'");

            try
            {
                char direction = dms[0]; // 'N' or 'S'
                int degrees = int.Parse(dms.Substring(1, 2)); // DD
                int minutes = int.Parse(dms.Substring(3, 2)); // MM
                double seconds = double.Parse(dms.Substring(5, 4)) / 100.0; // SSSS -> SS.ss

                double decimalValue = degrees + (minutes / 60.0) + (seconds / 3600.0);

                return direction == 'S' ? -decimalValue : decimalValue; // Negative for South
            }
            catch (Exception ex)
            {
                throw new FormatException($"Error converting latitude: {dms}. Details: {ex.Message}");
            }
        }

        public static double runWayConvertLongitudeToDecimal(string dms)
        {
            if (string.IsNullOrWhiteSpace(dms) || dms.Length < 9)
                throw new FormatException("Invalid longitude format! Expected: 'WDDDMMSSSS' or 'EDDDMMSSSS'");

            try
            {
                char direction = dms[0]; // 'E' or 'W'
                int degrees = int.Parse(dms.Substring(1, 3)); // DDD
                int minutes = int.Parse(dms.Substring(4, 2)); // MM
                double seconds = double.Parse(dms.Substring(6, 4)) / 100.0; // SSSS -> SS.ss

                double decimalValue = degrees + (minutes / 60.0) + (seconds / 3600.0);

                return direction == 'W' ? -decimalValue : decimalValue; // Negative for West
            }
            catch (Exception ex)
            {
                throw new FormatException($"Error converting longitude: {dms}. Details: {ex.Message}");
            }
        }
        #endregion

        #region  retrieve Runways Data Region
        public static List<RunwayViewModel> retrieveRunwaysData()
        {
            List<Runway> runwaysList = retrieveRunwaysDataFromairportData();
            List<RunwayViewModel> list = new List<RunwayViewModel>();

            foreach (var runW in runwaysList)
            {
                // ✅ Get Runway Start Coordinates
                var (runwayLatitude, runwayLongitude) = runWayExtractCoordinates(runW.ThresholdCoordinates);
                //var (runwayLatitude, runwayLongitude) = DisplayhelperFunctions.runWayExtractCoordinates(runW.ThresholdCoordinates);
                double runwayLatitudeInDecimal = runWayConvertLatitudeToDecimal(runwayLatitude);
                double runwayLongitudeInDecimal = runWayConvertLongitudeToDecimal(runwayLongitude);

                // ✅ Get Runway Length and Heading (Landing Course)
                double runwayLengthMeters = runW.RWLength;
                double landingCourseDegrees = runW.LandingCourse; // Direction in degrees

                // ✅ Calculate Runway End Coordinates
                DisplsyhelperFunctions helper = new DisplsyhelperFunctions();
                (double endLatitude, double endLongitude) = helper.CalculateDestinationPoint(
                    runwayLatitudeInDecimal,
                    runwayLongitudeInDecimal,
                    runwayLengthMeters,
                    landingCourseDegrees
                );

                RunwayViewModel runwayViewModel = new RunwayViewModel()
                {
                    RunwayName = runW.RWName,
                    RunwayLength = runwayLengthMeters,
                    RunwayWidth = runW.RWWidth,
                    RunwayStartLatitude = runwayLatitudeInDecimal,
                    RunwayStartLongitude = runwayLongitudeInDecimal,
                    RunwayEndLatitude = endLatitude,  // ✅ Correct End Coordinates 
                    RunwayEndLongitude = endLongitude,  // ✅ Correct End Coordinates
                                                        // 

                };

                list.Add(runwayViewModel);

            }

            return list;

        }
        #endregion

        #region  retrieve Runways Data Region
        public static List<holdingPointsViewModel> retrieveholdingPointsViewModelData()
        {
            DisplayHelperClassDataBase displayDataBase2 = new DisplayHelperClassDataBase();
            List<holdingPointsViewModel> list = new List<holdingPointsViewModel>();
            List<HoldingPoint> listRetrieve = displayDataBase2.GetHoldingPoints();


            foreach (var runW in listRetrieve)
            {
                holdingPointsViewModel runwayViewModel = new holdingPointsViewModel()
                {
                    Id = runW.Id,
                    Latitude = runW.Latitude,
                    Name = runW.Name,
                    Longitude = runW.Longitude,
                    Direction = runW.Direction,
                    AltitudeFeet = runW.AltitudeFeet,

                };

                list.Add(runwayViewModel);

            }

            return list;

        }
        #endregion

        #region Calculate Destination Point Region

        private const double EarthRadius = 6378137; // Earth’s radius in meters

        public (double, double) CalculateDestinationPoint(double lat, double lon, double distance, double bearing)
        {
            double radLat = ToRadians(lat);
            double radLon = ToRadians(lon);
            double radBearing = ToRadians(bearing);
            double angularDistance = distance / EarthRadius;

            double newLat = Math.Asin(
                Math.Sin(radLat) * Math.Cos(angularDistance) +
                Math.Cos(radLat) * Math.Sin(angularDistance) * Math.Cos(radBearing)
            );

            double newLon = radLon + Math.Atan2(
                Math.Sin(radBearing) * Math.Sin(angularDistance) * Math.Cos(radLat),
                Math.Cos(angularDistance) - Math.Sin(radLat) * Math.Sin(newLat)
            );

            return (ToDegrees(newLat), ToDegrees(newLon));
        }

        private double ToRadians(double degrees) => degrees * Math.PI / 180;
        private double ToDegrees(double radians) => radians * 180 / Math.PI;
        #endregion



    }
}
