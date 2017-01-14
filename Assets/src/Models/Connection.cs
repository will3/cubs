using System;

namespace AssemblyCSharp
{
	public class Connection
	{
		public readonly Surface a;
		public readonly Surface b;
		public readonly String identifier;
		public Connection (Surface a, Surface b)
		{
			this.a = a;
			this.b = b;
			identifier = a.identifier + "," + b.identifier;
		}

		public Surface OtherSurface(Surface surface) {
			return a.identifier.Equals (surface.identifier) ? b : a;
		}

		public Surface OtherSurface(string surfaceIdentifier) {
			return a.identifier.Equals (surfaceIdentifier) ? b : a;
		}
	}
}

