using UnityEngine;

namespace ServicesNS {
	/**
	 * Creates bezier-like results with a <linear> time input from 0 to 1.
	 */
	public static class BezierAlike {
		public static float Ease(float time) {
			float sinus;
			float power;
			float correction;
			float correctionStrength = 0.02f;
			float sum;
			float result;

			sinus = 1f - Mathf.Sin(time * Mathf.PI / 2f);
			power = Mathf.Pow(1f - time, 4f);
			correction = correctionStrength * Mathf.Sin(time * Mathf.PI * 2.5f);

			sum = sinus * 0.9f + power * 0.1f;

			result = Mathf.Cos(sum * Mathf.PI);
			result = (1f + result) / 2f;
			result = result * (1f - correctionStrength) + correction;

			// result = (1 + Math.cos(sinus * Math.PI)) / 2;

			return result;
		}

		// Close to ease-in: 0.4, 0, 1, 1
		public static float EaseIn(float time) {
			return 1f - Mathf.Cos(time * Mathf.PI / 2f);
		}

		// Close to ease-in: 0, 0, 0.6, 1
		public static float EaseOut(float time) {
			return Mathf.Sin(time * Mathf.PI / 2f);
		}

		public static float EaseInOut(float time) {
			return (1f - Mathf.Cos(time * Mathf.PI)) / 2f;
		}
	}
}