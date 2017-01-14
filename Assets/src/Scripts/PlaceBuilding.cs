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
			prefabType = BlockPrefabType.Spawner;
		} 

		if (Input.GetKeyDown (KeyCode.W)) {
			prefabType = BlockPrefabType.Turrent;
		}

		if (Input.GetKeyDown (KeyCode.E)) {
			prefabType = BlockPrefabType.Critter;
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
