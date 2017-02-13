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
			var search = new CharacterSearch ();
			search.character = character;
			search.maxDistance = character.vision;
			return Characters.Instance.GetTargets (search);
		}
	}
}

