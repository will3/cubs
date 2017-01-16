using System;

namespace AssemblyCSharp
{

	class AttackState : CharacterState {
		ICharacter character;
		ICharacter target;

		public AttackState(ICharacter character, ICharacter target) {
			this.character = character;
			this.target = target;
		}

		public CharacterState Step() {
			var reached = character.Chase (target);

			if (reached) {
				var done = character.Attack (target);

				if (done) {
					// Re-evaluate after each attack
					return new IdleState (character);
				}
			} 

			return this;
		}
	}
	
}
