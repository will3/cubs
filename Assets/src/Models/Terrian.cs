using System;
using System.Collections.Generic;
using UnityEngine;
using Cubiquity;
using Dijkstras;

namespace AssemblyCSharp
{
	public class Terrian : PathFindingContext
	{
		public readonly int size;
		private float heightDiff = 4.0f;
		private int seaLevel = 11;

		public Dictionary<Vector3i, TerrianBlock> map = new Dictionary<Vector3i, TerrianBlock>();

		public IDictionary<string, Connection> AllConnections {
			get { return connectionLookUp; }	
		}

		public IDictionary<string, Surface> AllSurfaces {
			get { return surfaceLookUp; }
		}

		private readonly Dictionary<string, Surface> surfaceLookUp = 
			new Dictionary<string, Surface> ();
		private readonly Dictionary<string, Connection> connectionLookUp = 
			new Dictionary<string, Connection> ();

		private const float maxDistanceBetweenSurfaces = 1000.0f;

		private Graph graph = new Graph ();

		private Vector3 center;

		public Terrian (int size) {
			this.size = size;
			graph.context = this;
		}

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

		public void Generate() {
			center = new Vector3 (size, size, size) * 0.5f - new Vector3 (0.5f, 0.5f, 0.5f);
			initBlocks ();
			generateHeightMap ();
			generateBiomes ();
			initGravity ();
			initSurfaces ();
			initConnections ();
		}

		private void initSurfaces() {
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
					var surface = block.AddSurface (dir);
					surface.isWater = block.type == TerrianBlockType.Water;
					surfaceLookUp [surface.identifier] = surface;
				}
			}
		}

		private void initGravity() {
			foreach (var kv in map) {
				var coord = kv.Key;
				var block = kv.Value;

				var dirs = GetGravities (coord.to_f() - center);

				foreach (var dir in dirs) {
					block.SetGravity (dir);
				}
			}
		}
			
		public IList<Dir> GetGravities(Vector3 position) {
			var max = Math.Max (Math.Abs(position.x), 
				Math.Max(Math.Abs(position.y), Math.Abs(position.z)));

			var dirs = new List<Dir> ();

			if (max == 0) {
				return dirs;
			}

			var ratio1 = 1 / 1.2f;
			var ratio2 = 1.2f;

			if (isWithinRatio(position.x, max, ratio1, ratio2)) { 
				dirs.Add (Dir.Right);
			}
			if (isWithinRatio(position.x, -max, ratio1, ratio2)) { 
				dirs.Add (Dir.Left);
			}

			if (isWithinRatio(position.y, max, ratio1, ratio2)) { 
				dirs.Add (Dir.Up);
			}
			if (isWithinRatio(position.y, -max, ratio1, ratio2)) { 
				dirs.Add (Dir.Down);
			}

			if (isWithinRatio(position.z, max, ratio1, ratio2)) {
				dirs.Add (Dir.Forward);
			}
			if (isWithinRatio(position.z, -max, ratio1, ratio2)) { 
				dirs.Add (Dir.Back);
			}

			return dirs;
		}

		public Vector3 GetGravity(Vector3 position) {
			var dirs = GetGravities (position);
			if (dirs.Count == 0) {
				return new Vector3 ();
			}

			var v = new Vector3 ();
			foreach (var dir in dirs) {
				v += DirUtils.GetUnitVector (dir).to_f ();
			}

			return v.normalized * -300f;
		}

		private bool isWithinRatio(float a, float b, float ratio1, float ratio2) {
			var ratio = a / b;
			return ratio > ratio1 && ratio < ratio2;
		}

		private void initBlocks() {
			for (int i = 0; i < size; i++) {
				for (int j = 0; j < size; j++) {
					for (int k = 0; k < size; k++) {
						var coord = new Vector3i (i, j, k);
						SetVoxel (i, j, k, new TerrianBlock (coord, TerrianBlockType.Grass));
					}
				}
			}
		}

		private void generateHeightMap() {
			for (var d = 0; d < 3; d++) {
				foreach(var side in new [] {0, 1}) {
					var g1 = new Noise ();
					g1.frequency = 1.0f / (float)size * 1.0f;

					var g2 = new Noise ();
					g2.frequency = 0.05f;

					var g3 = new Noise ();
					g3.frequency = 0.01f;

					var dir = side == 0 ? 1 : -1;

					var u = (d + 1) % 3;
					var v = (d + 2) % 3;

					for (var i = 0; i < size; i++) {
						for (var j = 0; j < size; j++) {
							var n3 = g3.get (i, j, 0);
							n3 = n3 > 0.5 ? n3 : 0.0f;

							var noise = (g1.get (i, j, 0) + g2.get (i, j, 0)) / 2.0f + (n3 > 0 ? n3 : 0.0f) * 1.0f;
							var height = (int)Mathf.Floor(noise * heightDiff) + 3;

							var coord = new [] { 0, 0, 0 };
							var startD = side * (size - 1);
							coord [d] = startD;
							coord [u] = i;
							coord [v] = j;

							for (var k = 0; k < height; k++) {
								coord [d] = startD + k * dir;
							
								var dis = new Vector3(coord[0], coord[1], coord[2]) - center;
								var max = Math.Max (Math.Abs(dis.x), Math.Max (Math.Abs(dis.y), Math.Abs(dis.z)));
								if (max <= seaLevel) {
									SetVoxel (coord [0], coord [1], coord [2], 
										new TerrianBlock (new Vector3i(coord[0], coord[1], coord[2]), TerrianBlockType.Water));
								}else {
									SetVoxel (coord [0], coord [1], coord [2], null);
								}
							}
						}
					}
				}
			}
		}

		private void generateBiomes() {
			var g1 = new Noise ();
			g1.frequency = 0.05f;
			var g2 = new Noise ();
			g2.frequency = g1.frequency * 2;

			for (var i = 0; i < size; i++) {
				for (var j = 0; j < size; j++) {
					for (var k = 0; k < size; k++) {
						if (!HasVoxel (i, j, k)) {
							continue;
						}

						var noise = g1.get (i, j, k) + g2.get (i, j, k) * 0.5f;
						noise /= 1.5f;

						if (noise > 0.5) {
							var coord = new Vector3i (i, j, k);
							SetVoxel (i, j, k, new TerrianBlock (coord, TerrianBlockType.Stone));
						}
					}
				}
			}
		}

		private void initConnections() {
			foreach (var coord in map.Keys) {
				var block1 = map [coord];

				foreach(var surface1 in block1.surfaceMap.Values) {
					var surfaceConnections = new Dictionary<string, float> ();

					for (var i = -1; i <= 1; i++) {
						for (var j = -1; j <= 1; j++) {
							for (var k = -1; k <= 1; k++) {
								
								var coord2 = coord + new Vector3i (i, j, k);
								if (!map.ContainsKey (coord2)) {
									continue;
								}
								var block2 = map [coord2];

								foreach (var surface2 in block2.surfaceMap.Values) {
									if (surface1 == surface2) {
										continue;
									}

									var distance = Vector3.Distance(surface1.pointAbove, surface2.pointAbove);

									if (surface1.dir != surface2.dir &&
										coord != coord2) {
										continue;
									}

									if (distance >= 2) {
										continue;
									}

									var connection = new Connection (surface1, surface2);
									connectionLookUp [connection.identifier] = connection;
									surfaceConnections [surface2.identifier] = surface2.DistanceTo (surface1);

									surface1.connections.Add (connection);
								}
							}
						}
					}

					graph.add_vertex (surface1.identifier, surfaceConnections);
				}
			}
		}

		public Surface GetSurface(Vector3i coordBelow, Vector3i coordAbove) {
			if (!map.ContainsKey (coordBelow)) {
				return null;
			}

			var block = map [coordBelow];

			var dir = DirUtils.GetDir (coordAbove - coordBelow);

			if (dir == Dir.None) {
				return null;
			}

			if (!block.surfaceMap.ContainsKey (dir)) {
				return null;
			}

			return block.surfaceMap [dir];
		}

		public Connection ConnectionBetween(Surface a, Surface b) {
			var identifier = Connection.IdentifierForSurfaces (a, b);
			if (connectionLookUp.ContainsKey (identifier)) {
				return connectionLookUp [identifier];
			}
			return null;
		}

		public Path GetPath(Surface a, Surface b, int maxStep) {
			return GetPath (a.identifier, b.identifier, maxStep);
		}

		public Path GetPath(string a, string b, int maxStep) {
			return graph.shortest_path (a, b, maxStep);
		}

		public Surface GetSurface(string identifier) {
			return surfaceLookUp [identifier];
		}

		#region PathFindingHeruistics implementation

		public float DistanceBetweenNodes (string a, string b)
		{			
			var surface1 = surfaceLookUp [a];
			var surface2 = surfaceLookUp [b];

			return surface1.DistanceTo (surface2);
		}

		public float CostToEnter(string a) {
			var surface1 = surfaceLookUp [a];

			return (surface1.hasObject || surface1.isWater) ? maxDistanceBetweenSurfaces : 0;
		}

		#endregion
	}
}

