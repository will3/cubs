using System;
using UnityEngine;

namespace AssemblyCSharp
{
	public static class Animators {
		public static void SetWalking(this Animator animator, bool flag) {
			animator.SetBool("walking", flag);
		}

		public static void TriggerAttack(this Animator animator) {
			animator.SetTrigger("attack");
		}

		public static void SetWalkSpeed(this Animator animator, float speed) {
			animator.SetFloat ("walk_speed", speed);
		}

		public static void SetIdleSpeed(this Animator animator, float speed) {
			animator.SetFloat ("idle_speed", speed);
		}

		public static void SetAttackSpeed(this Animator animator, float speed) {
			animator.SetFloat ("attack_speed", speed);
		}

		public static string TransitionAttackToIdle = "attack -> idle";
	}
}

