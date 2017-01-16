using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour {
	public Vector3 up = Vector3.up;

	void Start () { }
	
	void Update () {
		transform.LookAt (Camera.main.transform, up);
	}
}
