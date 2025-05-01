using System;
using System.Collections;
using UnityEngine;

namespace Artimus.Extensions {
	public static class MonoBehaviourExtension {
		public static Coroutine Delay(this MonoBehaviour monoBehaviour, Action action, float delay) {
			var coroutine = _DelayCoroutine(action, delay);

			return monoBehaviour.StartCoroutine(coroutine);
		}

		public static IEnumerator _DelayCoroutine(Action action, float delay) {
			yield return new WaitForSeconds(delay);

			action();
		}

		public static void WaitOneFrame(this MonoBehaviour monoBehaviour, Action callback) {
			var coroutine = _DelayOneFrameCoroutine(callback);

			monoBehaviour.StartCoroutine(coroutine);
		}

		public static IEnumerator _DelayOneFrameCoroutine(Action callback) {
			yield return null;

			callback();
		}

		public static T Instantiate<T>(this T prefab, Vector3 position) where T : MonoBehaviour {
			return UnityEngine.Object.Instantiate(prefab, position, Quaternion.identity);
		}

		public static T Instantiate<T>(this T prefab, Transform parent) where T : MonoBehaviour {
			return UnityEngine.Object.Instantiate(prefab, parent);
		}

		public static T Instantiate<T>(this T prefab, GameObject parent) where T : MonoBehaviour {
			return UnityEngine.Object.Instantiate(prefab, parent.transform);
		}
	}
}