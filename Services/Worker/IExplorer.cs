namespace Artimus.Services.Worker {
	/**
	 * A certain type of job that is sent through the corresponding
	 * Stargate and to be expected to return with the result (callback).
	 */
	public interface IExplorer {
		// Is executed in the worker thread.
		public void Process();

		// Is executed in the main thread after the processing is done.
		public void Finish() { }
	}
}