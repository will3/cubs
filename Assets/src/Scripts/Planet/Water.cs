using System;
using UnityEngine;
using Cubiquity;
using System.Collections.Generic;

namespace AssemblyCSharp {
	public class WaterPoint {
		public readonly Vector3 worldPosition;
		public readonly Vector3 normal;
		public readonly List<Vertice> vertices = new List<Vertice>();

		public WaterPoint(Vector3 worldPosition, Vector3 normal) {
			this.worldPosition = worldPosition;
			this.normal = normal;
		}
	}

	public class WaterMap {
		public readonly Dictionary<Vector3, WaterPoint> points = new Dictionary<Vector3, WaterPoint>();

		public void AddVertice(Chunk chunk, Vertice vertice) {
			var terrian = Game.Instance.Terrian;
			var objPosition = chunk.transparentObj.obj.transform.position;
			var worldPosition = chunk.transparentObj.obj.transform
				.TransformPoint (vertice.position);

			if (!points.ContainsKey (worldPosition)) {
				var gravity = terrian.GetGravityDir (worldPosition);
				points [worldPosition] = new WaterPoint (worldPosition, gravity);
			}

			points [worldPosition].vertices.Add (vertice);
		}
	}

	public class Water : MonoBehaviour
	{
		private WaterMap waterMap = new WaterMap();

		private Chunks chunks;

		public void Load(Chunks chunks) {
			foreach (var chunk in chunks.chunks.Values) {
				foreach (var vertice in chunk.transparentObj.vertices) {
					waterMap.AddVertice (chunk, vertice);
				}
			}
			this.chunks = chunks;
		}

		void Update() {
			var mag = 0.5f;
			var uniform = -0.5f;
			var verticesByChunksId = new Dictionary<string, Vector3[]> ();
			foreach (var chunk in chunks.chunks.Values) {
				verticesByChunksId [chunk.id] = chunk.transparentObj.obj.GetComponent<MeshFilter> ().mesh.vertices;
			}

			foreach (var point in waterMap.points.Values) {
				var amount = Mathf.Sin (Time.time * 2.0f + point.worldPosition.x + point.worldPosition.y + point.worldPosition.z) * mag + uniform;
				var offset = point.normal * amount;

				foreach (var vertice in point.vertices) {
					var vertices = verticesByChunksId [vertice.chunkId];
					var nextPosition = vertice.position + offset;
					vertices [vertice.index] = nextPosition;
				}
			}

			foreach (var chunk in chunks.chunks.Values) {
				chunk.transparentObj.obj.GetComponent<MeshFilter> ().mesh.vertices = verticesByChunksId [chunk.id];
			}

//			foreach (var point in waterMap.points.Values) {
//				var a = Game.Instance.planetController.gameObject.transform.TransformPoint (point.worldPosition);
//				var b = Game.Instance.planetController.gameObject.transform.TransformPoint (point.worldPosition + point.normal * 0.5f);
//
//				Debug.DrawLine (a, b);
//			}
		}
	}
}

