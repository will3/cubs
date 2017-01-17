using System;
using System.Collections.Generic;
using UnityEngine;
using Cubiquity;
using Dijkstras;

namespace AssemblyCSharp
{
	public class Terrian : PathFindingHeruistics
	{
		// Should prob be encapsulated
		public readonly int size;
		public readonly float heightDiff;
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
		private readonly Dictionary<string, List<Connection>> connectionBySurfaceIdentifier = 
			new Dictionary<string, List<Connection>> ();

		private const float maxDistanceBetweenSurfaces = 1000.0f;

		private Graph graph = new Graph ();

		public Terrian (int size, float heightDiff) {
			this.size = size;
			this.heightDiff = heightDiff;
			graph.herusitics = this;
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
					surfaceLookUp [surface.identifier] = surface;
				}
			}
		}

		private void initGravity() {
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

		private void initBlocks() {
			for (int i = 0; i < size; i++) {
				for (int j = 0; j < size; j++) {
					for (int k = 0; k < size; k++) {
						var coord = new Vector3i (i, j, k);
						SetVoxel (i, j, k, new TerrianBlock (coord, BlockType.Grass));
					}
				}
			}
		}

		private void generateHeightMap() {
			for (var d = 0; d < 3; d++) {
				foreach(var side in new [] {0, 1}) {
					var g1 = new Noise ();
					g1.scale = 1.0f / (float)size * 1.0f;

					var g2 = new Noise ();
					g2.scale = 0.05f;

					var dir = side == 0 ? 1 : -1;

					var u = (d + 1) % 3;
					var v = (d + 2) % 3;

					for (var i = 0; i < size; i++) {
						for (var j = 0; j < size; j++) {
							var noise = (g1.get (i, j, 0) + g2.get (i, j, 0));
							var height = (int)Mathf.Floor(noise * heightDiff);

							var coord = new [] { 0, 0, 0 };
							var startD = side * (size - 1);
							coord [d] = startD;
							coord [u] = i;
							coord [v] = j;

							for (var k = 0; k < height; k++) {
								coord [d] = startD + k * dir;
							
								SetVoxel (coord [0], coord [1], coord [2], null);
							}
						}
					}
				}
			}
		}

		private void generateBiomes() {
			var g1 = new Noise ();
			g1.scale = 0.05f;
			var g2 = new Noise ();
			g2.scale = g1.scale * 2;

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
							SetVoxel (i, j, k, new TerrianBlock (coord, BlockType.Stone));
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

									if (!connectionBySurfaceIdentifier.ContainsKey (surface1.identifier)) {
										connectionBySurfaceIdentifier [surface1.identifier] = new List<Connection> ();
									}

									connectionBySurfaceIdentifier [surface1.identifier].Add (connection);
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
			return graph.shortest_path (a.identifier, b.identifier, maxStep);
		}

		public Surface GetSurface(string identifier) {
			return surfaceLookUp [identifier];
		}

		public IList<Connection> GetConnections(string surfaceIdentifier) {
			return connectionBySurfaceIdentifier [surfaceIdentifier];
		}

		#region PathFindingHeruistics implementation

		public float DistanceBetweenNodes (string a, string b)
		{			
			// TODO handle no surface
			var surface1 = surfaceLookUp [a];
			var surface2 = surfaceLookUp [b];

			if (surface1.hasObject || surface2.hasObject) {
				return maxDistanceBetweenSurfaces;
			}

			return surface1.DistanceTo (surface2);
		}

		#endregion
	}
}

