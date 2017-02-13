using System;
using UnityEngine;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	public class Campfire : MonoBehaviour
	{
		private Billboard billboard;
		private BlockComponent blockComponent;
		List<Surface> surfaces;

		void Start() {
			billboard = GetComponentInChildren<Billboard> ();
			Debug.Assert (billboard != null);
			blockComponent = GetComponentInChildren<BlockComponent> ();
			Debug.Assert (blockComponent != null);

			if (blockComponent.surface != null) {
				var planet = Game.Instance.Planet;
				surfaces = new List<Surface> (planet.Terrian.FindSurfaces (blockComponent.surface, 5.0f));

				var num = 7;
				for (var i = 0; i < num; i++) {
					if (surfaces.Count == 0) {
						break;
					}
					var index = UnityEngine.Random.Range (0, surfaces.Count - 1);
					var surface = surfaces [index];
					surfaces.RemoveAt (index);

					planet.Create (Prefabs.Swordsman, surface);
				}
			}
		}

		void Update() {
			billboard.up = Game.Instance.Planet.gameObject.transform.TransformDirection (blockComponent.surface.normal);
		}
	}
}

