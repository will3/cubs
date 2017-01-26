using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class Bush : MonoBehaviour {

	private Billboard billboard;
	private BlockComponent blockComponent;

	void Start () {
		billboard = GetComponentInChildren<Billboard> ();
		blockComponent = GetComponent<BlockComponent> ();
	}

	void Update () {
		var normal = blockComponent.surface.normal;
		billboard.up = Game.Instance.Planet.gameObject.transform.TransformDirection (normal);
	}
}
