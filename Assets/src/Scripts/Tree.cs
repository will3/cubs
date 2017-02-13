using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class Tree : MonoBehaviour {

	private BlockComponent blockComponent;

	private Billboard billboard; 
	// Use this for initialization
	void Start () {
		blockComponent = GetComponent<BlockComponent> ();
		Debug.Assert (blockComponent);
		billboard = GetComponentInChildren<Billboard> ();
		Debug.Assert (billboard != null);
	}
	
	// Update is called once per frame
	void Update () {
		var normal = blockComponent.surface.normal;
		billboard.up = Game.Instance.Planet.gameObject.transform.TransformDirection (normal);
	}
}
