using System;
using UnityEngine;

namespace AssemblyCSharp
{
	public class Character : MonoBehaviour {
		public float idleLength = 0.1f;

		public float patrolLength = 10.0f;

		public float speed = 0.04f;


		public float hitPoints = 100.0f;

		public bool isMonster = false;

		public int maxPathFindingSteps = 32;

		public bool IsTarget(Character character) {
			return this.isMonster != character.isMonster;
		}
	}
	
}
