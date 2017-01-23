using System;
using UnityEngine;
using System.Linq;

namespace AssemblyCSharp
{
	public class EvilGate : MonoBehaviour, IBlock
	{
		private readonly BlockCoord _blockCoord = new BlockCoord();

		private Billboard billboard;

		private Cooldown spawnCooldown = new Cooldown(2.0f);

		#region IBlock implementation

		public BlockCoord blockCoord {
			get {
				return _blockCoord;
			}
		}

		#endregion

		void Start() {
			if (blockCoord.surface == null) {
				Destroy (gameObject);
			}
			billboard = GetComponentInChildren<Billboard> ();
			Debug.Assert (billboard != null);
		}

		void Update () {
			spawnCooldown.Update ();
			billboard.up = Game.Instance.Planet.gameObject.transform.TransformDirection (blockCoord.surface.normal);

			var planet = Game.Instance.Planet;
			var connectionMap = blockCoord.surface.connectionMap;
			if (connectionMap.Count > 0) {
				var index = (int)Math.Floor (UnityEngine.Random.Range (0.0f, connectionMap.Count));
				var key = connectionMap.Keys.ToList () [index];
				var otherSurface = planet.Terrian.AllSurfaces [key];
				if (spawnCooldown.Ready ()) {
					planet.Create (Prefabs.Spider, otherSurface);
				}
			}
		}
	}
}

