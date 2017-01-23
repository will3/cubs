using System;
using UnityEngine;
using System.Collections.Generic;
using Cubiquity;

namespace AssemblyCSharp
{
	public enum TerrianBlockType {
		Stone,
		Grass,
		Water,
		StoneWall
	}

	public class TerrianBlock
	{
		public readonly TerrianBlockType type;
		public readonly Vector3i coord;

		public readonly Dictionary<Dir, Surface> surfaceMap = new Dictionary<Dir, Surface>();

		private Dir gravityMask;

		private static Dictionary<TerrianBlockType, int> textureIds = new Dictionary<TerrianBlockType, int> {
			{ TerrianBlockType.Grass, 0 },
			{ TerrianBlockType.Stone, 1 },
			{ TerrianBlockType.Water, 2 },
			{ TerrianBlockType.StoneWall, 8 }
		};

		public bool transparent {
			get {
				return type == TerrianBlockType.Water;
			}
		}

		public TerrianBlock (Vector3i coord, TerrianBlockType type)
		{
			this.coord = coord;
			this.type = type;
		}

		public int GetTextureId() {
			return textureIds [type];
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

		public Voxel ToVoxel() {
			return new Voxel (
				coord, 
				GetTextureId (), 
				transparent, 
				type == TerrianBlockType.Water);
		}
	}
}

