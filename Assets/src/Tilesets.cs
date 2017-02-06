using System;
using UnityEngine;

namespace AssemblyCSharp
{
	public static class Tilesets
	{
		public static Vector2 GetOffset(int id, int rows) {
			int column = id % rows;
			int row = id / rows;

			return new Vector2 (1 / (float)rows * (float)column, 1 - 1 / (float)rows * (float)row);
		}

		public static float GetTileSize(int rows, int pixelSize) {
			return 1.0f / (float)rows;
		}
	}
}

