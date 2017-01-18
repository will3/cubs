using System;

namespace AssemblyCSharp
{

	class AttackState : ICharacterState {
		ICharacterBehaviour behaviour;
		Character target;
		Character character;

		public AttackState(ICharacterBehaviour behaviour, Character character, Character target) {
			this.behaviour = behaviour;
			this.character = character;
			this.target = target;
		}

		public ICharacterState Step() {
			if (target.Dead) {
				return new IdleState (behaviour, character);
			}

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

		public string Name() {
			return "Attack";
		}
	}
	
}
