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
		public bool transparent;
		public bool isWater;

		// Up direction
		public int up;
	}
}

