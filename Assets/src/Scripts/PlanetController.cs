using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class PlanetController : MonoBehaviour {

	private Quaternion nextQuaternion;
	private Vector3 nextPosition;

	void Awake() {
		Game.Instance.planetController = this;
	}

	// Use this for initialization
	void Start () {
		nextQuaternion = transform.rotation;

		var dragCamera = Camera.main.GetComponent<DragCamera> ();
		if (dragCamera != null) {
			var planet = Game.Instance.Planet;
			dragCamera.distance = planet.size * 1.3f;
		}
	}
	
	// Update is called once per frame
	void Update () {
		var speed = 1.0f;
		var camera = Camera.main;

		if (Input.GetKey (KeyCode.W)) {
			var back = camera.transform.TransformDirection (Vector3.back) * speed;
			back = Vector3.ProjectOnPlane (back, Vector3.up);
			nextPosition += back * speed;
		}

		if (Input.GetKey (KeyCode.S)) {
			var forward = camera.transform.TransformDirection (Vector3.forward);
			forward = Vector3.ProjectOnPlane (forward, Vector3.up);
			nextPosition += forward * speed;
		}

		if (Input.GetKey (KeyCode.A)) {
			nextPosition += camera.transform.TransformDirection (Vector3.right) * speed;
		}

		if (Input.GetKey (KeyCode.D)) {
			nextPosition += camera.transform.TransformDirection (Vector3.left) * speed;
		}

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

		if (Input.GetKeyDown (KeyCode.Q)) {
			nextQuaternion = Quaternion.Euler (0, 90, 0) * nextQuaternion;
		}

		if (Input.GetKeyDown (KeyCode.E)) {
			nextQuaternion = Quaternion.Euler (0, -90, 0) * nextQuaternion;
		}
			
		transform.rotation = Quaternion.Slerp (transform.rotation, nextQuaternion, 0.1f);
		transform.position = Vector3.Lerp (transform.position, nextPosition, 0.2f);
	}
}
