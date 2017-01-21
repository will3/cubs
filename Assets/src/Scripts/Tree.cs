using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class Tree : MonoBehaviour, IBlock {

	private BlockCoord _blockCoord = new BlockCoord();

	public BlockCoord blockCoord {
		get { return _blockCoord; }
	}

	private Billboard billboard; 
	// Use this for initialization
	void Start () {
		if (blockCoord.currentSurface == null) {
			Destroy (gameObject);
		}
		billboard = GetComponentInChildren<Billboard> ();
		Debug.Assert (billboard != null);
	}
	
	// Update is called once per frame
	void Update () {
		billboard.up = Game.Instance.Planet.gameObject.transform.TransformDirection (blockCoord.currentSurface.normal);
	}
}
