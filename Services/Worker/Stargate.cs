using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Artimus.Services.Worker {
	/**
	 * Thread worker implementation to receive (enter) and return (callback).
	 */
	public class Stargate {
		/*
		 * Insight:
		 * Using a list will result in "list has changed during iteration", even when only modified in one process.
		 * Using an array will result in the objects inside the array not necessarily all being updated, while the
		 * array is being checked.
		 * Using concurrent collections ensures both.
		 */
		public readonly ConcurrentQueue<IExplorer> _explorersIn = new ();
		public readonly ConcurrentQueue<IExplorer> _explorersOut = new ();

		public readonly AutoResetEvent _autoResetEvent = new (false);
		public readonly CancellationTokenSource _cancellationTokenSource = new ();

		#region MainThread

		public Stargate() {
			var cancellationToken = _cancellationTokenSource.Token;

			Task.Factory.StartNew(() => {
				while (true) {
					if (cancellationToken.IsCancellationRequested) {
						break;
					}

					_ProcessQueue();

					_autoResetEvent.WaitOne(); // Waiting for the next "Set()" call to awake the thread.
				}
			});
		}

		public void StopWorker() {
			_cancellationTokenSource.Cancel();
			_autoResetEvent.Set(); // Continue to check for cancellation.
		}

		public void Enter(IExplorer explorer) {
			_explorersIn.Enqueue(explorer);
			_autoResetEvent.Set(); // Continue at the wait inside the task.
		}

		public void Finish() {
			while (true) {
				// Remove first, to prevent deadlock on crashing explorers.
				var success = _explorersOut.TryDequeue(out var explorer);
				if (!success) {
					break;
				}

				explorer.Finish();
			}
		}

		#endregion

		#region WorkerThread

		public void _ProcessQueue() {
			while (true) {
				// Remove first, to prevent deadlock on crashing explorers.
				var success = _explorersIn.TryDequeue(out var explorer);
				if (!success) {
					break;
				}

				try {
					explorer.Process();
				}
				catch (Exception exception) {
					Debug.LogError(exception);
				}

				_explorersOut.Enqueue(explorer);
			}
		}

		#endregion
	}
}