using System;
using UnityEngine;

namespace AssemblyCSharp
{
	public class BlockCoord
	{
		public BlockCoord() { }
		public BlockCoord(Surface surface, Vector2 uv = new Vector2()) {
			this.surface = surface;
			this.uv = uv;
		}

		public Surface surface;
		public Vector2 uv;
	}
}

