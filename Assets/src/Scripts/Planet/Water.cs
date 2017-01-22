using System;
using UnityEngine;
using Cubiquity;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	public class WaterPoint {
		
	}

	public class Water : MonoBehaviour
	{
		private Dictionary<Vector3, List<Vertice>> map = new Dictionary<Vector3, List<Vertice>>();

		private Chunks waterChunk;

		public void Load(Chunks waterChunk) {
			this.waterChunk = waterChunk;
			var terrian = Game.Instance.Terrian;
			foreach (var chunk in waterChunk.chunks.Values) {
				foreach (var vertice in chunk.vertices) {
//					var coord = new Vector3i (vertice.coord [0], vertice.coord [1], vertice.coord [2]);
//					var block = terrian.map [coord];
//					var dir = (Dir)vertice.f;
//					if (!block.surfaceMap.ContainsKey (dir)) {
//						continue;
//					}
//						
//					if (!map.ContainsKey (vertice.vector)) {
//						map [vertice.vector] = new List<Vertice> ();
//					}
//					map [vertice.vector].Add (vertice);
//					vertice.mesh = chunk.mesh;
				}
			}
		}

		void Update() {
			foreach (var chunk in waterChunk.chunks.Values) {
				var mesh = chunk.mesh;

				var vertices = mesh.vertices;

				for (var i = 0; i < vertices.Length; i++) {
					vertices [i] += UnityEngine.Random.insideUnitSphere;
				}

				mesh.vertices = vertices;
			}
		}
	}
}

