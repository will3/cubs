using System;

namespace AssemblyCSharp
{

	class PatrolState : CharacterState {
		ICharacter character;
		private readonly Cooldown patrolDoneCooldown;
		private readonly Cooldown findTargetCooldown = new Cooldown(0.5f);

		public PatrolState(ICharacter character) {
			this.character = character;
			patrolDoneCooldown = new Cooldown(character.Traits.patrolLength);
		}

		public CharacterState Step() {
			patrolDoneCooldown.Update ();
			findTargetCooldown.Update ();

			// Attack if there is target
			if (findTargetCooldown.Ready ()) {
				var target = character.FindTarget ();

				if (target != null) {
					return new AttackState (character, target);
				}
			}

			// Idle if patrol done
			if (patrolDoneCooldown.Ready ()) {
				return new IdleState (character);	
			}

			// Keep patrolling
			character.Patrol ();
			return this;
		}
	}
	
}
