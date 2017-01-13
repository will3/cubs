using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class PlaceBuilding : MonoBehaviour {

	enum BuildingType {
		None,
		Spawner,
		Turrent
	}

	private BuildingType buildingType = BuildingType.None;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Q)) {
			buildingType = BuildingType.Spawner;	
		} 

		if (Input.GetKeyDown (KeyCode.W)) {
			buildingType = BuildingType.Turrent;	
		}

		if (Input.GetKeyDown (KeyCode.Mouse0)) {
			var planet = Game.Instance.Planet;
			var surface = planet.GetSurface ();

			if (buildingType != BuildingType.None && surface != null) {
				var name = getResourceName (buildingType);
				var obj = planet.Create (name, surface);

				switch (buildingType) {
				case BuildingType.Spawner:
					var spawner = obj.GetComponent<Spawner> ();
					spawner.surface = surface;
					break;
				case BuildingType.Turrent:
					//			var turrent = obj.GetComponent<Turrent> ();	
					break;
				}

			}
		}
	}

	private string getResourceName(BuildingType buildingType) {
		switch (buildingType) {
		case BuildingType.Spawner:
			return "Spawner";
		case BuildingType.Turrent:
			return "Turrent";
		}
		return null;
	}
}
