using UnityEngine;

namespace AssemblyCSharp
{
	public class Noise
	{
		private SimplexNoiseGenerator generator;
		private float startX = Random.Range(100.0f, 200.0f);
		private float startY = Random.Range(200.0f, 300.0f);
		private float startZ = Random.Range (300.0f, 400.0f);
		public float frequency = 0.1f;

		public Noise ()
		{
			generator = new SimplexNoiseGenerator ();
		}

		// Returns -1.0 to 1.0
		public float get(int i, int j, int k) {
			var value = generator
				.noise ((startX + i) * frequency, (startY + j) * frequency, (startZ + k) * frequency) * 5.0f;
			return value;
		}
	}
}

