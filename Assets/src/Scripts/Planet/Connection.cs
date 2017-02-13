using System;

namespace AssemblyCSharp
{
	public class Connection
	{
		private readonly Surface a;
		private readonly Surface b;
		public readonly String identifier;
		public readonly float distance;
		public readonly Surface[] surfaces;

		public bool hasWater {
			get {
				return a.isWater || b.isWater;
			}
		}

		public Connection (Surface a, Surface b, float distance)
		{
			this.a = a;
			this.b = b;
			this.surfaces = new [] {a, b};
			this.distance = distance;

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

