using System;

namespace AssemblyCSharp
{

	interface ICharacterState {
		ICharacterState Step();
		string Name();
	}
	
}
