using System;
using System.Collections.Generic;
using UnityEngine;

namespace AssemblyCSharp
{
	public class MeleeBehaviour : ICharacterBehaviour
	{
		GameObject obj;
		Block block;
		Character character;

		public MeleeBehaviour (
			Block block, 
			Character character, 
			GameObject obj)
		{
			this.block = block;
			this.obj = obj;
			this.character = character;
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

			if (currentPath == null || currentPath.Count == 0) {

				var target = planet.RandomSurface (currentSurface, 6);

				if (target.identifier.Equals (currentSurface.identifier)) {
					return;
				}

				var p = planet.Terrian.GetPath (target, currentSurface);

				if (p.path == null) {
					throw new Exception ("path finding failed for patrol point");
				}

				var path = p.path;

				if (path.Count > 0) {
					path.RemoveAt (0);
				}

				if (p.finished) {
					path.Add (target.identifier);
				}

				currentPath = path;
			}

			if (currentPath != null && currentPath.Count > 0) {
				
				var nextSurface = planet.Terrian.surfaceByIdentifier [currentPath [0]];

				var distance = nextSurface.DistanceTo (currentSurface);
				stepAmount += character.speed;

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

		public bool Chase (Character character)
		{
			throw new NotImplementedException ();
		}

		public bool Attack (Character character)
		{
			throw new NotImplementedException ();
		}

		public Character FindTarget ()
		{
			return null;
		}
	}
}

