using System;
using Cubiquity;
using System.Collections.Generic;
using UnityEngine;

namespace AssemblyCSharp
{
	public class Voxel
	{
		public Vector3i coord;
		public int[] textureIds; // Left, right, down, top, back forward

		public Voxel(Vector3i coord, int textureId) {
			this.coord = coord;
			this.textureIds = new [] { textureId, textureId, textureId, textureId, textureId, textureId };
		}
	}
}

