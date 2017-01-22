using System;
using Cubiquity;
using UnityEngine;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	public class Chunk
	{
		public bool dirty;
		public GameObject obj;
		public IList<Vertice> vertices;

		public int[] origin;
		public readonly int size;
		int yz;
		int z;
		Voxel[] data;

		public Mesh mesh {
			get {
				if (obj == null) {
					return null;
				}
				var meshFilter = obj.GetComponent<MeshFilter> ();
				return meshFilter.sharedMesh;
			}
		}

		public Chunk (int size)
		{
			this.size = size;
			this.yz = this.size * this.size;
			this.z = this.size;
			// TODO fix with size 
			this.data = new Voxel[this.size * this.size * this.size];
		}

		public void Set(int i, int j, int k, Voxel v) {
			var index = i * yz + j * z + k;
			this.data [index] = v;
		}

		public Voxel Get(int i, int j, int k) {
			var index = i * yz + j * z + k;
			return this.data [index];
		}

		public Voxel Get(Vector3i coord) {
			return Get (coord.x, coord.y, coord.z);
		}
	}
}

