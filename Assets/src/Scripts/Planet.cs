using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cubiquity;

public class Planet : MonoBehaviour {
	private ColoredCubesVolume coloredCubesVolume;

	public int width = 16;
	public int height = 16;
	public int depth = 16;

	// Use this for initialization
	void Start () {
		coloredCubesVolume = gameObject.GetComponent<ColoredCubesVolume> ();
		if (coloredCubesVolume == null) {
			Debug.LogError ("Component 'Planet' requires 'ColoredCubesVolume'");
		}

		ColoredCubesVolumeData data = VolumeData.CreateEmptyVolumeData<ColoredCubesVolumeData>(new Region(0, 0, 0, width-1, height-1, depth-1));

		coloredCubesVolume.data = data;

		var color = new QuantizedColor (143, 216, 70, 255);

		for (int i = 0; i < width; i++) {
			for (int j = 0; j < height; j++) {
				for (int k = 0; k < depth; k++) {
					data.SetVoxel (i, j, k, color);
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
