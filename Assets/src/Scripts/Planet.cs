using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cubiquity;
using AssemblyCSharp;

public class Planet : MonoBehaviour {
	public int size = 16;
	public float heightDiff = 4.0f;

	private ColoredCubesVolume coloredCubesVolume;
	private Terrian terrian = new Terrian();

	// Use this for initialization
	void Start () {
		coloredCubesVolume = gameObject.GetComponent<ColoredCubesVolume> ();
		if (coloredCubesVolume == null) {
			Debug.LogError ("Component 'Planet' requires 'ColoredCubesVolume'");
		}

		ColoredCubesVolumeData data = VolumeData.CreateEmptyVolumeData<ColoredCubesVolumeData>(new Region(0, 0, 0, size, size, size));
		coloredCubesVolume.data = data;
	
		for (int i = 0; i < size; i++) {
			for (int j = 0; j < size; j++) {
				for (int k = 0; k < size; k++) {
					var coord = new Vector3i (i, j, k);
					terrian.SetVoxel (i, j, k, new TerrianBlock (coord, BlockType.Grass));
				}
			}
		}

		var center = new Vector3 (-size / 2, -size / 2, -size / 2) + (new Vector3() * 0.5f);
		gameObject.transform.position = center;

		generateHeightMap ();
		generateBiomes ();

		loadData ();

		terrian.Init (size);
	}

	private void generateHeightMap() {
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
						var height = (int)Mathf.Floor(noise / 1.5f * heightDiff);

						var coord = new [] { 0, 0, 0 };
						var startD = side * (size - 1);
						coord [d] = startD;
						coord [u] = i;
						coord [v] = j;

						for (var k = 0; k < height; k++) {
							coord [d] = startD + k * dir;
							terrian.SetVoxel (coord [0], coord [1], coord [2], null);
						}
					}
				}
			}
		}
	}

	private void generateBiomes() {
		var g1 = new Noise ();
		g1.scale = 0.05f;
		var g2 = new Noise ();
		g2.scale = g1.scale * 2;

		for (var i = 0; i < size; i++) {
			for (var j = 0; j < size; j++) {
				for (var k = 0; k < size; k++) {
					if (!terrian.HasVoxel (i, j, k)) {
						continue;
					}

					var noise = g1.get (i, j, k) + g2.get (i, j, k) * 0.5f;
					noise /= 1.5f;

					if (noise > 0.5) {
						var coord = new Vector3i (i, j, k);
						terrian.SetVoxel (i, j, k, new TerrianBlock (coord, BlockType.Stone));
					}
				}
			}
		}
	}

	private void loadData() {
		var data = coloredCubesVolume.data;

		foreach (var kv in terrian.map) {
			var coord = kv.Key;
			var block = kv.Value;
			var color = block.GetColor ();
			data.SetVoxel (coord.x, coord.y, coord.z, new QuantizedColor (
				(byte)(color.r * 255), 
				(byte)(color.g * 255), 
				(byte)(color.b * 255), 
				(byte)(color.a * 255)
			));
		}
	}
	
	// Update is called once per frame
	void Update () {
		foreach (var block in terrian.map.Values) {
			foreach (var surface in block.surfaceMap.Values) {
				Debug.DrawLine (
					gameObject.transform.TransformPoint(surface.center),
					gameObject.transform.TransformPoint(surface.pointAbove),
					Color.red);
			}
		}
	}
}
