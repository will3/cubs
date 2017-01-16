using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;
using Cubiquity;

public class Critter : MonoBehaviour {

	#region Editor properties

	public Character character = new Character();

	#endregion

	private Billboard billBoard;

	private CharacterAI characterAI;

	private ICharacterBehaviour behaviour;

	private Block block;

	// Use this for initialization
	void Start () {
		billBoard = GetComponentInChildren<Billboard> ();
		Debug.Assert (billBoard != null);
		block = GetComponent<Block> ();
		Debug.Assert (block != null);

		behaviour = new MeleeBehaviour (this.block, character, gameObject);
		characterAI = new CharacterAI (behaviour, character);
	}

	// Update is called once per frame
	void Update () {
		// Unplaced
		if (block.currentSurface == null) {
			return;
		}

		billBoard.up = transform.TransformDirection (Vector3.up);

		characterAI.Step ();

		if (character.hitPoints <= 0.0f) {
			Destroy (gameObject);
		}
	}
}
