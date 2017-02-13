using System;
using UnityEngine;
using System.Collections.Generic;
using Cubiquity;

namespace AssemblyCSharp
{
	public class Vertice {
		public readonly Vector3 position;
		public readonly int index;
		public readonly int f;
		public readonly Vector3i coord;
		public readonly Vector2 uv;
		public readonly string chunkId;

		// Used by water
		public Mesh mesh;

		public Vertice(Vector3 vector, int index, int f, Vector3i coord, Vector2 uv, string chunkId) {
			this.position = vector;
			this.index = index;
			this.f = f;
			this.coord = coord;
			this.uv = uv;
			this.chunkId = chunkId;
		}
	}

	struct MCoord {
		public int i;
		public int j;
		public int k;
		public int d;

		public MCoord(int i, int j, int k, int d) {
			this.i = i;
			this.j = j;
			this.k = k;
			this.d = d;
		}

		public Vector3i vector {
			get {
				var coord = new [] { 0, 0, 0 };

				coord [d] = i;
				coord [(d + 1) % 3] = j;
				coord [(d + 2) % 3] = k;

				return new Vector3i (coord [0], coord [1], coord [2]);
			}
		}

		public MCoord add (int di, int dj, int dk){
			return new MCoord (i + di, j + dj, k + dk, d);
		}
	}

	public class Mesher
	{
		public static Mesh Mesh(Chunk chunk, Chunks chunks, int tileRows, float tileSize, List<Vertice> verticeList, bool transparent) {
			var m = new Mesh ();
			var vertices = new List<Vector3> ();
			var uvs = new List<Vector2> ();
			var uv2s = new List<Vector2> ();
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
								? chunks.Get (coord [0] + chunk.origin [0], coord [1] + chunk.origin [1], coord [2] + chunk.origin [2])
								: chunk.Get (coord [0], coord [1], coord [2]);
							coord [d] += 1;

							var b = coord [d] == chunk.size 
								? chunks.Get (coord [0] + chunk.origin [0], coord [1] + chunk.origin [1], coord [2] + chunk.origin [2])
								: chunk.Get (coord [0], coord [1], coord [2]);

							if (x == -1 && b == null) {
								continue;
							} else if (coord [d] == chunk.size && a == null) {
								continue;
							}

							var front = false;
							var back = false;
							
							if (transparent) {
								front = a != null && a.transparent && b == null;
								back = b != null && b.transparent && a == null;
							} else {
								front = a != null && !a.transparent && (b == null || b.transparent);
								back = b != null && !b.transparent && (a == null || a.transparent);
							} 

							if (!front && !back) {
								continue;
							}

							var v1 = new Vector3 (coord[0], coord[1], coord[2]) - offset;
							coord [u] += 1;
							var v2 = new Vector3 (coord[0], coord[1], coord[2]) - offset;
							coord [v] += 1;
							var v3 = new Vector3 (coord[0], coord[1], coord[2]) - offset;
							coord [u] -= 1;
							var v4 = new Vector3 (coord[0], coord[1], coord[2]) - offset;

							index = vertices.Count;

							var n = front ? normal : normal2;

							normals.Add (n);
							normals.Add (n);
							normals.Add (n);
							normals.Add (n);

							vertices.Add (v1);
							vertices.Add (v2);
							vertices.Add (v3);
							vertices.Add (v4);

							f = d * 2 + (front ? 1 : 0);									
							var voxel = front ? a : b;
							textureId = voxel.textureIds[f];
							uvOffset=  Tilesets.GetOffset (textureId, tileRows);

							var up = voxel.up;
							var upD = (int)(up / 2);

							Vector2 uv1, uv2, uv3, uv4;

							if (u * 2 == up) {
								uv4 = new Vector2 (uvOffset.x, uvOffset.y);
								uv1 = new Vector2 (uvOffset.x + tileSize, uvOffset.y);
								uv2 = new Vector2 (uvOffset.x + tileSize, uvOffset.y - tileSize);
								uv3 = new Vector2 (uvOffset.x, uvOffset.y - tileSize);
							} else if (u * 2 + 1 == up) {
								uv2 = new Vector2 (uvOffset.x, uvOffset.y);
								uv3 = new Vector2 (uvOffset.x + tileSize, uvOffset.y);
								uv4 = new Vector2 (uvOffset.x + tileSize, uvOffset.y - tileSize);
								uv1 = new Vector2 (uvOffset.x, uvOffset.y - tileSize);
							} else if (v * 2 == up) {
								uv1 = new Vector2 (uvOffset.x, uvOffset.y);
								uv2 = new Vector2 (uvOffset.x + tileSize, uvOffset.y);
								uv3 = new Vector2 (uvOffset.x + tileSize, uvOffset.y - tileSize);
								uv4 = new Vector2 (uvOffset.x, uvOffset.y - tileSize);
							} else if (v * 2 + 1 == up) {								
								uv1 = new Vector2 (uvOffset.x, uvOffset.y - tileSize);
								uv2 = new Vector2 (uvOffset.x + tileSize, uvOffset.y - tileSize);
								uv3 = new Vector2 (uvOffset.x + tileSize, uvOffset.y);
								uv4 = new Vector2 (uvOffset.x, uvOffset.y);
							} else {
								uv1 = new Vector2 (uvOffset.x, uvOffset.y);
								uv2 = new Vector2 (uvOffset.x + tileSize, uvOffset.y);
								uv3 = new Vector2 (uvOffset.x + tileSize, uvOffset.y - tileSize);
								uv4 = new Vector2 (uvOffset.x, uvOffset.y - tileSize);
							}

							uvs.Add (uv1);
							uvs.Add (uv2);
							uvs.Add (uv3);
							uvs.Add (uv4);

							if (verticeList != null) {
								var c = front ? a.coord : b.coord;
								verticeList.Add (
									new Vertice (v1, index, f, c, new Vector2 (0, 0), chunk.id));
								verticeList.Add (
									new Vertice (v2, index + 1, f, c, new Vector2 (1, 0), chunk.id));
								verticeList.Add (
									new Vertice (v3, index + 2, f, c, new Vector2 (1, 1), chunk.id));
								verticeList.Add (
									new Vertice (v4, index + 3, f, c, new Vector2 (0, 1), chunk.id));
							}

							if (front) {
								triangles.AddRange (new [] { 0 + index, 1 + index, 2 + index, 0 + index, 2 + index, 3 + index });
							} else {
								triangles.AddRange (new [] { 0 + index, 2 + index, 1 + index, 0 + index, 3 + index, 2 + index });
							}

							var mCoord = new MCoord (front ? x + 1 : x, y - 1, z - 1, d);

							var s00 = getV(chunks, mCoord.add (0, 0, 0), chunk.origin);
							var s01 = getV(chunks, mCoord.add (0, 1, 0), chunk.origin);
							var s02 = getV(chunks, mCoord.add (0, 2, 0), chunk.origin);
							var s10 = getV(chunks, mCoord.add (0, 0, 1), chunk.origin);
//							var s11 = getV(chunks, mCoord.add (0, 1, 1), chunk.origin);
							var s12 = getV(chunks, mCoord.add (0, 2, 1), chunk.origin);
							var s20 = getV(chunks, mCoord.add (0, 0, 2), chunk.origin);
							var s21 = getV(chunks, mCoord.add (0, 1, 2), chunk.origin);
							var s22 = getV(chunks, mCoord.add (0, 2, 2), chunk.origin);

							uv2s.Add (new Vector2 (vertexAO (s10, s01, s00), 0));
							uv2s.Add (new Vector2 (vertexAO (s01, s12, s02), 0));
							uv2s.Add (new Vector2 (vertexAO (s12, s21, s22), 0));
							uv2s.Add (new Vector2 (vertexAO (s21, s10, s20), 0));
						}		
					}
				}
			}

			m.vertices = vertices.ToArray();
			m.uv = uvs.ToArray();
			m.uv2 = uv2s.ToArray ();
			m.triangles = triangles.ToArray();
			m.normals = normals.ToArray ();
	
			return m;
		}

		private static int getV(Chunks chunks, MCoord mCoord, int[] origin) {
			var vector = mCoord.vector;
			return chunks.Get(vector.x + origin[0], vector.y + origin[1], vector.z + origin[2]) != null ? 1 : 0;
		}

		private static float vertexAO(int s1, int s2, int c) {
			if (s1 > 0 && s2 > 0) {
				return 1 / 4.0f;
			}
			return (3 - (s1 + s2 + c)) / 4.0f;
		}
	}
}

