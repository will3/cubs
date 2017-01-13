using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;
using Cubiquity;

public class Spawner : MonoBehaviour {
	public Surface surface;

	private double spawnTimestamp;
	private double spawnTime = 1.0;

	// Use this for initialization
	void Start () {
		spawnTimestamp = Time.time + spawnTime;
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
		var obj = planet.Create ("Critter", surface);
		var critter = obj.GetComponent<Critter> ();
		critter.SetSurface (surface);
	}
}
