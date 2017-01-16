using System;

namespace AssemblyCSharp
{

	class AttackState : CharacterState {
		ICharacterBehaviour behaviour;
		Character target;
		Character character;

		public AttackState(ICharacterBehaviour behaviour, Character character, Character target) {
			this.behaviour = behaviour;
			this.character = character;
			this.target = target;
		}

		public CharacterState Step() {
			var reached = behaviour.Chase (target);

			if (reached) {
				var done = behaviour.Attack (target);

				if (done) {
					// Re-evaluate after each attack
					return new IdleState (behaviour, character);
				}
			} 

			return this;
		}
	}
	
}
