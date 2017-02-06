using System;
using UnityEngine;
using System.Collections.Generic;
using Cubiquity;

namespace AssemblyCSharp
{
	public enum TerrianBlockType {
		Stone,
		Soil,
		Grass,
		Water,
	}

	public class TerrianBlock
	{
		public readonly TerrianBlockType type;
		public readonly Vector3i coord;

		public readonly Dictionary<Dir, Surface> surfaceMap = new Dictionary<Dir, Surface>();

		public TerrianBlock placementBlock;

		private Dir gravityMask;

		private static Dictionary<TerrianBlockType, int> textureIds = new Dictionary<TerrianBlockType, int> {
			{ TerrianBlockType.Soil, 2 },
			{ TerrianBlockType.Grass, 0 },
			{ TerrianBlockType.Stone, 1 },
			{ TerrianBlockType.Water, 206 },
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

		public int[] GetTextureIds() {
			if (this.type == TerrianBlockType.Soil) {
				var list = new List<int> ();
				for (var i = 0; i < 6; i++) {
					Dir gravity = DirUtils.GetDir (i);
					if (HasGravity (gravity)) {
						list.Add (textureIds [TerrianBlockType.Grass]);
					} else {
						list.Add (textureIds [TerrianBlockType.Soil]);
					}
				}
				return list.ToArray ();
			}
			var textureId = textureIds [type];
			return new []{ textureId, textureId, textureId, textureId, textureId, textureId };
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
			var voxel = new Voxel ();
			voxel.coord = coord;
			voxel.textureIds = GetTextureIds ();
			voxel.transparent = transparent;
			voxel.isWater = type == TerrianBlockType.Water;
			return voxel;
		}
	}
}

