using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Dijkstras;

namespace AssemblyCSharp
{
	public class MeleeBehaviour : MonoBehaviour, ICharacterBehaviour
	{
		Character character;

		Movement movement;

		private Animator animator;

		public void Start() {
			character = GetComponent<Character> ();
			Debug.Assert (character != null);
			animator = GetComponentInChildren<Animator> ();
			Debug.Assert (animator != null);

			movement = new Movement (character);
		}

		public void Update() {
			movement.StepPath ();
			animator.SetWalking (movement.Walking);

			if (character.CurrentSurface != null) {
				DebugUtil.DrawPath (character.CurrentSurface, movement.CurrentPath);
			}
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

			if (movement.CurrentPath.path.Count == 0) {
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
			if (movement.CurrentPath.path.Count == 1) {
				var connection = planet.Terrian.ConnectionBetween (character.CurrentSurface, target);
			
				if (connection != null) {
					return true;
				}
			}

			return false;
		}

		public bool Attack (Character targetCharacter)
		{
			animator.TriggerAttack ();

			if (animator.IsInTransition (0)) {
				if (animator.GetAnimatorTransitionInfo (0)
					.IsName (Animators.TransitionAttackToIdle)) {
					character.Damage (targetCharacter);
					return true;
				}
			}

			return false;
		}

		public Character FindTarget ()
		{
			return GameObject.FindObjectsOfType<Character> ()
				.Where (u => {
					if (!u.Placed) {
						return false;
					}
					if (u == character) {
						return false;
					}

					return u.IsTarget (character);
				})
				.OrderBy (u => (u.gameObject.transform.position - transform.position).sqrMagnitude)
				.FirstOrDefault ();
		}
	}
}

