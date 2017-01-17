using System;
using UnityEngine;

namespace AssemblyCSharp
{
	public class Character : MonoBehaviour, IBlock {
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

		public float idleLength = 0.1f;

		public float patrolLength = 10.0f;

		public float speed = 0.04f;

		public float hitPoints = 100.0f;

		public bool isMonster = false;

		public int maxPathFindingSteps = 32;

		private Surface currentSurface;

		public bool NotPlaced {
			get {
				return currentSurface == null;
			}
		}

		public bool IsTarget(Character character) {
			return this.isMonster != character.isMonster;
		}
	}
	
}
