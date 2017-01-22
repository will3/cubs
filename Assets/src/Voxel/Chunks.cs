using System;
using System.Collections.Generic;
using UnityEngine;

namespace AssemblyCSharp
{
	public class Chunks : MonoBehaviour
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

		void Update() {
			UpdateMesh ();
		}

		public void UpdateMesh() {
			foreach (var chunk in chunks.Values) {
				if (chunk.dirty) {
					UpdateChunkMesh (chunk);
					chunk.dirty = false;
				}
			}
		}

		private void UpdateChunkMesh(Chunk chunk) {
			if (chunk.obj != null) {
				GameObject.Destroy (chunk.obj);
			}

			var m = Mesher.Mesh (chunk);

			var obj = new GameObject("chunk_mesh", new [] { 
				typeof(MeshRenderer), 
				typeof(MeshFilter), 
				typeof(MeshCollider) });
			obj.GetComponent<MeshFilter>().mesh = m;

			obj.transform.parent = gameObject.transform;
		
			chunk.obj = obj;
		}
	}
}

