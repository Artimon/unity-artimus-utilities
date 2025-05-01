namespace ServicesNS {
	public static class StaticRandom {
		// @link https://github.com/OctoAwesome/octoawesome/blob/master/OctoAwesome/OctoAwesome.Basics/Noise/PerlinNoiseGenerator.cs
		// @link https://softwareengineering.stackexchange.com/questions/161336/how-to-generate-random-numbers-without-making-new-random-objects
		public static int Range(int minInclusive, int maxExclusive, int x, int y, int seed) {
			var num = _Randomize(x, y, seed);

			return minInclusive + (int)(num % (maxExclusive - minInclusive));
		}

		public static float Get(int x, int y, int seed) {
			var num = (float)_Randomize(x, y, seed);

			return num / uint.MaxValue;
		}

		private static uint _Randomize(int x, int y, int seed) {
			var num = (uint)seed;
			for (uint i = 0; i < 16; i++) {
				num = num * 541 + (uint)x;
				num = _BitRotate(num);
				num = num * 809 + (uint)y;
				num = _BitRotate(num);
				num = num * 673 + i;
				num = _BitRotate(num);
			}

			return num;
		}

		private static uint _BitRotate(uint x) {
			const int bits = 16;

			return (x << bits) | (x >> (32 - bits));
		}
	}
}