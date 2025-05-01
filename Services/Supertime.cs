using System.Diagnostics;

namespace Artimus.Services {
	/**
	 * Measures code runtime in microseconds.
	 */
	public static class Supertime {
		public static Stopwatch _stopwatch = new();

		/**
		 * Pass minMicroseconds to only log spikes and hide all results below the given value.
		 *
		 * Call twice to use:
		 *
		 * Supertime.Track();
		 * [Your code]
		 * Supertime.Track("Your code is finished");
		 */
		public static void Track(string message = null, long minMicroseconds = 0) {
			if (_stopwatch.IsRunning) {
				_stopwatch.Stop();

				var microseconds = _stopwatch.ElapsedTicks / (Stopwatch.Frequency / (1000L * 1000L));

				if (microseconds > minMicroseconds) {
					UnityEngine.Debug.Log($"{message}: {microseconds}µs passed.");
				}

				_stopwatch.Reset();
			}
			else {
				_stopwatch.Start();
			}
		}
	}
}