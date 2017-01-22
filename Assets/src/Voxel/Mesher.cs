using System;
using UnityEngine;
using System.Collections.Generic;
using Cubiquity;

namespace AssemblyCSharp
{
	public class Vertice {
		public Vector3 vector;
		public int index;
		public int f;
		public Vector3i coord;
		public Vector2 uv;

		// Used by water
		public Mesh mesh;

		public Vertice(Vector3 vector, int index, int f, Vector3i coord, Vector2 uv) {
			this.vector = vector;
			this.index = index;
			this.f = f;
			this.coord = coord;
			this.uv = uv;
		}
	}

	public class Mesher
	{
		public static Mesh Mesh(Chunk chunk, Chunks chunks, int tileRows, float tileSize, List<Vertice> verticeList = null) {
			var m = new Mesh ();
			var vertices = new List<Vector3> ();
			var uvs = new List<Vector2> ();
			var triangles = new List<int> ();
			var normals = new List<Vector3> ();

			var coord = new [] { 0, 0, 0 };
			var offset = new Vector3 (0.5f, 0.5f, 0.5f);
			int index;
			int textureId;
			int f;
			Vector2 uvOffset;

			for (var d = 0; d < 3; d++) {
				var u = (d + 1) % 3;
				var v = (d + 2) % 3;
				var normalCoord = new [] { 0, 0, 0 };
				normalCoord [d] += 1;
				var normal = new Vector3 (normalCoord [0], normalCoord [1], normalCoord [2]);
				normalCoord [d] *= -1;
				var normal2 = new Vector3 (normalCoord [0], normalCoord [1], normalCoord [2]);

				for (var x = -1; x < chunk.size; x++) {
					for (var y = 0; y < chunk.size; y++) {
						for (var z = 0; z < chunk.size; z++) {
							coord [d] = x;
							coord [u] = y;
							coord [v] = z;

							var a = (x == -1) 
								? chunks.Get(coord[0] + chunk.origin[0], coord[1] + chunk.origin[1], coord[2] + chunk.origin[2])
								: chunk.Get (coord[0], coord[1], coord[2]);
							coord [d] += 1;

							var b = coord[d] == chunk.size 
								? chunks.Get(coord[0] + chunk.origin[0], coord[1] + chunk.origin[1], coord[2] + chunk.origin[2])
								: chunk.Get (coord[0], coord[1], coord[2]);

							if ((a != null && b == null) || (b != null && a == null)) {
								var v1 = new Vector3 (coord[0], coord[1], coord[2]) - offset;
								coord [u] += 1;
								var v2 = new Vector3 (coord[0], coord[1], coord[2]) - offset;
								coord [v] += 1;
								var v3 = new Vector3 (coord[0], coord[1], coord[2]) - offset;
								coord [u] -= 1;
								var v4 = new Vector3 (coord[0], coord[1], coord[2]) - offset;

								index = vertices.Count;

								var n = a != null ? normal : normal2;

								normals.Add (n);
								normals.Add (n);
								normals.Add (n);
								normals.Add (n);

								vertices.Add (v1);
								vertices.Add (v2);
								vertices.Add (v3);
								vertices.Add (v4);

								f = d * 2 + ((a != null) ? 1 : 0);									
								textureId = (a ?? b).textureIds[f];
								uvOffset=  Tilesets.GetOffset (textureId, tileRows);
				
								var uv1 = new Vector2 (uvOffset.x, uvOffset.y);
								var uv2 = new Vector2 (uvOffset.x + tileSize, uvOffset.y);
								var uv3 = new Vector2 (uvOffset.x + tileSize, uvOffset.y + tileSize);
								var uv4 = new Vector2 (uvOffset.x, uvOffset.y + tileSize);
								
								uvs.Add (uv1);
								uvs.Add (uv2);
								uvs.Add (uv3);
								uvs.Add (uv4);

								if (verticeList != null) {
									var c = a != null ? a.coord : b.coord;
									verticeList.Add (
										new Vertice (v1, index, f, c, new Vector2 (0, 0)));
									verticeList.Add (
										new Vertice (v2, index + 1, f, c, new Vector2 (1, 0)));
									verticeList.Add (
										new Vertice (v3, index + 2, f, c, new Vector2 (1, 1)));
									verticeList.Add (
										new Vertice (v4, index + 3, f, c, new Vector2 (0, 1)));
								}

								if (a != null) {
									triangles.AddRange (new [] { 0 + index, 1 + index, 2 + index, 0 + index, 2 + index, 3 + index });
								} else {
									triangles.AddRange (new [] { 0 + index, 2 + index, 1 + index, 0 + index, 3 + index, 2 + index });
								}
							}		
						}
					}
				}
			}

			m.vertices = vertices.ToArray();
			m.uv = uvs.ToArray();
			m.triangles = triangles.ToArray();
			m.normals = normals.ToArray ();
	
			return m;
		}
	}
}

