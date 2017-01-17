using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;
using Cubiquity;

public class Critter : MonoBehaviour {

	public Character character;

	public Billboard billBoard;

	public CharacterAI characterAI;

	public ICharacterBehaviour behaviour;

	// Use this for initialization
	void Start () {
		billBoard = GetComponentInChildren<Billboard> ();
		Debug.Assert (billBoard != null);

		character = GetComponent<Character> ();
		Debug.Assert (character != null);

		// TODO make it configurable
		behaviour = gameObject.AddComponent<MeleeBehaviour> ();

		characterAI = gameObject.AddComponent<CharacterAI> ();
		characterAI.behaviour = behaviour;
	}

	// Update is called once per frame
	void Update () {
		if (character.NotPlaced) {
			return;
		}

		billBoard.up = transform.TransformDirection (Vector3.up);

		if (character.hitPoints <= 0.0f) {
			Destroy (gameObject);
		}
	}
}
