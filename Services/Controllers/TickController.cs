using System;
using UnityEngine;

namespace Artimus.Services {
	/**
	 * Add this component to a GameObject in your scene to enable.
	 */
	public class TickController : MonoBehaviour {
		public void Update() {
			Tick.UpdateAll();
		}
	}

	public class Tick {
		public const float Ticks20 = 20f;
		public const float Ticks4 = 4f;

		public static readonly Tick[] _ticks20 = new Tick[5];
		public static readonly Tick[] _ticks4 = new Tick[5];

		public static Tick Get20 => _ticks20.GetRandomElement();
		public static Tick Get4 => _ticks4.GetRandomElement();

		public readonly float _ticks;
		public float _timer;
		public bool isNow;

		public event Action OnTick;

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		public static void OnLoad() {
			CreateTimers(_ticks20, Ticks20);
			CreateTimers(_ticks4, Ticks4);
		}

		public Tick(float timerOffset, float ticks) {
			_timer = timerOffset;
			_ticks = ticks;
		}

		public static void CreateTimers(Tick[] target, float ticks) {
			for (var i = 0; i < target.Length; ++i) {
				var timerOffset = i * 1f / target.Length;

				target[i] = new Tick(timerOffset, ticks);
			}
		}

		public void _UpdateTimer() {
			_timer -= _ticks * Time.deltaTime;

			if (_timer < 0f) {
				_timer = Mathf.Max(-0.5f, _timer) + 1f;

				isNow = true;

				OnTick?.Invoke();

				return;
			}

			isNow = false;
		}

		public static void UpdateAll() {
			foreach (var tick in _ticks20) {
				tick._UpdateTimer();
			}

			foreach (var tick in _ticks4) {
				tick._UpdateTimer();
			}
		}
	}
}