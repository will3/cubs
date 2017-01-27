using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;
using Cubiquity;

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

	private Vector3i? startCoord;
	private List<Vector3i> lastBlocks = new List<Vector3i> ();

	void Update () {
		var planet = Game.Instance.Planet;

		if (Input.GetKey (KeyCode.Mouse0) && buildingMode) {
			var coord = planet.GetCoord ();
			if (coord.HasValue) {
				if (!startCoord.HasValue) {
					startCoord = coord.Value;
				} else {
					var endCoord = coord.Value;

					foreach (var c in lastBlocks) {
						planet.RemoveBuildingPlacement (c);	
					}
					lastBlocks.Clear ();

					var xs = new List<int> (){ startCoord.Value.x, endCoord.x };
					var ys = new List<int> (){ startCoord.Value.y, endCoord.y };
					var zs = new List<int> (){ startCoord.Value.z, endCoord.z };
					xs.Sort ();
					ys.Sort ();
					zs.Sort ();

					for (var i = xs[0]; i <= xs[1]; i++) {
						for (var j = ys[0]; j <= ys[1]; j++) {
							for (var k = zs[0]; k <= zs[1]; k++) {
								var c = new Vector3i (i, j, k);

								var block = new TerrianBlock (c, TerrianBlockType.WireframeBlue);
								block.placementBlock = new TerrianBlock (c, placement.type.Value);
								planet.AddBlock (c, block);

								lastBlocks.Add (c);
							}
						}
					}
				}
			}
		}

		if (Input.GetKeyUp (KeyCode.Mouse0) && buildingMode) {
			lastBlocks.Clear ();
			startCoord = null;
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
		
			}
		}
	}
}
