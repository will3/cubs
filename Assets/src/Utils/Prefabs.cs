using System;
using UnityEngine;

namespace AssemblyCSharp
{
	public static class Prefabs
	{
		public static string Spawner = "Spawner";
		public static string Critter = "Critter";
		public static string Turrent = "Turrent";
		public static string Bullet = "Bullet";

		public static GameObject Create(string name) {
			var resourcePath = "Prefabs/" + name;
			GameObject obj = GameObject.Instantiate(Resources.Load(resourcePath)) as GameObject;
			return obj;
		}
	}
}

