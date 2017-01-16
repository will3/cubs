using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;
using Cubiquity;

public class Spawner : MonoBehaviour {
	private double spawnTimestamp;
	private double spawnTime = 1.0;
	private BlockComponent blockComponent;

	private Surface currentSurface {
		get { return blockComponent.currentSurface; }
	}

	// Use this for initialization
	void Start () {
		spawnTimestamp = Time.time + spawnTime;
		blockComponent = GetComponent<BlockComponent> ();
		if (blockComponent == null) {
			Debug.LogError ("'Spawner' must have 'BlockComponent'");
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > spawnTimestamp) {
			Spawn ();
			spawnTimestamp = Time.time + spawnTime;	
		}
	}

	void Spawn() {
		var planet = Game.Instance.Planet;
		planet.Create (Prefabs.Spider, currentSurface);
	}
}
