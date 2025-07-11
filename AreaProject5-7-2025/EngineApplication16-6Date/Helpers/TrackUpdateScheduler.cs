namespace EngineApplication16_6Date.Helpers
{
    public class TrackUpdateScheduler
    {
        private readonly Timer _timer;
        private readonly TrackManager _trackManager;
        //private readonly HoldingEditTrackManager _holdingManager;
        private bool _isRunning = false;

        public TrackUpdateScheduler(TrackManager trackManager/*, HoldingEditTrackManager holdingManager*/)
        {
            _trackManager = trackManager;
            //_holdingManager = holdingManager;
            _timer = new Timer(UpdateAllTracks, null, Timeout.Infinite, Timeout.Infinite);
        }

        public void Start()
        {
            if (!_isRunning)
            {
                Console.WriteLine("🟢 Track Scheduler Started");
                _timer.Change(0, 1000); // كل ثانية
                _isRunning = true;
            }
        }

        public void Stop()
        {
            if (_isRunning)
            {
                Console.WriteLine("🔴 Track Scheduler Stopped");
                _timer.Change(Timeout.Infinite, Timeout.Infinite);
                _isRunning = false;
            }
        }

        private void UpdateAllTracks(object state)
        {
            _trackManager.UpdateOnce();
            //_holdingManager.UpdateOnce();
        }
    }
}

