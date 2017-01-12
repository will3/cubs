using System;
using UnityEngine;
using System.Collections.Generic;
using Cubiquity;

namespace AssemblyCSharp
{
	public enum BlockType {
		Stone,
		Grass
	}

	public class TerrianBlock
	{
		public readonly BlockType type;
		public readonly Vector3i coord;

		private Dir gravityMask;
		public readonly Dictionary<Dir, Surface> surfaceMap = new Dictionary<Dir, Surface>();

		private static Dictionary<BlockType, Color> colorMap = 
			new Dictionary<BlockType, Color> {
			{ BlockType.Grass, new Color (143 / 255.0f, 216 / 255.0f, 70 / 255.0f) },
			{ BlockType.Stone, new Color (178 / 255.0f, 175 / 255.0f, 171 / 255.0f) }
		};

		public TerrianBlock (Vector3i coord, BlockType type)
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

		public void SetSurface(Dir dir) {
			if (!surfaceMap.ContainsKey (dir)) {
				surfaceMap [dir] = new Surface (coord, dir);
			}
		}

		public Surface GetSurface(Dir dir) {
			if (surfaceMap.ContainsKey (dir)) {
				return surfaceMap [dir];
			}
			return null;
		}
	}
}

