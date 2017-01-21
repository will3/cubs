using System;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	public class Chunks
	{
		private float size = 16.0f;
		private int size_i = 16;
		private readonly Dictionary<string, Chunk> chunks = new Dictionary<string, Chunk>();

		public Voxel Get(int i, int j, int k) {
			var origin = GetOrigin (i, j, k);
			var key = origin [0] + "," + origin [1] + "," + origin [2];

			if (!chunks.ContainsKey (key)) {
				return null;
			}

			return  chunks [key].Get (i - origin[0], j - origin[1], k - origin[2]);
		}

		public void Set(int i, int j, int k, Voxel v) {
			var origin = GetOrigin (i, j, k);
			var key = origin [0] + "," + origin [1] + "," + origin [2];

			if (!chunks.ContainsKey (key)) {
				chunks [key] = new Chunk (new [] { size_i, size_i, size_i } );
			}

			chunks [key].Set (i - origin [0], j - origin [1], k - origin [2], v);
		}

		private int[] GetOrigin(int i, int j, int k) {
			return new [] { 
				(int)Math.Floor(i / size),
				(int)Math.Floor(j / size),
				(int)Math.Floor(k / size)
			};
		}
	}
}

