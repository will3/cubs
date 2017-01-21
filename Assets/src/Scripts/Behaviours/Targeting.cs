using System;
using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	public class Targeting
	{
		Character character;
		public Targeting (Character character)
		{
			this.character = character;
		}

		// Get targets sorted by distance
		public IEnumerable<Character> GetTargets() {
			var maxDisSq = character.vision * character.vision;
			var transform = character.transform;

			return GameObject
				.FindObjectsOfType<Character> ()
				.Where (u => {
				if (u == character) {
					return false;
				}

				if ((u.gameObject.transform.position - transform.position).sqrMagnitude > maxDisSq) {
					return false;
				}

				return u.IsTarget (character);
			})
				.OrderBy (u => (u.gameObject.transform.position - transform.position).sqrMagnitude);
		}
	}
}

