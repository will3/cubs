using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turrent : BlockComponent {
	private GameObject innerObject;

	void Start () {
		innerObject = transform.Find ("Inner").gameObject;
	}

	void Update () {
		innerObject.transform.Rotate (new Vector3 (0, 4, 0));
	}
}
