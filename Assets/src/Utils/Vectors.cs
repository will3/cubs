using System;
using Cubiquity;
using System.Collections.Generic;
using UnityEngine;

namespace AssemblyCSharp
{
	public static class Vectors
	{
		public static IList<Vector3i> NeighbourCoords(this Vector3i coord) {
			return new [] {
				new Vector3i(coord.x - 1, coord.y, coord.z),
				new Vector3i(coord.x + 1, coord.y, coord.z),
				new Vector3i(coord.x, coord.y - 1, coord.z),
				new Vector3i(coord.x, coord.y + 1, coord.z),
				new Vector3i(coord.x, coord.y, coord.z - 1),
				new Vector3i(coord.x, coord.y, coord.z + 1),
			};
		}

		public static Vector3i NextCoord(this Vector3i coord, Dir dir) {
			return coord + DirUtils.GetUnitVector (dir);
		}
	}
}

