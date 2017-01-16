using System;

namespace AssemblyCSharp
{

	public interface ICharacterBehaviour {
		void Idle();
		void Patrol();
		// Return true when in range
		bool Chase(Character character);
		// Return true when finishing an attack
		bool Attack(Character character);
		Character FindTarget();
	}	
}
