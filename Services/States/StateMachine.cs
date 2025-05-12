using System;
using System.Collections.Generic;
using UnityEngine;

namespace Artimus.Services.States {
	/**
	 * A state machine to be added to a GameObject.
	 * States can thus be fully configured in the inspector
	 * and have easy access to game objects and components.
	 */
	public class StateMachine : MonoBehaviour {
		public Dictionary<string, StateBase> _states = new ();

		// This is necessary to have all states available after instantiation.
		[HideInInspector]
		public StateBase[] _hiddenStates;

		[System.NonSerialized]
		public StateBase _state;

		public delegate void StateChangeHandler(StateBase state);

		public event StateChangeHandler OnBeforeStateChange;
		public event StateChangeHandler OnStateChanged;

		private void Awake() {
			var defaultState = (StateBase)null;

			foreach (var state in _hiddenStates) {
				_states.Add(state.Name, state);

				if (state.isDefault) {
					defaultState = state;
				}
			}

			if (!defaultState) {
				return;
			}

			_state = defaultState;
		}

		private void Start() {
			/*
			 * Calling OnEnter() at start allows other components to subscribe to events
			 * and instantiate other required classes.
			 */
			_state.OnEnter();
		}

		private void Update() {
			_state.OnUpdate(Time.deltaTime);
		}

		/**
		 * No TryGetState, since it is mandatory to configure the state machine correctly.
		 */
		public T GetState<T>() where T : StateBase {
			foreach (var state in _states.Values) {
				if (state is T typedState) {
					return typedState;
				}
			}

			return null;
		}

		public void Add(StateBase state) {
			_states.Add(state.Name, state);
		}

		public bool TryEnter(string stateName) {
			var success = _states.TryGetValue(stateName, out var state);
			if (!success) {
				return false;
			}

			return TryEnter(state);
		}

		public bool TryEnter(StateBase state) {
			if (!CanEnter(state)) {
				return false;
			}

			Force(state);

			return true;
		}

		public void Force(string stateName) {
			var success = _states.TryGetValue(stateName, out var state);
			if (!success) {
				return;
			}

			Force(state);
		}

		public void Force(StateBase state) {
			OnBeforeStateChange?.Invoke(_state);

			_state.OnExit();
			_state = state;
			_state.OnEnter();

			OnStateChanged?.Invoke(_state);
		}

		public void ForceIfNotInState(string stateName) {
			var success = _states.TryGetValue(stateName, out var state);
			if (!success) {
				return;
			}

			ForceIfNotInState(state);
		}

		public void ForceIfNotInState(StateBase state) {
			if (IsInState(state)) {
				return;
			}

			Force(state);
		}

		public bool CanEnter(string stateName) {
			var success = _states.TryGetValue(stateName, out var state);
			if (!success) {
				return false;
			}

			return CanEnter(state);
		}

		public bool CanEnter(StateBase state) {
			if (IsInState(state)) {
				return false;
			}

			return _state.CanExit();
		}

		public bool IsInState(string stateName) {
			return _state.Name == stateName;
		}

		public bool IsInState(StateBase state) {
			return _state == state;
		}

		// Cannot auto-assign actor, since it is unknown in this assembly.
		public void AddHidden(StateBase stateBase) {
			_hiddenStates ??= Array.Empty<StateBase>();

			if (!_hiddenStates.Contains(stateBase)) {
				_hiddenStates = _hiddenStates.Append(stateBase);
			}
		}
	}
}