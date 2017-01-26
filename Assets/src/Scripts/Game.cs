using System;
using UnityEngine;

namespace AssemblyCSharp
{
	public class Game : MonoBehaviour
	{
		public Planet Planet;
		public Terrian Terrian;
		public PlanetController planetController;
		public PlaceBuilding placeBuilding;

		private static Game _instance;
		public static Game Instance {
			get {
				return _instance;
			}
		}

		void Awake() {
			_instance = this;
		}

		private DragCamera dragCamera;

		void Start() {
			dragCamera = Camera.main.GetComponent<DragCamera> ();
			Debug.Assert (dragCamera);
		}

		void Update() {
			var zoomRate = 1.1f;
			if (Input.GetKeyDown (KeyCode.Equals)) {
				dragCamera.distance /= zoomRate; 
			}

			if (Input.GetKeyDown (KeyCode.Minus)) {
				dragCamera.distance *= zoomRate;
			}
		}
	}
}

