using System;
using UnityEngine;
using Cubiquity;
using System.Collections.Generic;

namespace AssemblyCSharp {
	public class WaterPoint {
		public readonly Vector3 worldPosition;
		public readonly Vector3 normal;
		public readonly List<Vertice> vertices = new List<Vertice>();
		public readonly float gravityLength;

		public WaterPoint(Vector3 worldPosition, Vector3 normal, float gravityLength) {
			this.worldPosition = worldPosition;
			this.normal = normal;
			this.gravityLength = gravityLength;
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
				var gravityVector = terrian.GetGravityVector (worldPosition, 1.0f);
				var gravity = gravityVector.normalized;
				points [worldPosition] = new WaterPoint (worldPosition, gravity, gravityVector.magnitude);
			}

			points [worldPosition].vertices.Add (vertice);
		}
	}

	public class Water : MonoBehaviour
	{
		public bool showWaterNormals = false;
		public float waveMag = 0.2f;
		public float waveOffset = -0.4f;
		public float waveFrequency = 2.0f;

		private WaterMap waterMap = new WaterMap();

		private Chunks chunks;

		private Cooldown updateCooldown = new Cooldown (0.1f);

		public void Load(Chunks chunks) {
			foreach (var chunk in chunks.chunks.Values) {
				foreach (var vertice in chunk.transparentObj.vertices) {
					waterMap.AddVertice (chunk, vertice);
				}
			}
			this.chunks = chunks;
		}

		void Start() {
			UpdateWater (false);
		}

		void Update() {
			return;
			updateCooldown.Update ();

			if (updateCooldown.Ready ()) {
				UpdateWater (true);
			}
				
			if (showWaterNormals) {
				foreach (var point in waterMap.points.Values) {
					var a = Game.Instance.planetController.gameObject.transform.TransformPoint (point.worldPosition);
					var b = Game.Instance.planetController.gameObject.transform.TransformPoint (point.worldPosition + point.normal * 0.5f);

					Debug.DrawLine (a, b);
				}
			}
		}

		private void UpdateWater(bool wave) {
			var verticesByChunksId = new Dictionary<string, Vector3[]> ();
			foreach (var chunk in chunks.chunks.Values) {
				var mesh = chunk.transparentObj.obj.GetComponent<MeshFilter> ().sharedMesh;
				verticesByChunksId [chunk.id] = mesh.vertices;
			}

			foreach (var point in waterMap.points.Values) {
				var amount = wave ?
					Mathf.Sin (Time.time * waveFrequency + point.worldPosition.x + point.worldPosition.y + point.worldPosition.z) * waveMag + waveOffset :
					waveOffset;
				amount *= point.gravityLength;
				var offset = point.normal * amount;

				foreach (var vertice in point.vertices) {
					var vertices = verticesByChunksId [vertice.chunkId];
					var nextPosition = vertice.position + offset;
					vertices [vertice.index] = nextPosition;
				}
			}

			foreach (var chunk in chunks.chunks.Values) {
				chunk.transparentObj.obj.GetComponent<MeshFilter> ().sharedMesh.vertices = verticesByChunksId [chunk.id];
			}
		}
	}
}

