using System;
using UnityEngine;

namespace AssemblyCSharp
{
	public static class Prefabs
	{
		public static string Swordsman = "Units/swordsman/swordsman";
		public static string Spider = "Units/spider/spider";
		public static string Archer = "Units/archer/archer";

		public static class Objects
		{
			public static string Arrow = "Objects/arrow/arrow";
		}

		public static GameObject Create(string name) {
			var resourcePath = name;
			var res = Resources.Load (resourcePath, typeof(GameObject));
			var obj = GameObject.Instantiate (res) as GameObject;

			if (obj == null) {
				throw new Exception (String.Format ("Prefab named '{0}' does not exist", name));
			}

			return obj;
		}
	}
}

