using System;
using UnityEngine;

namespace AssemblyCSharp
{
	public class CharacterAI : MonoBehaviour
	{
		public ICharacterBehaviour behaviour;
		public Character character;

		private ICharacterState currentState;

		public void Start() {
			Debug.Assert (behaviour != null);

			character = GetComponent<Character> ();
			Debug.Assert (character != null);

			currentState = new IdleState(behaviour, character);
		}

		public void Update() {
			if (!character.Placed) {
				return;
			}

			currentState = currentState.Step ();
		}
	}
}

