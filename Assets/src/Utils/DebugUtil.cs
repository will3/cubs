using System;
using UnityEngine;
using Dijkstras;

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

		public static void DrawPath(Surface start, Path path) {
			if (path.path.Count == 0) {
				return;
			}

			var planet = Game.Instance.Planet;

			for (var i = -1; i < path.path.Count - 1; i++) {
				var a =  i == -1 ? 
					start :
					planet.Terrian.GetSurface (path.path [i]);
				var b = planet.Terrian.GetSurface (path.path [i + 1]);
				Debug.DrawLine (
					planet.transform.TransformPoint(a.pointAbove),
					planet.transform.TransformPoint(b.pointAbove),
					Color.red
				);
			}
		}

		public static void DrawPoint(Vector3 point, float radius = 0.1f) {
			Gizmos.color = Color.red;
			Gizmos.DrawSphere (point, radius);
		}
	}
}

