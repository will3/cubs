using System;
using UnityEngine;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	public class Mesher
	{
		public static Mesh Mesh(Chunk chunk) {

			var m = new Mesh ();
			var vertices = new List<Vector3> ();
			var uvs = new List<Vector2> ();
			var triangles = new List<int> ();

			for (var i = 0; i < chunk.size[0]; i++) {
				for (var j = 0; j < chunk.size [1]; j++) {
					for (var k = 0; k < chunk.size [2]; k++) {
						var a = chunk.Get (i, j, k);

						var b = chunk.Get (i + 1, j, k);

						if ((a != null && b == null) || (b != null && a == null)) {
							var v1 = new Vector3 (i + 1, j, k);
							var v2 = new Vector3 (i + 1, j + 1, k);
							var v3 = new Vector3 (i + 1, j + 1, k + 1);
							var v4 = new Vector3 (i + 1, j, k + 1);

							vertices.Add (v1);
							vertices.Add (v2);
							vertices.Add (v3);
							vertices.Add (v4);

							uvs.Add (new Vector2 (0, 0));
							uvs.Add (new Vector2 (1, 0));
							uvs.Add (new Vector2 (1, 1));
							uvs.Add (new Vector2 (0, 1));

							triangles.AddRange (new [] { 0, 1, 2, 0, 2, 3 });
						}
					}
				}
			}

			m.vertices = vertices.ToArray();
			m.uv = uvs.ToArray();
			m.triangles = triangles.ToArray();

			m.RecalculateNormals ();

			return m;
		}
	}
}

