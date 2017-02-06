using System;
using Cubiquity;
using UnityEngine;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	public class Slope : IVoxelObject
	{
		public Mesh Mesh() {
			//  5 
			// 4   
			//	2   3
			// 0   1
			var a = new Vector3 (0, 0, 0);
			var b = new Vector3 (1, 0, 0);
			var c = new Vector3 (0, 0, 1);
			var d = new Vector3 (1, 0, 1);
			var e = new Vector3 (0, 1, 0);
			var f = new Vector3 (0, 1, 1);

			var vertices = new [] { a, b, c, d, e, f };

			var triangles = new [] { 
				0, 1, 3,
				0, 3, 2,
				5, 4, 0,
				5, 0, 2,
				4, 1, 0,
				5, 2, 3,
				4, 5, 3,
				4, 3, 1
			};

			var uvs = new [] { 
				new Vector2(0, 0),
				new Vector2(1, 0),
				new Vector2(0, 1),
				new Vector2(1, 1),
				new Vector2(1, 0),
				new Vector2(1, 1)
			};

			var mesh = createFaceNormalMesh (vertices, triangles, uvs);
			return mesh;
		}

		public static Mesh createFaceNormalMesh(IList<Vector3> vectors, IList<int> triangles, IList<Vector2> uvList) {
			var mesh = new Mesh ();

			var vs = new List<Vector3> ();
			var ts = new List<int> ();
			var ns = new List<Vector3> ();
			var uvs = new List<Vector2>();

			for (var i = 0; i < triangles.Count; i+=3) {
				var a = triangles [i];
				var b = triangles [i + 1];
				var c = triangles [i + 2];

				var v1 = vectors [a];
				var v2 = vectors [b];
				var v3 = vectors [c];

				var normal = Vector3.Cross (v3 - v1, v2 - v1);

				var index = ts.Count;

				vs.Add (vectors[a]);
				vs.Add (vectors[b]);
				vs.Add (vectors[c]);

				ts.Add (index);
				ts.Add (index + 1);
				ts.Add (index + 2);

				ns.Add (normal);
				ns.Add (normal);
				ns.Add (normal);

				uvs.Add (uvList [a]);
				uvs.Add (uvList [b]);
				uvs.Add (uvList [c]);
			}

			mesh.vertices = vs.ToArray ();
			mesh.triangles = ts.ToArray ();
			mesh.normals = ns.ToArray ();
			mesh.uv = uvs.ToArray ();

			return mesh;
		}
	}
}

