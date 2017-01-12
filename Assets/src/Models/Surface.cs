using System;
using UnityEngine;
using Cubiquity;

namespace AssemblyCSharp
{
	public class Surface {
		public readonly Dir dir;
		public readonly Vector3 center;
		public readonly Vector3 pointAbove;

		public Surface(Vector3i coord, Dir dir) {
			this.dir = dir;
			var vector = DirUtils.GetUnitVector (dir);
			center = new Vector3 (
				coord.x + vector.x * 0.5f, 
				coord.y + vector.y * 0.5f, 
				coord.z + vector.z * 0.5f);

			pointAbove = center + new Vector3 (vector.x, vector.y, vector.z);
		}
	}
}

