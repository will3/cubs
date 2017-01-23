using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class PlanetController : MonoBehaviour {

	private Quaternion nextQuaternion;

	void Awake() {
		Game.Instance.planetController = this;
	}

	// Use this for initialization
	void Start () {
		nextQuaternion = transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.RightArrow)) {
			nextQuaternion = Quaternion.Euler (0, 0, 90) * nextQuaternion;
		}

		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			nextQuaternion = Quaternion.Euler (0, 0, -90) * nextQuaternion;
		}

		if (Input.GetKeyDown (KeyCode.UpArrow)) {
			nextQuaternion = Quaternion.Euler (-90, 0, 0) * nextQuaternion;
		}

		if (Input.GetKeyDown (KeyCode.DownArrow)) {
			nextQuaternion = Quaternion.Euler (90, 0, 0) * nextQuaternion;
		}
			
		transform.rotation = Quaternion.Slerp (transform.rotation, nextQuaternion, 0.1f);
	}
}
