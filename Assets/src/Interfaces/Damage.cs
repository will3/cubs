using System;

namespace AssemblyCSharp
{
	[Serializable]
	public class Damage
	{
		public Damage() {
		}

		public Damage(float amount, string sourceName) {
			this.amount = amount;
			this.sourceName = sourceName;
		}

		public float amount = 50.0f;

		public string sourceName = "<unnamed>";
	}
}

