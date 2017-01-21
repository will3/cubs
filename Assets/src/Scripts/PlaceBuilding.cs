using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class PlaceBuilding : MonoBehaviour {
	private string prefabType;

	void Start () {
		
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.Q)) {
			prefabType = Prefabs.Swordsman;
		} 

		if (Input.GetKeyDown (KeyCode.W)) {
			prefabType = Prefabs.Spider;
		}

		if (Input.GetKeyDown (KeyCode.E)) {
			prefabType = Prefabs.Archer;
		}

		if (Input.GetKeyDown (KeyCode.R)) {
			prefabType = Prefabs.Objects.Trees.OfSize(0.5f);
		}

		if (Input.GetKeyDown (KeyCode.Mouse0)) {
			var planet = Game.Instance.Planet;
			var surface = planet.GetSurface ();

			if (prefabType != null && surface != null) {
				planet.Create (prefabType, surface);
			}
		}
	}
}
