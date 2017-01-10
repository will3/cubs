using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetController : MonoBehaviour {

	private Quaternion nextQuaternion;

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
			nextQuaternion = Quaternion.Euler (-90, 0, 0) * nextQuaternion;
		}
			
		transform.rotation = Quaternion.Slerp (transform.rotation, nextQuaternion, 0.1f);
	}
}
