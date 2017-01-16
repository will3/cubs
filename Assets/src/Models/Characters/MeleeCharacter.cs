using System;
using System.Collections.Generic;
using UnityEngine;

namespace AssemblyCSharp
{
	public class MeleeCharacter : ICharacter
	{
		private readonly Traits traits = new Traits();
		public Traits Traits { get { return traits; } }

		GameObject obj;
		Block block;

		public MeleeCharacter (Block block, GameObject obj)
		{
			this.block = block;
			this.obj = obj;
		}

		public void Idle ()
		{
			// Play idle animation
		}

		private List<string> currentPath;
		private float stepAmount;

		public void Patrol ()
		{
			var planet = Game.Instance.Planet;
			var currentSurface = block.currentSurface;

			if (currentPath == null) {

				var target = planet.RandomSurface (currentSurface, 6);

				// Reject target
				if (target.identifier.Equals (currentSurface.identifier)) {
					return;
				}

				var p = planet.Terrian.GetPath (target, currentSurface);

				if (p != null) {
					var path = p.path;

					path.RemoveAt (0);

					if (p.finished) {
						path.Add (target.identifier);
					}

					currentPath = path;
				}
			}

			if (currentPath != null && currentPath.Count > 0) {
				var nextSurface = planet.Terrian.surfaceByIdentifier [currentPath [0]];

				var distance = nextSurface.DistanceTo (currentSurface);
				stepAmount += traits.speed;

				var ratio = stepAmount / distance;

				if (ratio > 1.0f) {
					ratio = 1.0f;
					stepAmount -= distance;
				}

				planet.LerpSurface (this.block, this.obj, currentSurface, nextSurface, ratio);

				if (ratio == 1.0f) {
					currentPath.RemoveAt (0);
					ratio = 0.0f;
				}
			}
		}

		public bool Chase (ICharacter character)
		{
//			var planet = Game.Instance.Planet;
			return false;
		}

		public bool Attack (ICharacter character)
		{
			return false;
		}

		public ICharacter FindTarget ()
		{
			return null;
		}
	}
}

