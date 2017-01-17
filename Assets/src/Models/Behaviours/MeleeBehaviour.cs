using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Dijkstras;

namespace AssemblyCSharp
{
	public class MeleeBehaviour : MonoBehaviour, ICharacterBehaviour
	{
		Character character;
		private float stepRatio = 0.0f;
		private readonly Path currentPath = new Path(new List<string>());
		private bool pathNextTo = false;
		private float stepAmount;

		public void Start() {
			character = GetComponent<Character> ();
			Debug.Assert (character != null);
		}

		public void Update() {
			stepPath ();

			if (character.CurrentSurface != null) {
				DebugUtil.DrawPath (character.CurrentSurface, currentPath);
			}
		}

		public void Idle ()
		{
			// Play idle animation
		}

		// Update route
		private void Move(Surface target, bool nextTo = false) {
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

			var planet = Game.Instance.Planet;

			var p = planet.Terrian.GetPath (startPoint, target.identifier, character.maxPathFindingSteps);

			// Append path
			currentPath.path.AddRange (p.path);

			pathNextTo = nextTo;
		}

		private void MoveNextTo(Surface target) {
			Move (target, true);
		}

		public void Patrol ()
		{
			var planet = Game.Instance.Planet;
			var currentSurface = character.CurrentSurface;

			if (currentPath.path.Count == 0) {
				var target = planet.RandomSurface (currentSurface, 4);

				if (target.identifier.Equals (currentSurface.identifier)) {
					return;
				}

				Move (target);
			}
		}

		public bool Chase (Character targetCharacter)
		{
			var planet = Game.Instance.Planet;

			var target = targetCharacter.CurrentSurface;

			MoveNextTo (target);

			// If next to target
			if (currentPath.path.Count == 0) {
				var connection = planet.Terrian.ConnectionBetween (character.CurrentSurface, target);
			
				if (connection != null) {
					return true;
				}
			}

			return false;
		}

		public bool Attack (Character character)
		{
			return false;
		}

		public Character FindTarget ()
		{
			return GameObject.FindObjectsOfType<Character> ()
				.Where (u => {
					if (u.NotPlaced) {
						return false;
					}
					if (u == character) {
						return false;
					}

					return u.IsTarget (character);
				})
				.OrderBy (u => (u.gameObject.transform.position - transform.position).sqrMagnitude)
				.FirstOrDefault ();
		}

		private void stepPath() {
			if (currentPath.path.Count == 0) {
				return;
			}

			if (currentPath.path.Count == 1 && pathNextTo) {
				return;
			}

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

			planet.LerpSurface (character, gameObject, currentSurface, nextSurface, ratio);

			if (ratio == 1.0f) {
				currentPath.path.RemoveAt (0);
				ratio = 0.0f;
			}

			stepRatio = ratio;
		}
	}
}

