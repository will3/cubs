using System;

namespace AssemblyCSharp
{

	class PatrolState : ICharacterState {
		ICharacterBehaviour behaviour;
		private readonly Cooldown patrolDoneCooldown;
		private readonly Cooldown findTargetCooldown = new Cooldown(0.5f);
		private Character character;

		public PatrolState(ICharacterBehaviour behaviour, Character character) {
			this.character = character;
			this.behaviour = behaviour;
			patrolDoneCooldown = new Cooldown(character.patrolLength);
		}

		public ICharacterState Step() {
			patrolDoneCooldown.Update ();
			findTargetCooldown.Update ();

			// Attack if there is target
			if (findTargetCooldown.Ready ()) {
				var target = behaviour.FindTarget ();

				if (target != null) {
					return new AttackState (behaviour, character, target);
				}
			}

			// Idle if patrol done
			if (patrolDoneCooldown.Ready ()) {
				return new IdleState (behaviour, character);	
			}

			// Keep patrolling
			behaviour.Patrol ();
			return this;
		}

		public string Name() {
			return "Patrol";
		}
	}
	
}
