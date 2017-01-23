using System;
using UnityEngine;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	public class Campfire : MonoBehaviour
	{
		private Billboard billboard;
		private BlockComponent blockComponent;

		private IEnumerable<Surface> surfaces;

		void Start() {
			billboard = GetComponentInChildren<Billboard> ();
			Debug.Assert (billboard != null);
			blockComponent = GetComponentInChildren<BlockComponent> ();
			Debug.Assert (blockComponent != null);

			if (blockComponent.blockCoord.surface != null) {
				var planet = Game.Instance.Planet;
				surfaces = planet.Terrian.FindSurfaces (blockComponent.blockCoord.surface, 2.9f);
			}
		}

		void Update() {
			billboard.up = Game.Instance.Planet.gameObject.transform.TransformDirection (blockComponent.blockCoord.surface.normal);

			foreach (var surface in surfaces) {
				DebugUtil.DrawSurface (surface);
			}
		}
	}
}

