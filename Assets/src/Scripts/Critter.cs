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

	private Animator animator;

	// Use this for initialization
	void Start () {
		billBoard = GetComponentInChildren<Billboard> ();
		Debug.Assert (billBoard != null);

		character = GetComponent<Character> ();
		Debug.Assert (character != null);

		if (character.blockCoord.surface == null) {
			Destroy (gameObject);
		}

		switch (character.behaviourType) {
		case BehaviourType.Melee:
			behaviour = gameObject.AddComponent<MeleeBehaviour> ();
			break;
		case BehaviourType.Range:
			behaviour = gameObject.AddComponent<RangeBehaviour> ();
			break;
		}

		characterAI = gameObject.AddComponent<CharacterAI> ();
		characterAI.behaviour = behaviour;

		animator = GetComponentInChildren<Animator> ();
		Debug.Assert (animator != null);
		var animationEvents = animator.GetBehaviour<CritterAnimationEvents> ();
		animationEvents.animationInfo = character.animationInfo;
	}

	// Update is called once per frame
	void Update () {
		billBoard.up = transform.TransformDirection (Vector3.up);
		billBoard.dir = character.moveDirection;
	}
}
