using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dijkstras
{
	interface PathFindingContext {
		float DistanceBetweenNodes(string a, string b);
		float CostToEnter (string a);
	}

	// TODO update destination
	public class Path {
		public readonly List<string> path;
		public bool isNextTo = false;
		public readonly string destination;

		public Path(List<string> path, string destination) {
			this.path = path;
			this.destination = destination;
		}
	}

	class Graph
	{
		Dictionary<string, Dictionary<string, float>> vertices = new Dictionary<string, Dictionary<string, float>>();

		public PathFindingContext context;
		public float distanceFactor = 2.0f;

		public void add_vertex(string name, Dictionary<string, float> edges)
		{
			vertices[name] = edges;
		}

		public void remove_vertex(string name) {
			vertices.Remove (name);
		}

		class PathFindingCache {
			internal Dictionary<string, string> previous = new Dictionary<string, string>();
			Dictionary<string, float> distances = new Dictionary<string, float>();
			internal List<string> nodes = new List<string> ();

			private float closestDistance = int.MaxValue;
			public string closestVertex;

			internal void setClosest(string vertex, float value) {
				if (value < closestDistance) {
					closestDistance = value;
					closestVertex = vertex;
				}
			}

			internal void setDistance(string vertex, float value) {
				distances [vertex] = value;
				nodes.Add(vertex);
			}

			internal float getDistance(string vertex) {
				if (distances.ContainsKey (vertex)) {
					return distances [vertex];
				}
				return int.MaxValue;
			}
		}

		public Path shortest_path(string start, string finish, int maxStep)
		{
			if (start == finish) {
				throw new Exception ("start and finish are the same!");
			}

			var cache = new PathFindingCache ();

			List<string> path = new List<string> ();

			cache.setDistance (start, 0);

			var step = 0;
			var nodesCount = 0;
			var maxNodesCount = vertices.Count;
			string smallest;
			var reached = false;
			while (nodesCount < maxNodesCount)
			{
				step++;	

				smallest = cache.nodes[nodesCount];
				nodesCount++;

				if (step > maxStep) {
					// Incomplete
					var v = cache.closestVertex;

					if (v != null) {
						while (cache.previous.ContainsKey (v)) {
							path.Add (v);
							v = cache.previous [v];
						}	
					}

					break;
				}

				if (smallest == finish)
				{
					while (cache.previous.ContainsKey(smallest))
					{
						path.Add(smallest);
						smallest = cache.previous[smallest];
					}

					reached = true;

					break;
				}

				if (cache.getDistance(smallest) == int.MaxValue)
				{
					break;
				}

				foreach (var neighbor in vertices[smallest])
				{
					var distance = context.DistanceBetweenNodes (smallest, finish) +
					               (smallest == start ? 0 : context.CostToEnter (smallest));
					cache.setClosest (smallest, distance);
					var alt = cache.getDistance(smallest) + neighbor.Value + distance * distanceFactor;
					if (alt < cache.getDistance(neighbor.Key))
					{
						cache.setDistance (neighbor.Key, alt);
						cache.previous[neighbor.Key] = smallest;
					}
				}
			}
				
			path.Reverse ();

			if (reached) {
				Debug.Assert (path [path.Count - 1] == finish);
			}

			return new Path (path, finish);
		}
	}
}