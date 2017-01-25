using System;
using UnityEngine;

namespace AssemblyCSharp
{
	public class Cooldown
	{
		private float max;
		private float counter = 0;

		public Cooldown (float max, float counter = 0)
		{
			this.max = max;
			this.counter = counter;
		}

		public void Update() {
			if (counter > max) {
				counter = 0;
			}
				
			counter += Time.deltaTime;
		}

		public bool Ready() {
			if (counter >= max) {
				counter -= max;
				return true;
			}

			return false;
		}

		public void SetRandom() {
			counter = UnityEngine.Random.Range (0.0f, max);
		}
	}
}

