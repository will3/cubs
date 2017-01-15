using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class BlockComponent : MonoBehaviour {

	public Surface currentSurface;

	public bool targetable = false;

	public float hitPoints = 100.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (hitPoints <= 0.0f) {
			Destroy (gameObject);
		}
	}
}
