using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

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

public class BuildMenuController : MonoBehaviour {
	void Start () {
		
	}

	void Update () {
		
	}

	public void OnOptionSelected(string option) {
		if (option == "stone_wall") {
			var placement = Placement.Block (TerrianBlockType.StoneWall);
			Game.Instance.placeBuilding.StartPlacement (placement);
			HideMenu ();
		} else if (option == "camp_fire") {
			var placement = Placement.Prefab (Prefabs.Objects.Campfire);
			Game.Instance.placeBuilding.StartPlacement (placement);
		}
	}

	void HideMenu() {
		gameObject.SetActive (false);
	}
}
