using System.Collections.Generic;
using UnityEngine;

namespace Artimus.Services {
	// @link https://stackoverflow.com/questions/3706219/algorithm-for-iterating-over-an-outward-spiral-on-a-discrete-2d-grid-from-the-or
	public class SpiralOut {
		public int _layer;
		public int _leg;

		public Vector2Int position = Vector2Int.zero;

		public int _step;
		public int _maxStep;

		public SpiralOut(int maxRadius) {
			var width = 2 * maxRadius + 1;

			_maxStep = width * width;
		}

		// @TODO Cache resulting array.
		public static Vector2Int[] GetSpiral(int radius) {
			var spiralOut = new SpiralOut(radius);

			var spiral = new List<Vector2Int>();

			while (true) {
				var chunkSectorOffset = spiralOut.GetNext();
				if (chunkSectorOffset == null) {
					break;
				}

				spiral.Add((Vector2Int)chunkSectorOffset);
			}

			return spiral.ToArray();
		}

		public Vector2Int? GetNext() {
			_step += 1;

			if (_step > _maxStep) {
				return null;
			}

			if (_layer == 0) {
				_layer += 1;

				return position;
			}

			switch (_leg) {
				case 0:
					position.x += 1;
					if (position.x == _layer) {
						_leg += 1;
					}

					break;

				case 1:
					position.y += 1;
					if (position.y == _layer) {
						_leg += 1;
					}

					break;

				case 2:
					position.x -= 1;
					if (-position.x == _layer) {
						_leg += 1;
					}

					break;

				case 3:
					position.y -= 1;
					if (-position.y == _layer) {
						_layer += 1;
						_leg = 0;
					}

					break;
			}

			return position;
		}
	}
}