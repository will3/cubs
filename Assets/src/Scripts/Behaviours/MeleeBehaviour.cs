using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Dijkstras;

namespace AssemblyCSharp
{
	public class MeleeBehaviour : MonoBehaviour, ICharacterBehaviour
	{
		private Character character;

		private Movement movement;

		private Targeting targeting;

		private Animator animator;

		private CritterAnimationEvents animationEvents;

		private BlockComponent blockComponent;
			
		public void Start() {
			character = GetComponent<Character> ();
			Debug.Assert (character != null);
			animator = GetComponentInChildren<Animator> ();
			Debug.Assert (animator != null);
			blockComponent = GetComponent<BlockComponent> ();
			Debug.Assert (blockComponent != null);

			movement = new Movement (character, blockComponent);
			targeting = new Targeting (character);

			animationEvents = animator.GetBehaviour<CritterAnimationEvents> ();
			Debug.Assert (animationEvents != null);
		}

		public void Update() {
			movement.StepPath ();
			animator.SetWalking (movement.Walking);

			if (animationEvents.finishedAttack) {
				hasAppliedAttack = false;
			}
		}

		public void Idle ()
		{
		}

		private bool startedPatrol = false;

		public bool Patrol ()
		{
			if (startedPatrol && movement.Done) {
				startedPatrol = false;
				return true;
			}

			movement.Patrol ();
			startedPatrol = true;
			return false;
		}

		public bool Chase (Character targetCharacter)
		{
			var planet = Game.Instance.Planet;

			var target = targetCharacter.blockComponent.blockCoord.surface;

			movement.Move (target, true);

			// If next to target
			if (movement.Done) {
				var connection = planet.Terrian.ConnectionBetween (character.blockComponent.blockCoord.surface, target);
			
				if (connection != null) {
					return true;
				}
			}

			return false;
		}

		private bool hasAppliedAttack = false;
		public bool Attack (Character targetCharacter)
		{
			if (animationEvents.exitAttack && !hasAppliedAttack) {
				hasAppliedAttack = true;
				targetCharacter.ApplyDamage (character.damage);
				return true;
			}

			if (animationEvents.idle) {
				animator.TriggerAttack ();
			}
				
			return false;
		}

		public Character FindTarget ()
		{
			return targeting.GetTargets().FirstOrDefault ();
		}
	}
}

