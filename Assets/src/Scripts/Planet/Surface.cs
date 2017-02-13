using System;
using UnityEngine;
using Cubiquity;
using System.Collections.Generic;
using System.Linq;

namespace AssemblyCSharp
{
	public class Surface {
		public readonly Dir dir;
		public readonly Vector3i coord;
		public readonly Vector3i coordAbove;
		public readonly Vector3 normal;
		public readonly string identifier;

		public readonly Vector3 point;
		public readonly Vector3 pointAbove;

		public readonly Quaternion rotation;

		private readonly Vector3[] uvVectors;

		public Vector3 pointWithUV(Vector2 uv) {
			return point + uvVectors [0] * uv.x + uvVectors [1] * uv.y;
		}

		public bool isWater = false;

		public BlockComponent block;

		private readonly Dictionary<string, Connection> _connectionMap = new Dictionary<string, Connection> ();

		public IDictionary<string, Connection> connectionMap {
			get { return _connectionMap; }
		}

		public bool hasObject { get { return block != null; } }

		public Surface(Vector3i coord, Dir dir) {
			this.coord = coord;
			this.dir = dir;
			var vector = DirUtils.GetUnitVector (dir);
			normal = vector.to_f ();
			coordAbove = coord + vector;
			identifier = coord.x + "," + coord.y + "," + coord.z + "," + dir;

			point = new Vector3 (coord.x, coord.y, coord.z) + normal * 0.5f;
			pointAbove = new Vector3 (coordAbove.x, coordAbove.y, coordAbove.z);

			rotation = DirUtils.GetRotation (dir);

			uvVectors = DirUtils.GetUV (dir);
		}
			
		public Connection GetConnection(string surfaceIdentifier) {
			if (!_connectionMap.ContainsKey (surfaceIdentifier)) {
				return null;
			}
			return _connectionMap [surfaceIdentifier];
		}

		public IList<string> ConnectedSurfaceIdentifiers {
			get {
				return _connectionMap.Keys.ToList ();
			}
		}

		public string RandomConnectedSurfaceIdentifier {
			get {
				var keys = _connectionMap.Keys.ToList ();
				if (keys.Count == 0) {
					return null;
				}
				var index = UnityEngine.Random.Range (0, keys.Count - 1);
				return keys [index];
			}
		}

		public void AddConnection(string surfaceIdentifier, Connection connection) {
			_connectionMap [surfaceIdentifier] = connection;
		}

		public void ClearConnections() {
			_connectionMap.Clear ();
		}
	}
}

