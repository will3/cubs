using System;

namespace AssemblyCSharp
{
	[Serializable]
	public class Character {
		public float idleLength = 0.1f;

		public float patrolLength = 10.0f;

		public float speed = 0.04f;

		public bool targetable = true;

		public float hitPoints = 100.0f;

		public bool isMonster = false;
	}
	
}
