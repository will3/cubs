using System;
using UnityEngine;
using System.Linq;

namespace AssemblyCSharp
{
	public class EvilGate : MonoBehaviour
	{
		private BlockComponent blockComponent;

		private Billboard billboard;

		private Cooldown spawnCooldown = new Cooldown(2.0f);

		void Start() {
			blockComponent = GetComponent<BlockComponent> ();
			Debug.Assert (blockComponent != null);
			billboard = GetComponentInChildren<Billboard> ();
			Debug.Assert (billboard != null);
		}

		void Update () {
			spawnCooldown.Update ();
			billboard.up = Game.Instance.Planet.gameObject.transform.TransformDirection (blockComponent.blockCoord.surface.normal);

			var planet = Game.Instance.Planet;

			var next = blockComponent.blockCoord.surface.RandomConnectedSurfaceIdentifier;

			if (next != null) {
				var nextSurface = planet.Terrian.GetSurface (next);
				if (spawnCooldown.Ready ()) {
					planet.Create (Prefabs.Spider, nextSurface);
				}
			}
		}
	}
}

