using AreaProject16_6_2025.Models;
using MySql.Data.MySqlClient;

namespace AreaProject16_6_2025.DisplayDataBaseHelper
{
    public class DisplayHelperClassDataBase
    {
        #region Connection String Variables Region
        private readonly string connectionString =
        "server=localhost;port=3306;uid=root;pwd=root;";
        private readonly string connectionStringWithDatabaseName =
         "server=localhost;port=3306;database=Date5_5_2025CopyAirportSystemDatabaseVersion0ne;uid=root;pwd=root;";

        string DataBaseName = "Date5_5_2025CopyAirportSystemDatabaseVersion0ne";

        #endregion

        #region **🔹 Create Data Base Region **🔹
        public void CreateAirportSystemDatabaseDataBase()
        {
            using (var connection = new MySqlConnection(connectionString)) // ملاحظة: لا يجب أن يحتوي على اسم قاعدة البيانات
            {
                try
                {
                    connection.Open();
                    string createDatabaseQuery = $"CREATE DATABASE IF NOT EXISTS {DataBaseName} ;";

                    using (var command = new MySqlCommand(createDatabaseQuery, connection))
                    {
                        command.ExecuteNonQuery(); // ✅ تنفيذ الأمر لإنشاء قاعدة البيانات
                    }

                    Console.WriteLine("Create Database is Done .");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error where Create Data Base : " + ex.Message);
                }
            }
        }
        #endregion

        #region Create tables Region

        #region **🔹 Create Airport Table Region **🔹
        public void CreateAirportTable()
        {
            string connectionStringWithDb = connectionStringWithDatabaseName; // الاتصال بقاعدة البيانات

            using (var connection = new MySqlConnection(connectionStringWithDb))
            {
                try
                {
                    connection.Open();

                    string createTableQuery = @"
                CREATE TABLE IF NOT EXISTS Airport (
                    Id INT AUTO_INCREMENT PRIMARY KEY,
                    AirportName VARCHAR(50),
                    ICAOIdentifier VARCHAR(50),
                    MilICAOIdentifier VARCHAR(50),
                    IATADesignator VARCHAR(50),
                    TMAName VARCHAR(50),
                    CivilMilitaryPrivate VARCHAR(10),
                    MagneticTrueIndicator VARCHAR(10),
                    MagneticVariation VARCHAR(10),
                    ActiveFlag VARCHAR(10),
                    AirportReferencePoint VARCHAR(50),
                    AltitudeFor3DVisualization VARCHAR(10),
                    Type VARCHAR(10),
                    RunwayTaxiWeight VARCHAR(10),
                    TaxiSpeedApron VARCHAR(10),
                    TaxiSpeedTWY VARCHAR(10),
                    TaxiSpeedRunway VARCHAR(10),
                    CalcRouteByMinTime VARCHAR(10),
                    AbleToChangeSIDWithProbeRoute VARCHAR(10),
                    AbleToChangeSTARAppWithProbeRoute VARCHAR(10)
                );";

                    using (var command = new MySqlCommand(createTableQuery, connection))
                    {
                        command.ExecuteNonQuery(); // ✅ تنفيذ الاستعلام لإنشاء الجدول
                    }

                    Console.WriteLine("Create Airport is Done .");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("❌ خطأ أثناء إنشاء جدول Airport: " + ex.Message);
                }
            }
        }
        #endregion

        #region **🔹 Create Runway Table Region **🔹
        public void CreateRunwayTable()
        {
            string connectionStringWithDb = connectionStringWithDatabaseName; // الاتصال بقاعدة البيانات

            using (var connection = new MySqlConnection(connectionStringWithDb))
            {
                try
                {
                    connection.Open();

                    string createTableQuery = @"
                CREATE TABLE IF NOT EXISTS Runway (
                    Id INT AUTO_INCREMENT PRIMARY KEY,
                    AirportId INT,
                    RWName VARCHAR(50),
                    Status VARCHAR(10),
                    ShowSSA VARCHAR(10),
                    ThresholdCoordinates VARCHAR(50),
                    LandingCourse DOUBLE,
                    RWLength DOUBLE,
                    RWWidth DOUBLE,
                    ShowMeteoOnRunway VARCHAR(10),
                    FOREIGN KEY (AirportId) REFERENCES Airport(Id) ON DELETE CASCADE
                );";

                    using (var command = new MySqlCommand(createTableQuery, connection))
                    {
                        command.ExecuteNonQuery(); // ✅ تنفيذ الاستعلام لإنشاء الجدول
                    }

                    Console.WriteLine("Create Runway is Done .");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("❌ خطأ أثناء إنشاء جدول Runway: " + ex.Message);
                }
            }
        }
        #endregion
        #endregion

        #region Insert Airport from XML To My SQL  
        public bool InsertAirportAndRunwaysOrNet(Airport airport)
        {
            using (var connection = new MySqlConnection(connectionStringWithDatabaseName))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction()) // 🔥 استخدم المعاملات لضمان الإدخال الآمن
                {
                    try
                    {
                        // 📌 إدخال بيانات المطار
                        string airportQuery = @"
                         INSERT INTO Airport (
                             AirportName, ICAOIdentifier, MilICAOIdentifier, IATADesignator, TMAName, 
                             CivilMilitaryPrivate, MagneticTrueIndicator, MagneticVariation, ActiveFlag, AirportReferencePoint, 
                             AltitudeFor3DVisualization, Type, RunwayTaxiWeight, TaxiSpeedApron, TaxiSpeedTWY, 
                             TaxiSpeedRunway, CalcRouteByMinTime, AbleToChangeSIDWithProbeRoute, AbleToChangeSTARAppWithProbeRoute
                         ) VALUES (
                             @AirportName, @ICAOIdentifier, @MilICAOIdentifier, @IATADesignator, @TMAName, 
                             @CivilMilitaryPrivate, @MagneticTrueIndicator, @MagneticVariation, @ActiveFlag, @AirportReferencePoint, 
                             @AltitudeFor3DVisualization, @Type, @RunwayTaxiWeight, @TaxiSpeedApron, @TaxiSpeedTWY, 
                             @TaxiSpeedRunway, @CalcRouteByMinTime, @AbleToChangeSIDWithProbeRoute, @AbleToChangeSTARAppWithProbeRoute
                         ); SELECT LAST_INSERT_ID();";

                        using (var airportCommand = new MySqlCommand(airportQuery, connection, transaction))
                        {
                            #region 19 Column For Airport Region
                            airportCommand.Parameters.AddWithValue("@AirportName", airport.AirportName);
                            airportCommand.Parameters.AddWithValue("@ICAOIdentifier", airport.ICAOIdentifier);
                            airportCommand.Parameters.AddWithValue("@MilICAOIdentifier", airport.MilICAOIdentifier);
                            airportCommand.Parameters.AddWithValue("@IATADesignator", airport.IATADesignator);
                            airportCommand.Parameters.AddWithValue("@TMAName", airport.TMAName);
                            airportCommand.Parameters.AddWithValue("@CivilMilitaryPrivate", airport.CivilMilitaryPrivate);
                            airportCommand.Parameters.AddWithValue("@MagneticTrueIndicator", airport.MagneticTrueIndicator);
                            airportCommand.Parameters.AddWithValue("@MagneticVariation", airport.MagneticVariation);
                            airportCommand.Parameters.AddWithValue("@ActiveFlag", airport.ActiveFlag);
                            airportCommand.Parameters.AddWithValue("@AirportReferencePoint", airport.AirportReferencePoint);
                            airportCommand.Parameters.AddWithValue("@AltitudeFor3DVisualization", airport.AltitudeFor3DVisualization);
                            airportCommand.Parameters.AddWithValue("@Type", airport.Type);
                            airportCommand.Parameters.AddWithValue("@RunwayTaxiWeight", airport.RunwayTaxiWeight);
                            airportCommand.Parameters.AddWithValue("@TaxiSpeedApron", airport.TaxiSpeedApron);
                            airportCommand.Parameters.AddWithValue("@TaxiSpeedTWY", airport.TaxiSpeedTWY);
                            airportCommand.Parameters.AddWithValue("@TaxiSpeedRunway", airport.TaxiSpeedRunway);
                            airportCommand.Parameters.AddWithValue("@CalcRouteByMinTime", airport.CalcRouteByMinTime);
                            airportCommand.Parameters.AddWithValue("@AbleToChangeSIDWithProbeRoute", airport.AbleToChangeSIDWithProbeRoute);
                            airportCommand.Parameters.AddWithValue("@AbleToChangeSTARAppWithProbeRoute", airport.AbleToChangeSTARAppWithProbeRoute);

                            #endregion

                            int airportId = Convert.ToInt32(airportCommand.ExecuteScalar());

                            #region Run Ways of My Airport Region
                            // 📌 إدخال المدرجات
                            foreach (var runway in airport.Runways)
                            {
                                string runwayQuery = @"
                                  INSERT INTO Runway (AirportId, RWName, Status, ShowSSA, ThresholdCoordinates, LandingCourse, RWLength, RWWidth, ShowMeteoOnRunway) 
                                  VALUES (@AirportId, @RWName, @Status, @ShowSSA, @ThresholdCoordinates, @LandingCourse, @RWLength, @RWWidth, @ShowMeteoOnRunway);
                                  SELECT LAST_INSERT_ID();";

                                using (var runwayCommand = new MySqlCommand(runwayQuery, connection, transaction))
                                {

                                    #region 9 Values For RunWay Column Region

                                    runwayCommand.Parameters.AddWithValue("@AirportId", airportId);
                                    runwayCommand.Parameters.AddWithValue("@RWName", runway.RWName);
                                    runwayCommand.Parameters.AddWithValue("@Status", runway.Status);
                                    runwayCommand.Parameters.AddWithValue("@ShowSSA", runway.ShowSSA);
                                    runwayCommand.Parameters.AddWithValue("@ThresholdCoordinates", runway.ThresholdCoordinates);
                                    runwayCommand.Parameters.AddWithValue("@LandingCourse", runway.LandingCourse);
                                    runwayCommand.Parameters.AddWithValue("@RWLength", runway.RWLength);
                                    runwayCommand.Parameters.AddWithValue("@RWWidth", runway.RWWidth);
                                    runwayCommand.Parameters.AddWithValue("@ShowMeteoOnRunway", runway.ShowMeteoOnRunway);

                                    #endregion

                                    int runwayId = Convert.ToInt32(runwayCommand.ExecuteScalar());

                                }

                            }
                            #endregion

                        }
                        transaction.Commit(); // ✅ حفظ جميع التغييرات
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback(); // ❌ إلغاء التغييرات إذا حدث خطأ
                        Console.WriteLine($"❌ حدث خطأ أثناء الإدخال: {ex.Message}");
                        return false;
                    }
                }

            }
        }
        #endregion

        #region **🔹 Retrieve All Airports Data Region **🔹
        public List<Airport> GetFullAirportsData()
        {
            List<Airport> airports = new List<Airport>();

            using (var connection = new MySqlConnection(connectionStringWithDatabaseName))
            {
                connection.Open();

                string airportQuery = "SELECT * FROM Airport";

                using (var airportCommand = new MySqlCommand(airportQuery, connection))
                using (var reader = airportCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var airport = new Airport
                        {
                            #region Retrieve 19 Column For Airport Region

                            Id = Convert.ToInt32(reader["Id"]),
                            AirportName = reader["AirportName"].ToString(),
                            ICAOIdentifier = reader["ICAOIdentifier"].ToString(),
                            MilICAOIdentifier = reader["MilICAOIdentifier"].ToString(),
                            IATADesignator = reader["IATADesignator"].ToString(),
                            TMAName = reader["TMAName"].ToString(),
                            CivilMilitaryPrivate = reader["CivilMilitaryPrivate"].ToString(),
                            MagneticTrueIndicator = reader["MagneticTrueIndicator"].ToString(),
                            MagneticVariation = reader["MagneticVariation"].ToString(),
                            ActiveFlag = reader["ActiveFlag"].ToString(),
                            AirportReferencePoint = reader["AirportReferencePoint"].ToString(),
                            AltitudeFor3DVisualization = reader["AltitudeFor3DVisualization"].ToString(),
                            Type = reader["Type"].ToString(),
                            RunwayTaxiWeight = reader["RunwayTaxiWeight"].ToString(),
                            TaxiSpeedApron = reader["TaxiSpeedApron"].ToString(),
                            TaxiSpeedTWY = reader["TaxiSpeedTWY"].ToString(),
                            TaxiSpeedRunway = reader["TaxiSpeedRunway"].ToString(),
                            CalcRouteByMinTime = reader["CalcRouteByMinTime"].ToString(),
                            AbleToChangeSIDWithProbeRoute = reader["AbleToChangeSIDWithProbeRoute"].ToString(),
                            AbleToChangeSTARAppWithProbeRoute = reader["AbleToChangeSTARAppWithProbeRoute"].ToString(),

                            #endregion

                            Runways = new List<Runway>(),
                        };

                        airports.Add(airport);
                    }
                }
            }

            // 📌 Retrieve Runways & Related Data Using Separate Connections
            foreach (var airport in airports)
            {
                airport.Runways = GetRunways(airport.Id);
            }

            return airports;
        }
        #endregion

        #region **🔹 Retrieve All Runways Data  Based on Id of Airport Region **🔹
        public List<Runway> GetRunways(int airportId)
        {
            List<Runway> runways = new List<Runway>();

            using (var connection = new MySqlConnection(connectionStringWithDatabaseName))
            {
                connection.Open();

                string query = "SELECT * FROM Runway WHERE AirportId = @AirportId";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@AirportId", airportId);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var runway = new Runway
                            {
                                #region Retrieve 9 Values For RunWay Column Region

                                Id = Convert.ToInt32(reader["Id"]),
                                RWName = reader["RWName"].ToString(),
                                Status = reader["Status"].ToString(),
                                ShowSSA = reader["ShowSSA"].ToString(),
                                ThresholdCoordinates = reader["ThresholdCoordinates"].ToString(),
                                LandingCourse = Convert.ToDouble(reader["LandingCourse"]),
                                RWLength = Convert.ToDouble(reader["RWLength"]),
                                RWWidth = Convert.ToDouble(reader["RWWidth"]),
                                ShowMeteoOnRunway = reader["ShowMeteoOnRunway"].ToString()

                                #endregion

                            };

                            runways.Add(runway);
                        }
                    }
                }
            }

            return runways;
        }
        #endregion

        #region **🔹 Retrieve All Runways Data  Based on Id of Airport Region **🔹
         
        #region **🔹 Create HoldingPoint Table Region **🔹
        public void CreateHoldingPointTable()
        {
            string connectionStringWithDb = connectionStringWithDatabaseName; // الاتصال بقاعدة البيانات

            using (var connection = new MySqlConnection(connectionStringWithDb))
            {
                try
                {
                    connection.Open();

                    string createTableQuery = @"
        CREATE TABLE IF NOT EXISTS HoldingPoint (
            Id INT AUTO_INCREMENT PRIMARY KEY,
            Name VARCHAR(50), -- اسم الـ Fix
            Latitude DOUBLE,
            Longitude DOUBLE,
            Direction VARCHAR(10), -- Right أو Left
            LegDuration TIME, -- مدة البقاء في الـ leg
            AltitudeFeet INT,
            AirportId INT,
            FOREIGN KEY (AirportId) REFERENCES Airport(Id) ON DELETE CASCADE ON UPDATE CASCADE
        );";

                    using (var command = new MySqlCommand(createTableQuery, connection))
                    {
                        command.ExecuteNonQuery(); // ✅ تنفيذ الاستعلام لإنشاء الجدول
                    }

                    Console.WriteLine("✅ Create HoldingPoint is Done.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("❌ خطأ أثناء إنشاء جدول HoldingPoint: " + ex.Message);
                }
            }
        }
        #endregion

        #region 🔹 Add HoldingPoint Region
        public bool AddHoldingPoint(HoldingPoint hp)
        {
            try
            {
                using (var conn = new MySqlConnection(connectionStringWithDatabaseName))
                {
                    conn.Open();

                    var cmd = new MySqlCommand(@"
                INSERT INTO HoldingPoint 
                (Name, Latitude, Longitude, Direction, LegDuration, AltitudeFeet, AirportId) 
                VALUES 
                (@Name, @Latitude, @Longitude, @Direction, @LegDuration, @AltitudeFeet, @AirportId);
            ", conn);

                    cmd.Parameters.AddWithValue("@Name", hp.Name);
                    cmd.Parameters.AddWithValue("@Latitude", hp.Latitude);
                    cmd.Parameters.AddWithValue("@Longitude", hp.Longitude);
                    cmd.Parameters.AddWithValue("@Direction", hp.Direction);
                    cmd.Parameters.AddWithValue("@LegDuration", hp.LegDuration);
                    cmd.Parameters.AddWithValue("@AltitudeFeet", hp.AltitudeFeet);
                    cmd.Parameters.AddWithValue("@AirportId", hp.AirportId);

                    cmd.ExecuteNonQuery();
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ خطأ أثناء إضافة HoldingPoint: " + ex.Message);
                return false;
            }
        }
        #endregion

        #region 🔹 Get Holding Points Region
        public List<HoldingPoint> GetHoldingPoints()
        {
            List<HoldingPoint> list = new List<HoldingPoint>();

            using (var conn = new MySqlConnection(connectionStringWithDatabaseName))
            {
                conn.Open();
                var cmd = new MySqlCommand("SELECT * FROM HoldingPoint", conn);
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new HoldingPoint
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Name = reader["Name"].ToString(),
                        Latitude = Convert.ToDouble(reader["Latitude"]),
                        Longitude = Convert.ToDouble(reader["Longitude"]),
                        Direction = reader["Direction"].ToString(),
                        LegDuration = (TimeSpan)reader["LegDuration"],
                        AltitudeFeet = Convert.ToInt32(reader["AltitudeFeet"]),
                        AirportId = Convert.ToInt32(reader["AirportId"])
                    });
                }
            }

            return list;
        }
        #endregion

        #endregion


    }
}
