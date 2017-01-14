using System;
using System.Collections.Generic;

namespace Dijkstras
{
	interface PathFindingHeruistics {
		float DistanceBetweenNodes(string a, string b);
	}

	public class Path {
		public readonly List<string> path;
		public readonly bool finished;

		public Path(List<string> path, bool finished) {
			this.path = path;
			this.finished = finished;
		}
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

		public Path shortest_path(string start, string finish, int maxStep = 64)
		{
			var previous = new Dictionary<string, string>();
			var distances = new Dictionary<string, float>();
			var nodes = new List<string>();

			List<string> path = null;

			foreach (var vertex in vertices)
			{
				if (vertex.Key == start)
				{
					distances[vertex.Key] = 0;
				}
				else
				{
					distances[vertex.Key] = int.MaxValue;
				}

				nodes.Add(vertex.Key);
			}

			var step = 0;
			var nodesCount = 0;
			while (nodesCount < nodes.Count)
			{
				step++;
				if (step > maxStep) {
					return new Path (path, false);
				}
				nodes.Sort((x, y) => {
					return	distances[x].CompareTo(distances[y]);
				});

				var smallest = nodes[nodesCount];
				nodesCount++;

				if (smallest == finish)
				{
					path = new List<string>();
					while (previous.ContainsKey(smallest))
					{
						path.Add(smallest);
						smallest = previous[smallest];
					}

					break;
				}

				if (distances[smallest] == int.MaxValue)
				{
					break;
				}

				foreach (var neighbor in vertices[smallest])
				{
					var alt = distances[smallest] + neighbor.Value + getHerustics(smallest, neighbor.Key, finish);
					if (alt < distances[neighbor.Key])
					{
						distances[neighbor.Key] = alt;
						previous[neighbor.Key] = smallest;
					}
				}
			}

			return new Path (path, true);
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