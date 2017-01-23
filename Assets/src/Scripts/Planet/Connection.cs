using System;

namespace AssemblyCSharp
{
	public class Connection
	{
		public readonly Surface a;
		public readonly Surface b;
		public readonly String identifier;
		public readonly float distance;

		public Connection (Surface a, Surface b, float distance)
		{
			this.a = a;
			this.b = b;
			this.distance = distance;
			identifier = IdentifierForSurfaces (a, b);
		}

		public static string IdentifierForSurfaces(Surface a, Surface b) {
			return a.identifier + "," + b.identifier;
		}

		public Surface OtherSurface(Surface surface) {
			return a.identifier.Equals (surface.identifier) ? b : a;
		}

		public Surface OtherSurface(string surfaceIdentifier) {
			return a.identifier.Equals (surfaceIdentifier) ? b : a;
		}
	}
}

