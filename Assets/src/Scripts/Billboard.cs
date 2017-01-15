using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour {
	void Start () { }
	
	void Update () {
		var up = transform.parent.TransformDirection (Vector3.up);
		transform.LookAt (Camera.main.transform, up);
	}
}
