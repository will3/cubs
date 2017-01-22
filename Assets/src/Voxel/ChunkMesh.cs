using System;
using UnityEngine;

namespace AssemblyCSharp
{
	public class ChunkMesh : MonoBehaviour
	{
		public int tileRows = 8;
		public int tilePixelSize = 8;
		public Material material;
		public Material transparentMaterial;
		private Chunks chunks;
		private Chunks transparentChunks;

		public void Set(int i, int j, int k, Voxel v) {
			if (v.transparent) {
				transparentChunks.Set (i, j, k, v);
			} else {
				chunks.Set (i, j, k, v);
			}
		}

		public Voxel Get(int i, int j, int k) {
			return chunks.Get (i, j, k) ?? transparentChunks.Get (i, j, k);
		}

		void Start() {
			chunks = new Chunks (gameObject, material, tileRows, tilePixelSize);
			transparentChunks = new Chunks (gameObject, transparentMaterial, tileRows, tilePixelSize);
		}

		void Update() {
			chunks.UpdateMesh ();
			transparentChunks.UpdateMesh ();
		}
	}
}

