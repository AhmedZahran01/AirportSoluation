using EngineApplication16_6Date.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AreaProject16_6_2025.Controllers
{ 
    [Route("api/scheduler")]
    [ApiController]
    public class SchedulerController : ControllerBase
    {
        private readonly TrackUpdateScheduler _scheduler;

        public SchedulerController(TrackUpdateScheduler scheduler)
        {
            _scheduler = scheduler;
        }

        [HttpPost("start")]
        public IActionResult StartScheduler()
        {
            _scheduler.Start();
            return Ok("✅ Scheduler Started");
        }

        [HttpPost("stop")]
        public IActionResult StopScheduler()
        {
            _scheduler.Stop();
            return Ok("🛑 Scheduler Stopped");
        }
    }
     
}
