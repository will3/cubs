using System;
using UnityEngine;
using System.Linq;

namespace AssemblyCSharp
{
	public class ChunkMesh : MonoBehaviour
	{
		public int tileRows = 8;
		public int tilePixelSize = 8;
		public Material material;
		public Material transparentMaterial;
		private Chunks chunks;
		private Chunks _waterChunks;

		public Chunks waterChunks {
			get {
				return _waterChunks;
			}
		}

		public void Set(int i, int j, int k, Voxel v) {
			if (v.transparent) {
				waterChunks.Set (i, j, k, v);
			} else {
				chunks.Set (i, j, k, v);
			}
		}

		public Voxel Get(int i, int j, int k) {
			return chunks.Get (i, j, k) ?? waterChunks.Get (i, j, k);
		}

		public void UpdateMesh() {
			chunks.UpdateMesh ();
			waterChunks.UpdateMesh ();
		}

		void Start() {
			chunks = new Chunks (gameObject, material, tileRows, tilePixelSize);
			_waterChunks = new Chunks (gameObject, transparentMaterial, tileRows, tilePixelSize);
		}

		void Update() {
			UpdateMesh ();	
		}
	}
}

