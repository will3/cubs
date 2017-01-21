using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class PlaceBuilding : MonoBehaviour {
	
	class Placement {
		public static Placement Prefab(string prefabName) {
			var placement = new Placement();
			placement.prefabName = prefabName;
			return placement;
		}

		public string prefabName;
	}

	private Placement placement;

	void Start () {
		
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.Q)) {
			placement = Placement.Prefab (Prefabs.Swordsman);
		} 

		if (Input.GetKeyDown (KeyCode.W)) {
			placement = Placement.Prefab (Prefabs.Spider);
		}

		if (Input.GetKeyDown (KeyCode.E)) {
			placement = Placement.Prefab (Prefabs.Archer);
		}

		if (Input.GetKeyDown (KeyCode.R)) {
			placement = Placement.Prefab (Prefabs.Objects.Trees.OfSize (0.5f));
		}

		if (Input.GetKeyDown (KeyCode.Mouse0)) {
			var planet = Game.Instance.Planet;
			var surface = planet.GetSurface ();

			if (placement != null && surface != null) {
				if (placement.prefabName != null) {
					planet.Create (placement.prefabName, surface);
				}
			}
		}
	}
}
