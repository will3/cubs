using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;
using Cubiquity;

public class Critter : MonoBehaviour {
	private List<string> path = new List<string>();
	private Surface nextSurface;
	private float stepAmount = 0.0f;
	private float speed = 0.04f;
	private BlockComponent blockComponent;

	private Surface currentSurface { get { return blockComponent.currentSurface; } }

	private Billboard billBoard;

	// Use this for initialization
	void Start () {
		blockComponent = GetComponent<BlockComponent> ();
		Debug.Assert (blockComponent != null);
		billBoard = GetComponentInChildren<Billboard> ();
		Debug.Assert (billBoard != null);
	}

	// Update is called once per frame
	void Update () {
		// Unplaced
		if (currentSurface == null) {
			return;
		}

		billBoard.up = transform.TransformDirection (Vector3.up);

		if (path == null || path.Count == 0) {
			wander ();
		}

		var planet = Game.Instance.Planet;

		if (path != null && path.Count > 0) {
			var nextSurface = planet.Terrian.surfaceByIdentifier [path [0]];

			var distance = nextSurface.DistanceTo (currentSurface);
			stepAmount += speed;

			var ratio = stepAmount / distance;

			if (ratio > 1.0f) {
				ratio = 1.0f;
				stepAmount -= distance;
			}
				
			planet.LerpSurface (blockComponent, currentSurface, nextSurface, ratio);
				
			if (ratio == 1.0f) {
				path.RemoveAt (0);
				ratio = 0.0f;
			}
		}
	}

	public void SetTarget(Surface surface) {
		var planet = Game.Instance.Planet;
		var p = planet.Terrian.GetPath (surface, currentSurface);
		path = p.path;
		if (path == null || path.Count == 0) {
			return;
		}

		path.RemoveAt (0);

		if (p.finished) {
			path.Add (surface.identifier);
		}
	}

	void wander() {
		var planet = Game.Instance.Planet;
			
		var target = planet.RandomSurface (currentSurface, 6);

		if (target.identifier.Equals (currentSurface.identifier)) {
			return;	
		}
			
		SetTarget (target);
	}
}
