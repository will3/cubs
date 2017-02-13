using System;
using UnityEngine;

namespace AssemblyCSharp
{
	public class SlopeLoader : MonoBehaviour
	{
		void Start() {
			var meshFilter = GetComponent<MeshFilter> ();
			var slope = new Slope ();
			meshFilter.sharedMesh = slope.Mesh ();
		}

		void Update() {
			transform.rotation *= Quaternion.Euler (0, 5, 0);
		}
	}
}

