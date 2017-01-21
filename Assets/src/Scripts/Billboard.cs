using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour {
	public Vector3 up = Vector3.up;
	public Vector3 dir = Vector3.right;

	private SpriteRenderer spriteRenderer;
	void Start () { 
		spriteRenderer = GetComponent<SpriteRenderer> ();
		Debug.Assert (spriteRenderer != null);
	}
	
	void Update () {
		transform.LookAt (Camera.main.transform, up);

		// TODO
//		var cross = Vector3.Cross (up, dir);
//		var crossTransformed = Camera.main.transform.InverseTransformDirection (cross);
//
//		spriteRenderer.flipX = crossTransformed.z < 0;
	}
}
