using System;
using System.Collections.Generic;
using UnityEngine;

namespace AssemblyCSharp
{
	public class Chunks : MonoBehaviour
	{
		private float size = 16.0f;
		private int size_i = 16;
		public readonly Dictionary<string, Chunk> chunks = new Dictionary<string, Chunk>();

		public Material material;
		public Material transparentMaterial;

		public int tileRows = 8;
		public int tilePixelSize = 8;

		public bool hasCollider = true;

		public Voxel Get(int i, int j, int k) {
			var origin = GetOrigin (i, j, k);
			var key = origin [0] + "," + origin [1] + "," + origin [2];

			if (!chunks.ContainsKey (key)) {
				return null;
			}

			return  chunks [key].Get (i - origin[0], j - origin[1], k - origin[2]);
		}

		public bool Has(int i, int j, int k) {
			return Get (i, j, k) != null;
		}

		public void Set(int i, int j, int k, Voxel v) {
			var origin = GetOrigin (i, j, k);
			var key = origin [0] + "," + origin [1] + "," + origin [2];

			if (!chunks.ContainsKey (key)) {
				chunks [key] = new Chunk (size_i);
				chunks [key].origin = origin;
			}

			chunks [key].Set (i - origin [0], j - origin [1], k - origin [2], v);
			chunks [key].dirty = true;
		}

		private int[] GetOrigin(int i, int j, int k) {
			return new [] { 
				(int)(Math.Floor(i / size) * size),
				(int)(Math.Floor(j / size) * size),
				(int)(Math.Floor(k / size) * size)
			};
		}

		void Update() {
			UpdateMesh ();
		}

		public void UpdateMesh() {
			foreach (var chunk in chunks.Values) {
				if (chunk.dirty) {
					UpdateChunkMesh (chunk, false);
					UpdateChunkMesh (chunk, true);
					chunk.dirty = false;
				}
			}
		}

		private void UpdateChunkMesh(Chunk chunk, bool transparent) {
			var chunkObject = transparent ? chunk.transparentObj : chunk.obj;
			if (chunkObject.obj == null) {
				var name = transparent ? "trans_" + chunk.id : "mesh_" + chunk.id;
				var components = hasCollider ? 
					new [] { typeof(MeshRenderer), typeof(MeshFilter), typeof(MeshCollider) } :
					new [] { typeof(MeshRenderer), typeof(MeshFilter) };
				
				var obj = new GameObject(name, components);

				obj.GetComponent<MeshRenderer> ().material = transparent ? transparentMaterial : material;
				obj.transform.parent = gameObject.transform;
				obj.transform.localPosition = new Vector3 (chunk.origin [0], chunk.origin [1], chunk.origin [2]);
				chunkObject.obj = obj;
			}

			var prevMesh = chunkObject.obj.GetComponent<MeshFilter> ().sharedMesh;
			if (prevMesh != null) {
				Destroy (prevMesh);
			}
				
			var tileSize = Tilesets.GetTileSize (tileRows, tilePixelSize);
			var vertices = new List<Vertice> ();
			var m = Mesher.Mesh (chunk, this, tileRows, tileSize, vertices, transparent);
			chunkObject.vertices = vertices;

			chunkObject.obj.GetComponent<MeshFilter> ().sharedMesh = m;
			if (hasCollider) {
				chunkObject.obj.GetComponent<MeshCollider> ().sharedMesh = m;
			}
		}
	}
}

