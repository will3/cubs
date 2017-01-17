using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dijkstras
{
	interface PathFindingHeruistics {
		float DistanceBetweenNodes(string a, string b);
	}

	public class Path {
		public readonly List<string> path;

		public Path(List<string> path) {
			this.path = path;
		}
	}

	class Graph
	{
		Dictionary<string, Dictionary<string, float>> vertices = new Dictionary<string, Dictionary<string, float>>();

		public PathFindingHeruistics herusitics;
		public float herusticsFactor = 100.0f;

		public void add_vertex(string name, Dictionary<string, float> edges)
		{
			vertices[name] = edges;
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

			internal void sort() {
				nodes.Sort((x, y) => {
					return	
						getDistance(x).CompareTo(getDistance(y));
				});
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
					var distanceFrom = herusitics.DistanceBetweenNodes (smallest, finish);
					cache.setClosest (smallest, distanceFrom);
					var alt = cache.getDistance(smallest) + neighbor.Value + distanceFrom * herusticsFactor;
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
			} else {
				var a = 1;
			}

			return new Path (path);
		}
	}

	class MainClass
	{
		public static void Main(string[] args)
		{
//			Graph g = new Graph();
//			g.add_vertex('A', new Dictionary<char, int>() {{'B', 7}, {'C', 8}});
//			g.add_vertex('B', new Dictionary<char, int>() {{'A', 7}, {'F', 2}});
//			g.add_vertex('C', new Dictionary<char, int>() {{'A', 8}, {'F', 6}, {'G', 4}});
//			g.add_vertex('D', new Dictionary<char, int>() {{'F', 8}});
//			g.add_vertex('E', new Dictionary<char, int>() {{'H', 1}});
//			g.add_vertex('F', new Dictionary<char, int>() {{'B', 2}, {'C', 6}, {'D', 8}, {'G', 9}, {'H', 3}});
//			g.add_vertex('G', new Dictionary<char, int>() {{'C', 4}, {'F', 9}});
//			g.add_vertex('H', new Dictionary<char, int>() {{'E', 1}, {'F', 3}});
//
//			g.shortest_path('A', 'H').ForEach( x => Console.WriteLine(x) );
		}
	}
}