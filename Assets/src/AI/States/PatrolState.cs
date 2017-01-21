using System;

namespace AssemblyCSharp
{

	class PatrolState : ICharacterState {
		ICharacterBehaviour behaviour;
		private readonly Cooldown findTargetCooldown = new Cooldown(0.1f);
		private Character character;

		public PatrolState(ICharacterBehaviour behaviour, Character character) {
			this.character = character;
			this.behaviour = behaviour;
		}

		public ICharacterState Step() {
			findTargetCooldown.Update ();

			// Attack if there is target
			if (findTargetCooldown.Ready ()) {
				var target = behaviour.FindTarget ();

				if (target != null) {
					return new AttackState (behaviour, character, target);
				}
			}

			// Keep patrolling
			var finished = behaviour.Patrol ();
			if (!finished) {
				return this;
			}

			return new IdleState (behaviour, character);
		}

		public string Name() {
			return "Patrol";
		}
	}
	
}
