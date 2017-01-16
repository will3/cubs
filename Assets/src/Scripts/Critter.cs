using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;
using Cubiquity;

public class Critter : MonoBehaviour {

	#region Editor properties

	public bool targetable = true;

	public float hitPoints = 100.0f;

	public bool isMonster = false;

	#endregion

	private Billboard billBoard;

	private CharacterAI characterAI;

	private ICharacter character;

	private Block block;

	// Use this for initialization
	void Start () {
		billBoard = GetComponentInChildren<Billboard> ();
		Debug.Assert (billBoard != null);
		block = GetComponent<Block> ();
		Debug.Assert (block != null);

		character = new MeleeCharacter (this.block, gameObject);
		characterAI = new CharacterAI (character);
	}

	// Update is called once per frame
	void Update () {
		// Unplaced
		if (block.currentSurface == null) {
			return;
		}

		billBoard.up = transform.TransformDirection (Vector3.up);

		characterAI.Step ();

		if (hitPoints <= 0.0f) {
			Destroy (gameObject);
		}
	}
}
