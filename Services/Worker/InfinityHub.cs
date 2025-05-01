using UnityEngine;

namespace Artimus.Services.Worker {
	/**
	 * Creating and accessing all Stargate instances.
	 * Automatically creates a Replicator to manage the Infinity Hub.
	 *
	 * Add further Stargate instances to the Infinity Hub as needed.
	 */
	public class InfinityHub {
		public static InfinityHub _instance;

		public readonly Stargate _stargateGeneric = new ();

		public static InfinityHub Instance {
			get {
				return _instance ??= Replicator.Factory();
			}
		}

		public void Shutdown() {
			_stargateGeneric.StopWorker();

			_instance = null;
		}

		public void Update() {
			_stargateGeneric.Finish();
		}

		public void EnterGateGeneric(IExplorer explorer) {
			_stargateGeneric.Enter(explorer);
		}

		public class Replicator : MonoBehaviour {
			public InfinityHub _infinityHub;

			private void Update() {
				_infinityHub.Update();
			}

			public static InfinityHub Factory() {
				var gameObject = new GameObject("Infinity Hub Replicator");
				var replicator = gameObject.AddComponent<Replicator>();

				replicator._infinityHub = new InfinityHub();

				return replicator._infinityHub;
			}

			private void OnDestroy() {
				_infinityHub.Shutdown();
			}
		}
	}
}