using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class PlaceBuilding : MonoBehaviour {
	private Placement placement;
	private bool buildingMode = false;

	public void StartPlacement(Placement placement) {
		this.placement = placement;
		buildingMode = true;
	}

	void Start () {
		Game.Instance.placeBuilding = this;
	}
		
	void Update () {
		if (Input.GetKeyDown (KeyCode.Mouse0)) {
			HandlePlacement ();
		}

		if (Input.GetKeyDown (KeyCode.Escape)) {
			this.placement = null;
			buildingMode = false;
		}
	}

	void HandlePlacement() {
		var planet = Game.Instance.Planet;
		if (planet == null) {
			return;
		}

		if (placement == null) {
			return;
		}
			
		if (placement.prefabName != null) {
			var surface = planet.GetSurface ();
			if (surface != null) {
				planet.Create (placement.prefabName, surface);
			}
		}

		if (placement.type.HasValue) {
			var coord = planet.GetCoord ();
			if (coord.HasValue) {
				var block = new TerrianBlock (coord.Value, TerrianBlockType.WireframeBlue);
//				var block = new TerrianBlock (coord.Value, placement.type.Value);
				planet.AddBlock (coord.Value, block);
			}
		}
	}
}
