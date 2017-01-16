using System;
using UnityEngine;

namespace AssemblyCSharp
{
	public static class Prefabs
	{
		public static string Spawner = "Spawner";
		public static string Turrent = "Turrent";
		public static string Bullet = "Bullet";
		public static string Spider = "spider/spider";

		public static GameObject Create(string name) {
			var resourcePath = "Prefabs/" + name;
			var res = Resources.Load (resourcePath, typeof(GameObject));
			var obj = GameObject.Instantiate (res) as GameObject;

			if (obj == null) {
				throw new Exception (String.Format ("Prefab named '{0}' does not exist", name));
			}

			return obj;
		}
	}
}

