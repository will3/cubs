using System;
using Cubiquity;
using System.Collections.Generic;
using UnityEngine;

namespace AssemblyCSharp
{
	public class Voxel
	{
		public readonly Vector3i coord;
		public readonly int[] textureIds; // Left, right, down, top, back forward
		public readonly bool transparent;

		public Voxel(Vector3i coord, int textureId, bool transparent) {
			this.coord = coord;
			this.textureIds = new [] { textureId, textureId, textureId, textureId, textureId, textureId };
			this.transparent = transparent;
		}
	}
}

