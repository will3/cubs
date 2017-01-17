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
		private Path currentPath;
		private float stepAmount;

		public void Start() {
			character = GetComponent<Character> ();
			Debug.Assert (character != null);
		}

		public void Update() {
			stepPath ();
		}

		public void Idle ()
		{
			// Play idle animation
		}

		private void Move(Surface target, bool nextTo = false) {
			if (currentPath != null) {
				currentPath.NextTo = nextTo;
			}
				
			// Path stay true, do nothing
			if (currentPath != null && target.identifier == currentPath.destination) {
				return;
			}

			// If moving to wrong point, stop first
			if (stepRatio != 0.0f) {
				currentPath.Stop ();
				return;
			}

			// At this point, path should be empty
			Debug.Assert (currentPath == null || currentPath.Empty);

			var planet = Game.Instance.Planet;

			var currentSurface = character.CurrentSurface;

			currentPath = planet.Terrian.GetPath (currentSurface, target, character.maxPathFindingSteps);

			currentPath.NextTo = nextTo;
		}

		private void MoveNextTo(Surface target) {
			Move (target, true);
		}

		public void Patrol ()
		{
			var planet = Game.Instance.Planet;
			var currentSurface = character.CurrentSurface;

			if (currentPath == null || currentPath.Empty) {
				var target = planet.RandomSurface (currentSurface, 6);

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
			if (currentPath.Empty) {
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
			if (currentPath == null || currentPath.Empty) {
				return;
			}

			var planet = Game.Instance.Planet;
			var currentSurface = character.CurrentSurface;
				
			var nextSurface = planet.Terrian.GetSurface (currentPath.Next);

			var distance = nextSurface.DistanceTo (currentSurface);
			stepAmount += character.speed;

			var ratio = stepAmount / distance;

			if (ratio > 1.0f) {
				ratio = 1.0f;
				stepAmount -= distance;
			}

			planet.LerpSurface (character, gameObject, currentSurface, nextSurface, ratio);

			if (ratio == 1.0f) {
				currentPath.RemoveOne ();
				ratio = 0.0f;
			}

			stepRatio = ratio;
		}
	}
}

