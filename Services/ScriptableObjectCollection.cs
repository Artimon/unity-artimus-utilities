#if UNITY_EDITOR
using UnityEditor;
#endif

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Artimus.Services {
	/**
	 * Enables you to add a ScriptableObject to a master collection
	 * without having to navigate to the master collection.
	 */
	public class ScriptableObjectCollection<T> : ScriptableObject where T : ScriptableObject {
		public T[] list;

		public virtual void OnItemAdded(T scriptableObject) { }

#if UNITY_EDITOR
		public bool TryAdd(T scriptableObject) {
			if (list.Contains(scriptableObject)) {
				return false;
			}

			list = new List<T>(list) {
					scriptableObject
				}
				.OrderBy(i => i.name)
				.ToArray();

			OnItemAdded(scriptableObject);

			EditorUtility.SetDirty(this);

			return true;
		}
#endif
	}
}