using System;

namespace AssemblyCSharp
{
	public class Game
	{
		public Planet Planet;
		public Terrian Terrian;

		private static Game _instance;
		public static Game Instance {
			get {
				if (_instance == null) {
					_instance = new Game ();
				}
				return _instance;
			}
		}

		private Game () { }
	}
}

