using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cubiquity;
using AssemblyCSharp;

public class Planet : MonoBehaviour {
	private ColoredCubesVolume coloredCubesVolume;
	public int size = 16;

	// Use this for initialization
	void Start () {
		coloredCubesVolume = gameObject.GetComponent<ColoredCubesVolume> ();
		if (coloredCubesVolume == null) {
			Debug.LogError ("Component 'Planet' requires 'ColoredCubesVolume'");
		}

		ColoredCubesVolumeData data = VolumeData.CreateEmptyVolumeData<ColoredCubesVolumeData>(new Region(0, 0, 0, size, size, size));

		coloredCubesVolume.data = data;

		var color = new QuantizedColor (143, 216, 70, 255);

		for (int i = 0; i < size; i++) {
			for (int j = 0; j < size; j++) {
				for (int k = 0; k < size; k++) {
					data.SetVoxel (i, j, k, color);
				}
			}
		}

		var center = new Vector3 (-size / 2, -size / 2, -size / 2) + (new Vector3() * 0.5f);
		gameObject.transform.position = center;

		generateHeightMap ();
		generateTerrian ();
	}

	private void generateHeightMap() {
		var data = coloredCubesVolume.data;

		for (var d = 0; d < 3; d++) {
			foreach(var side in new [] {0, 1}) {
				var g1 = new Noise ();
				var g2 = new Noise ();
				g2.scale = g1.scale * 2;

				var dir = side == 0 ? 1 : -1;

				var u = (d + 1) % 3;
				var v = (d + 2) % 3;

				for (var i = 0; i < size; i++) {
					for (var j = 0; j < size; j++) {
						var noise = g1.get (i, j, 0) + g2.get(i, j, 0) * 0.5f;
						var height = (int)Mathf.Floor(noise * 4.0f);

						var coord = new [] { 0, 0, 0 };
						var startD = side * (size - 1);
						coord [d] = startD;
						coord [u] = i;
						coord [v] = j;

						for (var k = 0; k < height; k++) {
							coord [d] = startD + k * dir;
							data.SetVoxel (coord [0], coord [1], coord [2], new QuantizedColor (0, 0, 0, 0));
						}
					}
				}
			}
		}
	}

	private void generateTerrian() {
		var data = coloredCubesVolume.data;

		var g1 = new Noise ();
		g1.scale = 0.05f;
		var g2 = new Noise ();
		g2.scale = g1.scale * 2;

		var stone = new QuantizedColor (178, 175, 171, 255);

		for (var i = 0; i < size; i++) {
			for (var j = 0; j < size; j++) {
				for (var k = 0; k < size; k++) {
					var v = data.GetVoxel (i, j, k);
					if (v.alpha == 0) {
						continue;
					}

					var noise = g1.get (i, j, k) + g2.get (i, j, k) * 0.5f;
					noise /= 1.5f;

					if (noise > 0.5) {
						data.SetVoxel (i, j, k, stone);
					}
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
