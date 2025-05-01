using UnityEngine;

namespace Artimus.Services.States {
	/**
	 * Extend from this class to create states for the state machine.
	 * They'll automatically add themselves to the state machine if placed on the same GameObject.
	 */
	public abstract class StateBase : MonoBehaviour {
		public StateMachine2 _stateMachine;

		public bool isDefault;

		public abstract string Name { get; }

		public virtual void OnEnter() { }
		public virtual void OnUpdate(float deltaTime) { }
		public virtual void OnExit() { }

		public virtual bool CanExit() {
			return true;
		}

#if UNITY_EDITOR
		private void OnValidate() {
			if (Application.isPlaying) {
				return;
			}

			if (_stateMachine) {
				return;
			}

			_stateMachine = GetComponent<StateMachine2>();
			_stateMachine.AddHidden(this);
		}
#endif
	}
}


