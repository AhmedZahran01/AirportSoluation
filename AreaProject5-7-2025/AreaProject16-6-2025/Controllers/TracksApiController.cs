using AreaProject16_6_2025.DisplayDataBaseHelper;
using AreaProject16_6_2025.ViewModel;
using EngineApplication16_6Date.Helpers;
using EngineApplication16_6Date.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AreaProject16_6_2025.Controllers
{
    [Route("api/tracksApiJson")]
    [ApiController]
    public class TracksApiController : ControllerBase
    {
        #region Constractor Region
        private readonly TrackManager _trackManager;

        // ✅ استخدم DI بدلاً من إنشاء TrackManager يدويًا
        public TracksApiController(TrackManager trackManager)
        {
            _trackManager = trackManager;
        }
        #endregion

        #region Get Latest Track Transactions Region

        [HttpGet("latest")]
        public IActionResult GetLatest
            (bool checkStartScenario = false, bool checkPouse = false, bool deleteTrackOne = false)
        {
            if (checkStartScenario)
            {
                _trackManager.StartLocalUpdates(); // تأكد المؤقت شغال
                var currentTracks = _trackManager.GetCurrentTracks();
                // خذ نسخة آمنة (مثلاً عن طريق ToList أو deep clone)
                var safeTracks = currentTracks.Select(track => new Track
                {
                    Id = track.Id,
                    Name = track.Name,
                    //trackTransactions = track.trackTransactions?.ToList() ?? new List<trackTransaction>()
                    trackTransactions = track.trackTransactions != null
                ? new List<trackTransaction>(track.trackTransactions)
                : new List<trackTransaction>()
                    // انسخ الحقول الأخرى المطلوبة بنفس الطريقة
                }).ToList();
                if (deleteTrackOne)
                {
                    safeTracks.RemoveAt(0);
                }
                var tracksJson = Newtonsoft.Json.JsonConvert.SerializeObject(safeTracks); // ✅ تعديل طريقة التحويل إلى JSON
                return Ok(tracksJson); // اترك ASP.NET Core يتولى التحويل إلى JSON
            }

            else if (checkPouse)
            {
                _trackManager.endLocalUpdates(); // تأكد المؤقت شغال
                return Ok();
            }

            else
            {
                _trackManager.endLocalUpdates(); // تأكد المؤقت شغال
                return Ok();
            }

        }
        #endregion

        #region Get Latest Track Transactions Region

        [HttpGet("getHoldingPoints")]
        public IActionResult GetHoldingPoints()
        {
            DisplayHelperClassDataBase displayHelper = new DisplayHelperClassDataBase();
            var listOfHoldingPoints = displayHelper.GetHoldingPoints(); // استخدم الكود اللي فوق

            var listOfHoldingPointsJson = Newtonsoft.Json.JsonConvert.SerializeObject(listOfHoldingPoints); // ✅ تعديل طريقة التحويل إلى JSON

            return Ok(listOfHoldingPointsJson);

        }
        #endregion

        #region Get Latest Track Transactions Region

        [HttpGet("updateLocationOfTrack1FromTracks")]
        public IActionResult updateLocationOfTrack1FromTracks()
        {
            _trackManager.updateLocationOfTrack1FromTracks(); // تأكد المؤقت شغال
            return Ok(); // اترك ASP.NET Core يتولى التحويل إلى JSON
        }

        #endregion
         
    } 
}
