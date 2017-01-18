using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;
using Cubiquity;
using UnityEditor.Animations;

public class Critter : MonoBehaviour {

	private Character character;

	private Billboard billBoard;

	private CharacterAI characterAI;

	private ICharacterBehaviour behaviour;

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
		billBoard.up = transform.TransformDirection (Vector3.up);
	}
}
