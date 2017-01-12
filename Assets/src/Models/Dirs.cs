using System;
using Cubiquity;

namespace AssemblyCSharp
{
	public enum Dir {
		None = 0,
		Up = 1,
		Down = 2,
		Left = 4,
		Right = 8,
		Forward = 16,
		Back = 32
	}

	public class DirUtils 
	{
		private static Vector3i up = new Vector3i(0, 1, 0);
		private static Vector3i down = new Vector3i(0, -1, 0);
		private static Vector3i left = new Vector3i(-1, 0, 0);
		private static Vector3i right = new Vector3i(1, 0, 0);
		private static Vector3i forward = new Vector3i(0, 0, 1);
		private static Vector3i back = new Vector3i(0, 0, -1);
		private static Vector3i zero = new Vector3i (0, 0, 0);

		public static Vector3i GetUnitVector(Dir dir) {
			switch (dir) {
			case Dir.Up:
				return up;
			case Dir.Down:
				return down;
			case Dir.Left:
				return left;
			case Dir.Right:
				return right;
			case Dir.Forward:
				return forward;
			case Dir.Back:
				return back;
			}
			return zero;
		}

		public static readonly Dir[] Dirs = new [] { Dir.Up, Dir.Down, Dir.Left, Dir.Right, Dir.Forward, Dir.Back };
	}
}

