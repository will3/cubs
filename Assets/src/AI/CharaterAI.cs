using System;

namespace AssemblyCSharp
{

	public class CharacterAI 
	{
		public readonly ICharacter character;

		private CharacterState state;

		public CharacterAI(ICharacter character) {
			this.character = character;
			state = new IdleState(character);
		}

		public void Step() {
			state = state.Step ();
		}
	}
}

