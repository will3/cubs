using System;
using System.Collections.Generic;

namespace Dijkstras
{
	interface PathFindingHeruistics {
		float DistanceBetweenNodes(string a, string b);
	}

	public class Path {
		// Destination of path
		public readonly string destination;
		private readonly List<string> path;

		public Path(List<string> path, string destination) {
			this.path = path;
			this.destination = destination;
		}

		// True if path leads to destination, false if partial path
		public bool LeadsToDestination {
			get {
				if (path.Count == 0) {
					return false;
				}
				return path [path.Count - 1] == destination;
			}
		}

		// Remove all waypoints except next one
		public void Stop() {
			path.RemoveRange (1, path.Count - 1);
		}

		// Return true if no more points to reach
		public bool Empty {
			get {
				return NextTo ? path.Count == 1 : path.Count == 0;
			}
		}

		// Return next point
		public string Next {
			get {
				return path [0];
			}
		}

		// Call this when reaching a point
		public void RemoveOne() {
			path.RemoveAt (0);
		}

		public bool NextTo = false;
	}

	class Graph
	{
		Dictionary<string, Dictionary<string, float>> vertices = new Dictionary<string, Dictionary<string, float>>();

		public PathFindingHeruistics herusitics;
		public float herusticsFactor = 10.0f;

		public void add_vertex(string name, Dictionary<string, float> edges)
		{
			vertices[name] = edges;
		}

		class PathFindingCache {
			internal Dictionary<string, string> previous = new Dictionary<string, string>();
			Dictionary<string, float> distances = new Dictionary<string, float>();
			internal List<string> nodes = new List<string> ();

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
			var cache = new PathFindingCache ();

			List<string> path = new List<string> ();

			cache.setDistance (start, 0);

			var step = 0;
			var nodesCount = 0;
			var maxNodesCount = vertices.Count;
			while (nodesCount < maxNodesCount)
			{
				step++;
				if (step > maxStep) {
					if (path.Count > 0) {
						path.RemoveAt (0);
					}
					return new Path (path, finish);
				}
		

				var smallest = cache.nodes[nodesCount];
				nodesCount++;

				if (smallest == finish)
				{
					while (cache.previous.ContainsKey(smallest))
					{
						path.Add(smallest);
						smallest = cache.previous[smallest];
					}

					break;
				}

				if (cache.getDistance(smallest) == int.MaxValue)
				{
					break;
				}

				foreach (var neighbor in vertices[smallest])
				{
					var alt = cache.getDistance(smallest) + neighbor.Value + getHerustics(smallest, neighbor.Key, finish);
					if (alt < cache.getDistance(neighbor.Key))
					{
						cache.setDistance (neighbor.Key, alt);
						cache.previous[neighbor.Key] = smallest;
					}
				}
			}
				
			path.Add (finish);

			if (path.Count > 0) {
				path.RemoveAt (0);
			}
			return new Path (path, finish);
		}

		private float getHerustics(string a, string b, string finish) {
			var distance2 = herusitics.DistanceBetweenNodes (finish, b);
			var distance1 = herusitics.DistanceBetweenNodes (finish, a);

			return (distance2 - distance1) * herusticsFactor;
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