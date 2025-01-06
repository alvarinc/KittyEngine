using System.Diagnostics;

namespace KittyEngine.Core.Services
{
    public class GameTime
    {
        private Stopwatch _totalStopwatch;
        private Stopwatch _deltaStopwatch;

        public GameTime()
        {
            _totalStopwatch = new Stopwatch();
            _deltaStopwatch = new Stopwatch();
        }

        public void Reset()
        {
            _totalStopwatch.Reset();
            _deltaStopwatch.Reset();
        }

        public void Restart()
        {
            _totalStopwatch.Restart();
            _deltaStopwatch.Restart();
        }

        public void Pause()
        {
            _totalStopwatch.Stop();
            _deltaStopwatch.Stop();
        }

        public void Start()
        {
            _totalStopwatch.Start();
            _deltaStopwatch.Start();
        }

        public void Mark()
        {
            _deltaStopwatch.Restart();
        }

        public TimeSpan DeltaTime => _deltaStopwatch.Elapsed;
        public TimeSpan TotalTime => _totalStopwatch.Elapsed;
    }
}
