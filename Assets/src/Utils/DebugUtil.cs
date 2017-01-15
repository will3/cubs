using System;
using UnityEngine;

namespace AssemblyCSharp
{
	public class Line {
		public readonly Vector3 a;
		public readonly Vector3 b;

		public Line(Vector3 a, Vector3 b) {
			this.a = a;
			this.b = b;
		}
	}

	public class DebugUtil
	{
		public static void DrawSurface(Surface surface) {
			var planet = Game.Instance.Planet;
			Debug.DrawLine (
				planet.transform.TransformPoint(surface.point),
				planet.transform.TransformPoint(surface.pointAbove),
				Color.red);
		}

		public static void DrawLine(Line line) {
			Debug.DrawLine (line.a, line.b, Color.red);	
		}
	}
}

