using UnityEngine;

namespace AssemblyCSharp
{
	public class Noise
	{
		private SimplexNoiseGenerator generator;
		private float startX = Random.Range(100.0f, 200.0f);
		private float startY = Random.Range(200.0f, 300.0f);
		private float startZ = Random.Range (300.0f, 400.0f);
		public float scale = 0.1f;

		public Noise ()
		{
			generator = new SimplexNoiseGenerator ();
		}

		public float get(int i, int j, int k) {
			return generator
				.noise ((startX + i) * scale, (startY + j) * scale, (startZ + k) * scale) + 0.5f;
		}
	}
}

