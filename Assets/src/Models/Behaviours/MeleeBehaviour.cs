using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Dijkstras;

namespace AssemblyCSharp
{
	public class MeleeBehaviour : MonoBehaviour, ICharacterBehaviour, CritterAnimationEvents.Listener
	{
		Character character;

		Movement movement;

		private Animator animator;

		private CritterAnimationEvents animationEvents;

		private bool exitAttackTrigger = false;
		public void DidExitAttack ()
		{
			exitAttackTrigger = true;
		}

		public void DidEnterAny()
		{
			exitAttackTrigger = false;
		}
			
		public void Start() {
			character = GetComponent<Character> ();
			Debug.Assert (character != null);
			animator = GetComponentInChildren<Animator> ();
			Debug.Assert (animator != null);

			movement = new Movement (character);

			animationEvents = animator.GetBehaviour<CritterAnimationEvents> ();
			animationEvents.listener = this;
		}

		public void Update() {
			movement.StepPath ();
			animator.SetWalking (movement.Walking);
		}

		public void Idle ()
		{
		}

		private void MoveNextTo(Surface target) {
			movement.Move (target, true);
		}

		public void Patrol ()
		{
			var planet = Game.Instance.Planet;
			var currentSurface = character.CurrentSurface;

			if (movement.Done) {
				var target = planet.RandomSurface (currentSurface, 4);

				if (target.identifier.Equals (currentSurface.identifier)) {
					return;
				}

				movement.Move (target);
			}
		}

		public bool Chase (Character targetCharacter)
		{
			var planet = Game.Instance.Planet;

			var target = targetCharacter.CurrentSurface;

			MoveNextTo (target);

			// If next to target
			if (movement.Done) {
				var connection = planet.Terrian.ConnectionBetween (character.CurrentSurface, target);
			
				if (connection != null) {
					return true;
				}
			}

			return false;
		}

		public bool Attack (Character targetCharacter)
		{
			if (exitAttackTrigger) {
				exitAttackTrigger = false;
				character.Damage (targetCharacter);
				return true;
			}

			animator.TriggerAttack ();

			return false;
		}

		public Character FindTarget ()
		{
			var maxDis = 5.0f;
			var maxDisSq = maxDis * maxDis;
			return GameObject.FindObjectsOfType<Character> ()
				.Where (u => {
					if (!u.Placed) {
						return false;
					}
					if (u == character) {
						return false;
					}

					if ((u.gameObject.transform.position - transform.position).sqrMagnitude > maxDisSq) {
						return false;
					}

					return u.IsTarget (character);
				})
				.OrderBy (u => (u.gameObject.transform.position - transform.position).sqrMagnitude)
				.FirstOrDefault ();
		}
	}
}

