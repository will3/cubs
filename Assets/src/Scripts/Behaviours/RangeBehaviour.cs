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

		private Billboard billboard;

		private CritterAnimationEvents animationEvents;

		private bool hasFiredArrow = false;

		public void Start() {
			character = GetComponent<Character> ();
			Debug.Assert (character != null);
			animator = GetComponentInChildren<Animator> ();
			Debug.Assert (animator != null);
			billboard = GetComponentInChildren<Billboard> ();
			Debug.Assert (billboard != null);

			movement = new Movement (character);
			targeting = new Targeting (character);

			animationEvents = animator.GetBehaviour<CritterAnimationEvents> ();
			Debug.Assert (animationEvents != null);
		}

		public void Update() {
			movement.StepPath ();
			animator.SetWalking (movement.Walking);

			if (animationEvents.exitAttack) {
				hasFiredArrow = false;
			}
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
			if (animationEvents.finishedAttack && !hasFiredArrow) {
				hasFiredArrow = true;
				var planet = Game.Instance.Planet;
				var obj = Prefabs.Create (Prefabs.Objects.Arrow);
				var arrow = obj.GetComponent<Arrow> ();
				arrow.transform.parent = planet.gameObject.transform;

				var a = character.CalcAttackExitPoint ();
				var b = targetCharacter.CalcCenterPoint ();

				arrow.gameObject.transform.position = a + (b - a).normalized * 0.5f;

				var dis = (arrow.gameObject.transform.position - b).magnitude;
				var speed = 20.0f;
				arrow.timeToLive = dis / speed;
				var dir = (b - a).normalized;
				arrow.velocity = dir * speed;
				arrow.target = targetCharacter;

				return true;
			}

			if (animationEvents.idle) {
				animator.TriggerAttack ();
			}

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

