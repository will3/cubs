using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragCamera : MonoBehaviour {
	public Vector3 target = new Vector3();
	public Quaternion quaternion = Quaternion.Euler(-45, 45, 0);
	public float distance = 100;

	private Camera _camera;

	// Use this for initialization
	void Start () {
		_camera = GetComponent<Camera> ();
		if (_camera == null) {
			Debug.LogError ("'DragCamera' must be attached to 'Camera'");
		}
	}
	
	// Update is called once per frame
	void Update () {
		updateCamera ();
	}

	private void updateCamera() {
		var position = quaternion * new Vector3 (0, 0, 1) * distance + target;

		_camera.transform.position = position;
		_camera.transform.LookAt (target);
	}
}
