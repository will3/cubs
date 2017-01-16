using System;

namespace AssemblyCSharp
{
	class IdleState : CharacterState {
		// Find target available straight away
		private readonly Cooldown findTargetCooldown = new Cooldown(0.5f, 0.5f);
		private readonly Cooldown idleDoneCooldown;
		private readonly Character character;

		ICharacterBehaviour behaviour;
		public IdleState(ICharacterBehaviour behaviour, Character character) {
			this.character = character;
			idleDoneCooldown = new Cooldown(character.idleLength);
			this.behaviour = behaviour;
		}

		public CharacterState Step() {
			behaviour.Idle ();
			findTargetCooldown.Update ();
			idleDoneCooldown.Update ();

			// If there's target, attack
			if (findTargetCooldown.Ready ()) {
				var target = behaviour.FindTarget ();

				if (target != null) {
					return new AttackState (behaviour, character, target);
				}
			}

			// If havn't done idling, idle
			if (!idleDoneCooldown.Ready ()) {
				return this;
			}

			// Otherwise patrol
			return new PatrolState (behaviour, character);
		}
	}
	
}
