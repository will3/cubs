using System;
using UnityEngine;
using System.Linq;

namespace AssemblyCSharp
{
	public class RangeBehaviour : MonoBehaviour, ICharacterBehaviour
	{
		private Character character;

		private Movement movement;

		private Targeting targeting;

		private Animator animator;

		private CritterAnimationEvents animationEvents;

		public void Start() {
			character = GetComponent<Character> ();
			Debug.Assert (character != null);
			animator = GetComponentInChildren<Animator> ();
			Debug.Assert (animator != null);

			movement = new Movement (character);
			targeting = new Targeting (character);

			animationEvents = animator.GetBehaviour<CritterAnimationEvents> ();
			Debug.Assert (animationEvents != null);
		}

		public void Update() {
			movement.StepPath ();
			animator.SetWalking (movement.Walking);
		}
			
		public void Idle ()
		{ }

		public void Patrol ()
		{
			movement.Patrol ();
		}

		public bool Chase (Character character)
		{
			return true;
		}

		public bool Attack (Character targetCharacter)
		{
			if (animationEvents.exitAttack) {
				return true;
			}

			animator.TriggerAttack ();
//			character.Damage (targetCharacter);

			// https://en.wikipedia.org/wiki/Trajectory_of_a_projectile#Angle_.CE.B8_required_to_hit_coordinate_.28x.2Cy.29
			// Angle {\displaystyle \theta }  \theta required to hit coordinate (x,y)

			return false;
		}

		public Character FindTarget ()
		{
			var targets = targeting.GetTargets ();

			foreach (var target in targets) {
				if (HasVision (target)) {
					return target;
				}
			}

			return null;
		}

		private bool HasVision(Character target) {
			var planet = Game.Instance.Planet;
			var a = planet.gameObject.transform.TransformPoint (character.CurrentSurface.pointAbove);
			var b = target.transform.position + 
				planet.gameObject.transform.TransformDirection(target.CurrentSurface.normal) * 0.5f;
			var dis = Vector3.Distance (a, b);

			var ray = new Ray (a, (b - a).normalized);

			RaycastHit hitInfo;
			Physics.Raycast (ray, out hitInfo, dis);
			return !Physics.Raycast (ray, dis);
		}
	}
}

