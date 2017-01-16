using System;

namespace AssemblyCSharp
{

	class IdleState : CharacterState {
		// Find target available straight away
		private readonly Cooldown findTargetCooldown = new Cooldown(0.5f, 0.5f);
		private readonly Cooldown idleDoneCooldown;

		ICharacter character;
		public IdleState(ICharacter character) {
			idleDoneCooldown = new Cooldown(character.Traits.idleLength);
			this.character = character;
		}

		public CharacterState Step() {
			character.Idle ();
			findTargetCooldown.Update ();
			idleDoneCooldown.Update ();

			// If there's target, attack
			if (findTargetCooldown.Ready ()) {
				var target = character.FindTarget ();

				if (target != null) {
					return new AttackState (character, target);
				}
			}

			// If havn't done idling, idle
			if (!idleDoneCooldown.Ready ()) {
				return this;
			}

			// Otherwise patrol
			return new PatrolState (character);
		}
	}
	
}
