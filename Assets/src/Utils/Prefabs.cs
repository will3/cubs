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
			public static class Trees
			{
				public static string OfSize(float size) {
					if (size > 0.8) {
						return "Objects/tree/tree_2";
					} else if (size > 0.4) {
						return "Objects/tree/tree_1";
					} else {
						return "Objects/tree/tree_0";
					}
				}
			}
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

