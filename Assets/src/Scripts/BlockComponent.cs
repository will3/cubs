using System;
using UnityEngine;

namespace AssemblyCSharp
{
	public class BlockComponent : MonoBehaviour
	{
		public Surface surface;
		public Vector2 uv;
			
		void Start() {
			if (surface == null) {
				gameObject.SetActive (false);
				Destroy (gameObject);
			}
		}

		void Update() {
		}
	}
}

