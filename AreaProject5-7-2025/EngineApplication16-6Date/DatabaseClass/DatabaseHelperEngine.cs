using EngineApplication16_6Date.Models;
using MySql.Data.MySqlClient;

namespace EngineApplication16_6Date.DatabaseClass
{
    public class DatabaseHelperEngine
    {
        #region **🔹 Connection String Region **🔹 
        private readonly string connectionString =
              "server=localhost;port=3306;uid=root;pwd=root;";

        private readonly string connectionStringWithDatabaseName =
             "server=localhost;port=3306;database=Date5_5_2025CopyAirportSystemDatabaseVersion0ne;uid=root;pwd=root;";
        #endregion
         
        #region **🔹 Create Tables (Track & trackTransaction) 🔹**
        public void CreateTrackTables()
        {
            using (var connection = new MySqlConnection(connectionStringWithDatabaseName))
            {
                try
                {
                    connection.Open();
                    string createTrackTableQuery = @"
                                         CREATE TABLE IF NOT EXISTS track (
                                             Id INT AUTO_INCREMENT PRIMARY KEY,
                                             Name VARCHAR(50) NOT NULL
                                         );";

                    string createTransactionTableQuery = @"
                             CREATE TABLE IF NOT EXISTS trackTransaction (
                                 Id INT AUTO_INCREMENT PRIMARY KEY,
                                 TrackId INT,
                                 trackLatitude DOUBLE,
                                 trackLongitude DOUBLE,
                                 trackspeed DOUBLE,
                                 FOREIGN KEY (TrackId) REFERENCES track(Id) ON DELETE CASCADE
                             );";

                    using (var command = new MySqlCommand(createTrackTableQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    using (var command = new MySqlCommand(createTransactionTableQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    Console.WriteLine("create Track table successfully.");
                    Console.WriteLine("create trackTransaction table successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error When Create Track table  .");
                    Console.WriteLine("Error When Create trackTransaction table .  " + ex.Message);
                }
            }
        }
        #endregion

        #region **🔹 Get All Tracks with Transactions 🔹**
        public List<Track> GetAllTracks()
        {
            List<Track> tracks = new List<Track>();

            using (var connection = new MySqlConnection(connectionStringWithDatabaseName))
            {
                try
                {
                    connection.Open();
                    string query = @"
                           SELECT t.Id AS TrackId, t.Name AS TrackName, 
                                  trx.Id AS TransactionId, trx.trackLatitude, trx.trackLongitude, trx.trackspeed
                           FROM track t
                           LEFT JOIN trackTransaction trx ON t.Id = trx.TrackId;";

                    using (var command = new MySqlCommand(query, connection))
                    {
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
                                        Name = reader.GetString("TrackName"),  // ✅ استرجاع اسم المسار
                                        trackTransactions = new List<trackTransaction>()
                                    };
                                }

                                if (!reader.IsDBNull(reader.GetOrdinal("TransactionId")))
                                {
                                    trackDictionary[trackId].trackTransactions.Add(new trackTransaction
                                    {
                                        Id = reader.GetInt32("TransactionId"),
                                        trackLatitude = reader.GetDouble("trackLatitude"),
                                        trackLongitude = reader.GetDouble("trackLongitude"),
                                        trackspeed = reader.GetDouble("trackspeed"),
                                        TrackId = reader.GetInt32("TrackId"),
                                    });
                                }
                            }

                            tracks = trackDictionary.Values.ToList();
                        }
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ خطأ أثناء جلب المسارات: {ex.Message}");
                }
            }

            return tracks;
        }
        #endregion
         
        #region **🔹 Insert New Track and Transactions into Database 🔹**
        public void InsertNewTrack(Track track)
        {
            using (var connection = new MySqlConnection(connectionStringWithDatabaseName))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // إدخال المسار (Track) أولاً
                        string trackQuery = @"
                              INSERT INTO track (Name) 
                              VALUES (@Name);
                              SELECT LAST_INSERT_ID();";

                        int trackId;

                        using (var trackCommand = new MySqlCommand(trackQuery, connection, transaction))
                        {
                            trackCommand.Parameters.AddWithValue("@Name", track.Name);

                            object result = trackCommand.ExecuteScalar();
                            if (result != null && int.TryParse(result.ToString(), out trackId))
                            {
                                Console.WriteLine($" Track added with ID: {trackId} and Name: {track.Name}");
                            }
                            else
                            {
                                throw new Exception("⚠️ لم يتم إرجاع معرف المسار بعد الإدخال.");
                            }
                        }
                        foreach (var trx in track.trackTransactions)
                        {
                            // 1. احصل على عدد الـ TrackTransaction المرتبط بـ TrackId
                            string countQuery = "SELECT COUNT(*) FROM trackTransaction WHERE TrackId = @TrackId";
                            using (var countCommand = new MySqlCommand(countQuery, connection, transaction))
                            {
                                countCommand.Parameters.AddWithValue("@TrackId", trackId);
                                int currentCount = Convert.ToInt32(countCommand.ExecuteScalar());

                                // 2. لو العدد 10 أو أكثر، احذف أقدم واحد
                                if (currentCount >= 10)
                                {
                                    string deleteOldestQuery = @"
                DELETE FROM trackTransaction 
                WHERE TrackId = @TrackId 
                ORDER BY Id ASC 
                LIMIT 1;";

                                    using (var deleteCommand = new MySqlCommand(deleteOldestQuery, connection, transaction))
                                    {
                                        deleteCommand.Parameters.AddWithValue("@TrackId", trackId);
                                        deleteCommand.ExecuteNonQuery();
                                    }
                                }

                                // 3. أضف الـ trx الجديد
                                string trxQuery = @"
            INSERT INTO trackTransaction (TrackId, trackLatitude, trackLongitude, trackspeed) 
            VALUES (@TrackId, @TrackLatitude, @TrackLongitude, @trackspeed);";

                                using (var trxCommand = new MySqlCommand(trxQuery, connection, transaction))
                                {
                                    trxCommand.Parameters.AddWithValue("@TrackId", trackId);
                                    trxCommand.Parameters.AddWithValue("@TrackLatitude", trx.trackLatitude);
                                    trxCommand.Parameters.AddWithValue("@TrackLongitude", trx.trackLongitude);
                                    trxCommand.Parameters.AddWithValue("@trackspeed", trx.trackspeed);

                                    trxCommand.ExecuteNonQuery();
                                }
                            }
                        }


                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine($"❌ خطأ أثناء إدخال البيانات: {ex.Message}");
                    }
                }
            }
        }
        #endregion


    }
}
