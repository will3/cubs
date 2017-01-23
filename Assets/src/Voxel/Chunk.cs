using System;
using Cubiquity;
using UnityEngine;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	public class ChunkObject {
		public IList<Vertice> vertices;
		public GameObject obj;
		public Mesh mesh;
	}

	public class Chunk
	{
		public bool dirty;
		public ChunkObject obj = new ChunkObject();
		public ChunkObject transparentObj = new ChunkObject();

		public int[] _origin;
		public string id;

		public int[] origin {
			get {
				return _origin;
			}
			set {
				_origin = value;
				id = _origin [0] + "," + _origin [1] + "," + _origin [2];
			}
		}

		public readonly int size;
		int yz;
		int z;
		Voxel[] data;

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

