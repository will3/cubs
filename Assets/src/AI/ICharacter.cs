using System;

namespace AssemblyCSharp
{

	public interface ICharacter {
		void Idle();
		void Patrol();
		// Return true when in range
		bool Chase(ICharacter character);
		// Return true when finishing an attack
		bool Attack(ICharacter character);
		ICharacter FindTarget();
		Traits Traits { get; }
	}
	
}
