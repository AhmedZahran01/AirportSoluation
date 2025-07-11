using EngineApplication16_6Date.DatabaseClass;
using EngineApplication16_6Date.Models;

namespace EngineApplication16_6Date.Helpers
{
    public class EnginehelperFunctions
    {
        #region Recreate DataBase Region
        public static void createAndInsertTracksEngineToDataBaseSystem()
        {
            DatabaseHelperEngine helperEngine = new DatabaseHelperEngine();
            helperEngine.CreateTrackTables();
            var tracks = Track.LoadTracksFromXml();
            foreach (var track in tracks)
            {
                helperEngine.InsertNewTrack(track);
            } 

        }
        #endregion 

        #region start Timer  Region
        //public static void startTimer(bool checkStartTimer)
        //{ 
        //    TrackManager _trackManager = new TrackManager();
        //    _trackManager.ToggleTrackUpdates(true);
             
        //}
        #endregion


    }
}
