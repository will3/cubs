using System;
using Dijkstras;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	public class Movement
	{
		private float stepAmount;
		private float stepRatio;
		private readonly Path currentPath = new Path(new List<string>());
		private readonly Character character;
		private bool walking;
		public bool Walking {
			get { return walking; }
		}

		public Movement(Character character) {
			this.character = character;
		}

		public Path CurrentPath {
			get {
				return currentPath;
			}
		}

		// Update route
		public void Move(Surface target, bool nextTo = false) {
			// If current path moving to destination
			if (currentPath.path.Count > 0 &&
				currentPath.path [currentPath.path.Count - 1] == target.identifier) {
				return;
			}

			if (stepRatio == 0.0f) {
				currentPath.path.Clear ();
			} else {
				currentPath.path.RemoveRange (1, currentPath.path.Count - 1);
			}

			var startPoint = currentPath.path.Count == 1 ? 
				currentPath.path [0] : 
				character.CurrentSurface.identifier;

			// Already at destination
			if (startPoint == target.identifier) {
				return;
			}

			var planet = Game.Instance.Planet;

			if (startPoint == target.identifier) {
				throw new Exception ("start point and target are the same!");
			}

			var p = planet.Terrian.GetPath (startPoint, target.identifier, character.maxPathFindingSteps);

			// Append path
			currentPath.path.AddRange (p.path);
			currentPath.isNextTo = nextTo;
		}

		public void StepPath() {
			if (currentPath.path.Count == 0) {
				walking = false;
				return;
			}

			if (currentPath.path.Count == 1 && currentPath.isNextTo) {
				walking = false;
				return;
			}

			walking = true;

			var planet = Game.Instance.Planet;
			var currentSurface = character.CurrentSurface;

			var nextSurface = planet.Terrian.GetSurface (currentPath.path[0]);

			var distance = nextSurface.DistanceTo (currentSurface);
			stepAmount += character.speed;

			var ratio = stepAmount / distance;

			if (ratio > 1.0f) {
				ratio = 1.0f;
				stepAmount -= distance;
			}

			planet.LerpSurface (character, character.gameObject, currentSurface, nextSurface, ratio);

			if (ratio == 1.0f) {
				currentPath.path.RemoveAt (0);
				ratio = 0.0f;
			}

			stepRatio = ratio;
		}
	}
}

