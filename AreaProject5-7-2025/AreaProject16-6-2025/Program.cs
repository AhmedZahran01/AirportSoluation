using AreaProject16_6_2025.DisplayDataBaseHelper;
using AreaProject16_6_2025.Models;
using EngineApplication16_6Date.Helpers;

namespace AreaProject16_6_2025
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            #region Add many Airport Region

            if (true)
            {
                DisplayHelperClassDataBase classDataBase = new DisplayHelperClassDataBase();
                List<Airport> airways = new List<Airport>();
                string xmlPa = "";
                for (int i = 0; i < 2; i++)
                {
                    if (i == 0)
                    {
                        xmlPa = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "AirportXmlFile", "ASWAN-HESN.xml");
                        //xmlPa = "C:\\Users\\Click\\Desktop\\Area Projects\\AreaProject13-5-2025\\AreaProject13-5-2025VersionOne\\wwwroot\\AirportXmlFile\\ASWAN-HESN.xml";
                    }
                    else if (i == 1)
                    {
                        xmlPa = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "AirportXmlFile", "HURGHADA.xml");
                        //xmlPa = "C:\\Users\\Click\\Desktop\\Area Projects\\AreaProject13-5-2025\\AreaProject13-5-2025VersionOne\\wwwroot\\AirportXmlFile\\HURGHADA.xml";
                    }
                    Airport SS = Airport.LoadFromXml(xmlPa);
                    airways.Add(SS);
                    classDataBase.InsertAirportAndRunwaysOrNet(SS);
                    int xsshhl = 4;

                } 
                int xssl = 4; 
            }
            #endregion

            #region Create Data Base System Region
            builder.Services.AddSingleton<TrackManager>();
            //builder.Services.AddSingleton<HoldingEditTrackManager>();
            builder.Services.AddSingleton<TrackUpdateScheduler>();


            #region Create Data Base System Region
            bool CreateDataBaseSystem = false;
            if (CreateDataBaseSystem)
            {
                DisplayHelperClassDataBase displayDataBase = new DisplayHelperClassDataBase();
                displayDataBase.CreateAirportSystemDatabaseDataBase();
                displayDataBase.CreateAirportTable();
                displayDataBase.CreateRunwayTable();

                var x = Airport.LoadFromXml();
                bool CheckInsertOrNot = displayDataBase.InsertAirportAndRunwaysOrNet(x);
            }
            #endregion

            #region Create Holding Points Region
            bool CreateHoldingPoint = false;
            if (CreateHoldingPoint)
            {
                DisplayHelperClassDataBase displayDataBase2 = new DisplayHelperClassDataBase();
                bool allAdded = true;
                displayDataBase2.CreateHoldingPointTable();

                List<HoldingPoint> holdingPoints = HoldingPoint.LoadHoldingPointsFromXml();
                foreach (var holdingPoint in holdingPoints)
                {
                    bool CheckInsertOrNot = displayDataBase2.AddHoldingPoint(holdingPoint);
                    if (!CheckInsertOrNot)
                    {
                        allAdded = false;
                    }
                }
                if (allAdded) { Console.WriteLine(" All HoldingPoints is Added "); }
                if (!allAdded) { Console.WriteLine("All HoldingPoints is Not Added "); }

                List<HoldingPoint> CheckListData = displayDataBase2.GetHoldingPoints();
                int checkda = 00;
            }

            #endregion

            #endregion

            #region Add Track System Region
            bool addTrackSystem = false;
            if (addTrackSystem)
            {
                EnginehelperFunctions.createAndInsertTracksEngineToDataBaseSystem();
            }
            #endregion

            var app = builder.Build();

            #region MyRegion 
            var scheduler = app.Services.GetRequiredService<TrackUpdateScheduler>();
            //scheduler.Start();

            #endregion

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            #region Map Controllers Region

            app.MapControllers();
            #endregion

            app.Run();
        }
    }
}