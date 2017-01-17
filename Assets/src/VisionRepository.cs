using System;
using UnityEngine;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	class VisibleSurfacesResult {
		internal readonly IList<Surface> surfaces;

		VisibleSurfacesResult(IList<Surface> surfaces) {
			this.surfaces = surfaces;
		}
	}

	public class VisionRepository
	{
		private Dictionary<string, VisibleSurfacesResult> map = new Dictionary<string, VisibleSurfacesResult>();

		public VisionRepository () { }

		public IList<Surface> GetVisibleSurfaces(Surface surface) {
			if (map.ContainsKey (surface.identifier)) {
				return map [surface.identifier].surfaces;
			}

			map [surface.identifier] = CalculateVisibleSurfaces (surface);
		}

		private IList<Surface> CalculateVisibleSurfaces(Surface fromSurface) {
			var planet = Game.Instance.Planet;

			var point1 = planet.transform.TransformPoint (fromSurface.pointAbove);

			var surfaces = new List<Surface> ();

			foreach (var kv in planet.Terrian.AllSurfaces) {
				var surface = kv.Value;

				var point2 = planet.transform.TransformPoint (surface.pointAbove);

				var ray = new Ray (point1, (point2 - point1).normalized);

				if (Physics.Raycast (ray, Vector3.Distance(point2, point1))) {
					continue;
				}

				surfaces.Add (surface);
			}
				
			// Sort by distance
			surfaces.Sort ((x, y) => {
				return (int)(x.DistanceTo(fromSurface) - y.DistanceTo(fromSurface));
			});
				
			return new VisibleSurfacesResult (surfaces);
		}
	}
}

