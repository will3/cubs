using System;
using UnityEngine;
using Cubiquity;

namespace AssemblyCSharp
{
	public class Surface {
		public readonly Dir dir;
		public readonly Vector3i coord;
		public readonly Vector3i coordAbove;
		public readonly Vector3 normal;
		public readonly string identifier;

		public readonly Vector3 point;
		public readonly Vector3 pointAbove;

		public readonly Quaternion rotation;

		public Block block;

		public bool hasObject { get { return block != null; } }

		public Surface(Vector3i coord, Dir dir) {
			this.coord = coord;
			this.dir = dir;
			var vector = DirUtils.GetUnitVector (dir);
			normal = new Vector3(vector.x, vector.y, vector.z);
			coordAbove = coord + vector;
			identifier = coord.x + "," + coord.y + "," + coord.z + "," + dir;

			point = new Vector3 (coord.x, coord.y, coord.z) + normal * 0.5f;
			pointAbove = new Vector3 (coordAbove.x, coordAbove.y, coordAbove.z);

			rotation = DirUtils.GetRotation (dir);
		}

		public float DistanceTo(Surface surface2) {
			return Vector3.Distance (pointAbove, surface2.pointAbove);
		}
	}
}

