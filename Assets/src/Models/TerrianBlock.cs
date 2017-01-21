using System;
using UnityEngine;
using System.Collections.Generic;
using Cubiquity;

namespace AssemblyCSharp
{
	public enum TerrianBlockType {
		Stone,
		Grass,
		Water
	}

	public class TerrianBlock
	{
		public readonly TerrianBlockType type;
		public readonly Vector3i coord;

		public readonly Dictionary<Dir, Surface> surfaceMap = new Dictionary<Dir, Surface>();

		private Dir gravityMask;

		private static Dictionary<TerrianBlockType, Color> colorMap = 
			new Dictionary<TerrianBlockType, Color> {
			{ TerrianBlockType.Grass, new Color (143 / 255.0f, 216 / 255.0f, 70 / 255.0f) },
			{ TerrianBlockType.Stone, new Color (178 / 255.0f, 175 / 255.0f, 171 / 255.0f) },
			{ TerrianBlockType.Water, Colors.hexToColor("#00B4C3") }
		};

		public TerrianBlock (Vector3i coord, TerrianBlockType type)
		{
			this.coord = coord;
			this.type = type;
		}

		public Color GetColor() {
			return colorMap [type];
		}

		public void SetGravity(Dir dir) {
			gravityMask |= dir;
		}

		public bool HasGravity(Dir dir) {
			return (gravityMask & dir) != Dir.None;
		}

		public Surface AddSurface(Dir dir) {
			if (!surfaceMap.ContainsKey (dir)) {
				surfaceMap [dir] = new Surface (coord, dir);
			}

			return surfaceMap [dir];
		}

		public Surface GetSurface(Dir dir) {
			if (surfaceMap.ContainsKey (dir)) {
				return surfaceMap [dir];
			}
			return null;
		}
	}
}

