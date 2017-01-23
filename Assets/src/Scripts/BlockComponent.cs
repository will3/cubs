using System;
using UnityEngine;

namespace AssemblyCSharp
{
	public class BlockComponent : MonoBehaviour, IBlock
	{
		private readonly BlockCoord _blockCoord = new BlockCoord();

		public BlockCoord blockCoord {
			get {
				return _blockCoord;
			}
		}
			
		void Start() {
			if (blockCoord.surface == null) {
				gameObject.SetActive (false);
				Destroy (gameObject);
			}
		}

		void Update() {
		}
	}
}

