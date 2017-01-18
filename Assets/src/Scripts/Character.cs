using System;
using UnityEngine;

namespace AssemblyCSharp
{
	public class Character : MonoBehaviour, IBlock {

		public float animAttackSpeed = 1.0f;

		public float animWalkSpeed = 1.0f;

		public float animIdleSpeed = 1.0f;

		public float idleLength = 0.1f;

		public float patrolLength = 10.0f;

		public float speed = 0.04f;

		public float hitPoints = 100.0f;

		public bool isMonster = false;

		public int maxPathFindingSteps = 32;

		private Surface currentSurface;

		public bool attacking = false;

		public bool NotPlaced {
			get {
				return currentSurface == null;
			}
		}

		public bool IsTarget(Character character) {
			return this.isMonster != character.isMonster;
		}

		#region IBlock implementation

		public Surface CurrentSurface {
			get {
				return currentSurface;
			}
			set {
				currentSurface = value;
			}
		}

		#endregion

		public void Start() {
			var animator = GetComponentInChildren<Animator> ();
			animator.SetFloat ("attack_speed", animAttackSpeed);
			animator.SetFloat ("walk_speed", animWalkSpeed);
			animator.SetFloat ("idle_speed", animIdleSpeed);
		}
	}
	
}
