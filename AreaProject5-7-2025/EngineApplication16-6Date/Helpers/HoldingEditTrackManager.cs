using EngineApplication16_6Date.Models;
using MySql.Data.MySqlClient;

namespace EngineApplication16_6Date.Helpers
{
    public class HoldingEditTrackManager
    {
        #region Constractor Region

        int numberOfTrackid;
        private readonly Timer localTrackUpdateTimer;
        private List<Track> localTracks = new List<Track>();
        private readonly string connectionStringWithDatabaseName =
            "server=localhost;port=3306;database=Date5_5_2025CopyAirportSystemDatabaseVersion0ne;uid=root;pwd=root;";
        private bool isTimerRunning = false;
        int counter = 0;
        public HoldingEditTrackManager( )
        {
            localTrackUpdateTimer = new Timer(localUpdateTracks, null, Timeout.Infinite, Timeout.Infinite);
            localTracks = GetAllTracksBasedOnDeleteTrackId(1); // مبدأياً جلب التراكات من الداتابيز
            
        }
        #endregion

        public List<Track> GetCurrentTracks()
        {
            return localTracks;
        }

        public void StartLocalUpdates(int id)
        {
            numberOfTrackid = id;
            localTrackUpdateTimer.Change(0, 1000);

            if (counter == 0)
            {
                Console.WriteLine("✅ Local Track Update Timer Started.");
                counter++;
            }
        }
        public void endLocalUpdates()
        {
            localTrackUpdateTimer.Change(Timeout.Infinite, Timeout.Infinite); // إيقاف المؤقت
            isTimerRunning = false;
            Console.WriteLine("  ==\n Timer Stopped.");
        }

        #region Toggle Track Updates Region
        // ✅ تشغيل أو إيقاف المؤقت بناءً على الحالة
        public void ToggleTrackUpdates(bool enable = true)
        {
            if (enable)
            {
                if (!isTimerRunning)
                {
                    localTrackUpdateTimer.Change(0, 1000);
                    //isTimerRunning = false;
                    Console.WriteLine(" \r\n        =======================================================================================\n Timer Started.");

                }
            }
            else
            {
                if (!isTimerRunning)
                {
                    localTrackUpdateTimer.Change(Timeout.Infinite, Timeout.Infinite); // إيقاف المؤقت
                    //isTimerRunning = false;
                    Console.WriteLine("        =======================================================================================        =======================================================================================\r\n=========== ==\n Timer Stopped.");
                }
            }
        }
        #endregion

        private void localUpdateTracks(object state)
        {
            if (localTracks.Count == 0)
            {
                localTracks = GetAllTracksBasedOnDeleteTrackId(numberOfTrackid);
            }

            foreach (var track in localTracks)
            {
                double moveLat = 0, moveLng = 0, newtrackHeading2 = 0;
                var last = track.trackTransactions.LastOrDefault();

                if (last == null)
                {
                    continue;
                }

                switch (track.Id)
                {
                    //case 0: moveLat = 0.0004; break; // شمال
                    case 1: moveLng = 0.00009; newtrackHeading2 = 90; break; // شرق
                    case 2: moveLat = 0.00009; newtrackHeading2 = 0; break; // شمال
                    case 3: moveLng = -0.00009; newtrackHeading2 = 270; break; // غرب
                    case 4: moveLat = -0.00009; newtrackHeading2 = 180; break; // جنوب
                }

                var newLat = last.trackLatitude + moveLat;
                var newLng = last.trackLongitude + moveLng;

                // احسب الزاوية بين الإحداثي القديم والجديد
                var newtrackHeading = CalculateHeading(last.trackLatitude, last.trackLongitude, newLat, newLng);


                var newTransaction = new trackTransaction()
                {
                    Id = 0, // لأنه لسه مش محفوظ في DB
                    TrackId = track.Id,
                    trackLatitude = last.trackLatitude + moveLat,
                    trackLongitude = last.trackLongitude + moveLng,
                    trackHeading = newtrackHeading,

                };

                track.trackTransactions.Add(newTransaction);
                if (track.Id == 4)
                {
                    Console.WriteLine($" Added new : {track.Name} , {newTransaction.trackLatitude}  , {newTransaction.trackLongitude} ");
                }
                //Console.WriteLine($"➕ Added new transaction to Track: {track.Name}  {newTransaction.trackLatitude} + {newTransaction.trackLongitude} ");
            }
        }

        private double CalculateHeading(double lat1, double lon1, double lat2, double lon2)
        {
            var dLon = (lon2 - lon1) * Math.PI / 180.0;
            lat1 = lat1 * Math.PI / 180.0;
            lat2 = lat2 * Math.PI / 180.0;

            var y = Math.Sin(dLon) * Math.Cos(lat2);
            var x = Math.Cos(lat1) * Math.Sin(lat2) -
                    Math.Sin(lat1) * Math.Cos(lat2) * Math.Cos(dLon);
            var brng = Math.Atan2(y, x) * 180.0 / Math.PI;
            return (brng + 360.0) % 360.0;
        }

         
        #region Get All Tracks Region
        public List<Track> GetAllTracksBasedOnDeleteTrackId(int numberOfTrackId)
        {
            List<Track> tracks = new List<Track>();

            try
            {
                using (var connection = new MySqlConnection(connectionStringWithDatabaseName))
                {
                    connection.Open();

                    string query = @"
                SELECT t.Id AS TrackId, t.Name, trx.Id AS TransactionId, trx.trackLatitude, trx.trackLongitude, trx.trackspeed 
                FROM track t
                LEFT JOIN trackTransaction trx ON t.Id = trx.TrackId
                WHERE t.Id != @trackId;";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        // إضافة البراميتر
                        command.Parameters.AddWithValue("@trackId", numberOfTrackId);

                        using (var reader = command.ExecuteReader())
                        {
                            var trackDictionary = new Dictionary<int, Track>();

                            while (reader.Read())
                            {
                                int trackId = reader.GetInt32("TrackId");

                                if (!trackDictionary.ContainsKey(trackId))
                                {
                                    trackDictionary[trackId] = new Track
                                    {
                                        Id = trackId,
                                        Name = reader.IsDBNull(reader.GetOrdinal("Name")) ? "Unknown" : reader.GetString("Name"),
                                        trackTransactions = new List<trackTransaction>()
                                    };
                                }

                                if (!reader.IsDBNull(reader.GetOrdinal("TransactionId")))
                                {
                                    trackDictionary[trackId].trackTransactions.Add(new trackTransaction
                                    {
                                        Id = reader.GetInt32("TransactionId"),
                                        TrackId = trackId,
                                        trackLatitude = reader.GetDouble("trackLatitude"),
                                        trackLongitude = reader.GetDouble("trackLongitude"),
                                        trackspeed = reader.GetDouble("trackspeed")
                                    });
                                }
                            }

                            tracks = trackDictionary.Values.ToList();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error when get Tracks " + ex.Message);
            }

            return tracks;
        }
        #endregion

        public void UpdateOnce()
        {
            localUpdateTracks(null); // نداء يدوي للفنكشن
        }

    }
}