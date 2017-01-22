using System;
using UnityEngine;

namespace AssemblyCSharp
{
	public class Campfire : MonoBehaviour
	{
		private Billboard billboard;
		private BlockComponent blockComponent;

		void Start() {
			billboard = GetComponentInChildren<Billboard> ();
			Debug.Assert (billboard != null);
			blockComponent = GetComponentInChildren<BlockComponent> ();
			Debug.Assert (blockComponent != null);
		}

		void Update() {
			billboard.up = Game.Instance.Planet.gameObject.transform.TransformDirection (blockComponent.blockCoord.surface.normal);
		}
	}
}

