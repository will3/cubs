using System;
using System.Collections.Generic;
using UnityEngine;
using Cubiquity;

namespace AssemblyCSharp
{
	public class Terrian
	{
		public Dictionary<Vector3i, TerrianBlock> map = new Dictionary<Vector3i, TerrianBlock>();

		public Terrian () {}

		public void SetVoxel(int i, int j, int k, TerrianBlock block) {
			var coord = new Vector3i (i, j, k);
			if (block == null) {
				map.Remove (coord);
			} else {
				map [coord] = block;
			}
		}

		public bool HasVoxel(int i, int j, int k) {
			return GetVoxel (i, j, k) != null;
		}

		public bool HasVoxel(Vector3i v) {
			return HasVoxel (v.x, v.y, v.z);
		}

		public TerrianBlock GetVoxel(int i, int j, int k) {
			var coord = new Vector3i (i, j, k);
			TerrianBlock v;
			map.TryGetValue(coord, out v);
			return v;
		}

		public void Init(int size) {
			initGravity (size);

			foreach (var kv in map) {
				var coord = kv.Key;
				var block = kv.Value;

				foreach (var dir in DirUtils.Dirs) {
					var hasGravity = block.HasGravity (dir);
					if (!hasGravity) {
						continue;
					}
					var nextCoord = coord + DirUtils.GetUnitVector (dir);
					if (HasVoxel (nextCoord)) {
						continue;
					}
					block.SetSurface (dir);
				}
			}
		}

		private void initGravity(int size) {
			var center = new Vector3 (size, size, size) * 0.5f - new Vector3 (0.5f, 0.5f, 0.5f);

			foreach (var kv in map) {
				var coord = kv.Key;
				var block = kv.Value;

				var diff = new Vector3 (
					coord.x - center.x,
					coord.y - center.y,
					coord.z - center.z);

				var max = Math.Max (Math.Abs(diff.x), 
					Math.Max(Math.Abs(diff.y), Math.Abs(diff.z)));
				
				if (max == 0) {
					continue;
				}

				var ratio1 = 1 / 1.2f;
				var ratio2 = 1.2f;

				if (isWithinRatio(diff.x, max, ratio1, ratio2)) { 
					block.SetGravity (Dir.Right); 
				}
				if (isWithinRatio(diff.x, -max, ratio1, ratio2)) { 
					block.SetGravity (Dir.Left); 
				}

				if (isWithinRatio(diff.y, max, ratio1, ratio2)) { 
					block.SetGravity (Dir.Up); 
				}
				if (isWithinRatio(diff.y, -max, ratio1, ratio2)) { 
					block.SetGravity (Dir.Down); 
				}

				if (isWithinRatio(diff.z, max, ratio1, ratio2)) { 
					block.SetGravity (Dir.Forward); 
				}
				if (isWithinRatio(diff.z, -max, ratio1, ratio2)) { 
					block.SetGravity (Dir.Back); 
				}
			}
		}

		private bool isWithinRatio(float a, float b, float ratio1, float ratio2) {
			var ratio = Math.Abs (a / b);
			return ratio > ratio1 && ratio < ratio2;
		}
	}
}

