using System;

namespace AssemblyCSharp
{
	public class CharacterAI 
	{
		public readonly ICharacterBehaviour behaviour;

		private CharacterState state;

		public CharacterAI(ICharacterBehaviour behaviour, Character character) {
			this.behaviour = behaviour;
			state = new IdleState(behaviour, character);
		}

		public void Step() {
			state = state.Step ();
		}
	}
}

