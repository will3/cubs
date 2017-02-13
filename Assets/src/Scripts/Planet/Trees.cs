using System;
using UnityEngine;

namespace AssemblyCSharp
{
	public class Trees
	{
		public float treesNum = 1.0f;

		private Terrian terrian;
		private Planet planet;

		public Trees(Planet planet, Terrian terrian) {
			this.planet = planet;
			this.terrian = terrian;
		}

		public void Generate() {
			var g1 = new Noise ();
			g1.frequency = 0.05f;
			var g2 = new Noise ();
			g2.frequency = g1.frequency * 2;
			var g3 = new Noise ();
			g3.frequency = 2.0f;

			foreach (var surface in terrian.AllSurfaces.Values) {
				var coord = surface.coord;
				var block = terrian.GetVoxel (coord.x, coord.y, coord.z);

				if (block.type == TerrianBlockType.Water) {
					continue;
				}

				var noise = 
					g1.get (coord.x, coord.y, coord.z) +
					g2.get (coord.x, coord.y, coord.z) * 0.5f;
				var noise2 = g3.get(coord.x, coord.y, coord.z);

				noise /= 1.5f;

				var min1 = 0.3;
				var min2 = 0.5;

				if (block.type == TerrianBlockType.Stone) {
					noise2 -= 0.5f;
				}

				if (noise > min1 && 
					noise2 > min2 &&
					UnityEngine.Random.Range(0.0f, 1.0f) < treesNum) {
					var size = UnityEngine.Random.Range(0.1f, 1.0f);
					var uv = new Vector2 (
						UnityEngine.Random.Range (-0.3f, 0.3f), 
						UnityEngine.Random.Range (-0.3f, 0.3f));

					planet.Create (Prefabs.Objects.Trees.OfSize(size), surface, uv);
				}
			}
		}
	}
}

