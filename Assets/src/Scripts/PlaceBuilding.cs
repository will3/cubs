using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class PlaceBuilding : MonoBehaviour {
	private string placement;

	void Start () {
		
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.Q)) {
			placement = Prefabs.Swordsman;
		} 

		if (Input.GetKeyDown (KeyCode.W)) {
			placement = Prefabs.Spider;
		}

		if (Input.GetKeyDown (KeyCode.E)) {
			placement = Prefabs.Archer;
		}

		if (Input.GetKeyDown (KeyCode.R)) {
			placement = Prefabs.Objects.Trees.OfSize (0.5f);
		}

		if (Input.GetKeyDown (KeyCode.T)) {
			placement = Prefabs.Objects.EvilGate;
		}

		if (Input.GetKeyDown(KeyCode.Y)) {
			placement = Prefabs.Objects.Campfire;
		}

		if (Input.GetKeyDown (KeyCode.Mouse0)) {
			var planet = Game.Instance.Planet;
			var surface = planet.GetSurface ();

			if (placement != null && surface != null) {
				if (placement != null) {
					planet.Create (placement, surface);
				}
			}
		}
	}
}
