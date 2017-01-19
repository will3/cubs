using System;
using UnityEngine;

namespace AssemblyCSharp
{
	public class Character : MonoBehaviour, IBlock {

		public string characterName = "<unnamed>";

		public float idleLength = 0.1f;

		public float patrolLength = 10.0f;

		public float speed = 0.04f;

		public float hitPoints = 100.0f;

		public bool isMonster = false;

		public int maxPathFindingSteps = 32;

		public float damage = 10.0f;

		public float vision = 5.0f;

		public BehaviourType behaviourType;

		#region animation properties

		public float animAttackSpeed = 1.0f;

		public float animWalkSpeed = 1.0f;

		public float animIdleSpeed = 1.0f;

		#endregion
	
		#region IBlock implementation

		private Surface currentSurface;

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
			animator.SetAttackSpeed (animAttackSpeed);
			animator.SetWalkSpeed (animWalkSpeed);
			animator.SetIdleSpeed (animIdleSpeed);
		}

		public void Update() {
			if (hitPoints <= 0.0f) {
				Destroy (gameObject);
			}
		}

		public void Damage(Character character) {
			character.hitPoints -= damage;
			Debug.LogFormat ("{0} attacked {1} for {2} damage", this.characterName, character.characterName, damage);
		}

		public bool Placed {
			get {
				return currentSurface != null;
			}
		}

		public bool Dead {
			get {
				return hitPoints <= 0.0f;
			}
		}

		public bool IsTarget(Character character) {
			return this.isMonster != character.isMonster;
		}

	}
	
}
