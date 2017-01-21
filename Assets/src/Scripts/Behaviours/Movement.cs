using System;
using Dijkstras;
using System.Collections.Generic;
using UnityEngine;

namespace AssemblyCSharp
{
	public class Movement
	{
		private float stepAmount;
		private float stepRatio;
		private readonly Path currentPath = new Path(new List<string>(), "");
		private readonly Character character;
		private bool walking;
		public bool Walking {
			get { return walking; }
		}

		public Movement(Character character) {
			this.character = character;
		}

		// Update route
		public void Move(Surface target, bool nextTo = false) {
			// If current path moving to destination
			if (currentPath.path.Count > 0 &&
				currentPath.destination == target.identifier) {
				return;
			}

			if (stepRatio == 0.0f) {
				currentPath.path.Clear ();
			} else {
				currentPath.path.RemoveRange (1, currentPath.path.Count - 1);
			}

			var startPoint = currentPath.path.Count == 1 ? 
				currentPath.path [0] : 
				character.blockCoord.currentSurface.identifier;

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

			currentPath.isNextTo = nextTo && 
				currentPath.path.Count > 0 && 
				currentPath.path[currentPath.path.Count - 1] == target.identifier;
		}

		public bool Done {
			get {
				return currentPath.path.Count == 0 || currentPath.path.Count == 1 && currentPath.isNextTo;
			}
		}
			
		public void StepPath() {
			if (character.blockCoord.currentSurface != null) {
				DebugUtil.DrawPath (character.blockCoord.currentSurface, currentPath);
			}

			if (Done) {
				walking = false;
				return;
			}

			walking = true;

			var planet = Game.Instance.Planet;
			var nextSurface = planet.Terrian.GetSurface (currentPath.path[0]);

			// If next surface has object, reset path
			if (stepRatio == 0.0f && 
				nextSurface.hasObject) {
				walking = false;
				currentPath.path.Clear ();
				return;
			}

			var currentSurface = character.blockCoord.currentSurface;

			var distance = nextSurface.DistanceTo (currentSurface);
			stepAmount += character.speed;

			var ratio = stepAmount / distance;

			if (ratio > 1.0f) {
				ratio = 1.0f;
				stepAmount -= distance;
			}

			var a = character.transform.position;
			planet.LerpSurface (character, character.gameObject, currentSurface, nextSurface, ratio);
			var b = character.transform.position;

			character.moveDirection = (b - a).normalized;

			if (ratio == 1.0f) {
				currentPath.path.RemoveAt (0);
				ratio = 0.0f;
			}

			stepRatio = ratio;
		}

		public void Patrol() {
			var planet = Game.Instance.Planet;
			var currentSurface = character.blockCoord.currentSurface;

			if (Done) {
				var target = planet.RandomSurface (currentSurface, 4);

				if (target.identifier.Equals (currentSurface.identifier)) {
					return;
				}

				Move (target);
			}
		}
	}
}

