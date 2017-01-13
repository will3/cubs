using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;
using Cubiquity;

public class Critter : MonoBehaviour {

	public bool debugMode = false;

	private Surface currentSurface;

	private List<string> path = new List<string>();

	private double stepTimestamp;
	private double timePerStep = 0.2;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		var planet = Game.Instance.Planet;

		if (debugMode) {
			if (Input.GetKeyDown (KeyCode.Mouse0)) {
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				var surface = planet.GetSurface (ray);

				if (surface != null) {
					if (currentSurface == null) {
						SetSurface (surface);
					} else {
						SetTarget (surface);
					}
				}
			}
		}

		if (Time.time > stepTimestamp) {
			step ();
		}	
	}

	public void SetSurface(Surface surface) {
		var planet = Game.Instance.Planet;
		currentSurface = surface;
		planet.SetSurface (gameObject, surface);
		resetStepTimer ();
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

	void step() {
		var planet = Game.Instance.Planet;
		if (path == null || path.Count == 0) {
			// Wander

			if (planet.Terrian.connectionBySurfaceIdentifier.ContainsKey (currentSurface.identifier)) {
				var connections = planet.Terrian.connectionBySurfaceIdentifier [currentSurface.identifier];

				var index = Random.Range(0, connections.Count - 1);
				var connection = connections [index];

				var otherSurface = connection.OtherSurface (currentSurface);
				SetSurface (otherSurface);
				resetStepTimer ();
			}

			return;
		}
			
		// TODO handle surface not found
		var surface = planet.Terrian.surfaceByIdentifier [path [0]];
		SetSurface (surface);
		path.RemoveAt (0);
		resetStepTimer ();
	}

	void resetStepTimer() {
		stepTimestamp = Time.time + timePerStep;
	}
}
