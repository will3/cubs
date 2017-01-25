using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class PlaceBuilding : MonoBehaviour {

	public class Placement
	{
		public string prefabName;
		public TerrianBlockType? type;

		public static Placement Prefab(string name) {
			var placement = new Placement ();
			placement.prefabName = name;
			return placement;
		}

		public static Placement Block(TerrianBlockType type) {
			var placement = new Placement ();
			placement.type = type;
			return placement;
		}
	}

	private Placement placement;

	void Start () {
		
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			placement = Placement.Prefab (Prefabs.Swordsman);
		} 

		if (Input.GetKeyDown (KeyCode.Alpha2)) {
			placement = Placement.Prefab (Prefabs.Spider);
		}

		if (Input.GetKeyDown (KeyCode.Alpha3)) {
			placement = Placement.Prefab (Prefabs.Archer);
		}

		if (Input.GetKeyDown (KeyCode.Alpha4)) {
			placement = Placement.Prefab (Prefabs.Objects.Trees.OfSize (Random.Range (0.0f, 1.0f)));
		}

		if (Input.GetKeyDown (KeyCode.Alpha5)) {
			placement = Placement.Prefab (Prefabs.Objects.EvilGate);
		}

		if (Input.GetKeyDown(KeyCode.Alpha6)) {
			placement = Placement.Prefab (Prefabs.Objects.Campfire);
		}

		if (Input.GetKeyDown (KeyCode.Alpha7)) {
			placement = Placement.Block (TerrianBlockType.StoneWall);
		}

		if (Input.GetKeyDown (KeyCode.Alpha8)) {
			placement = Placement.Block (TerrianBlockType.Grass);
		}

		if (Input.GetKeyDown (KeyCode.Mouse0)) {
			HandlePlacement ();
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
				var block = new TerrianBlock (coord.Value, placement.type.Value);
				planet.AddBlock (coord.Value, block);	
			}
		}
	}
}
