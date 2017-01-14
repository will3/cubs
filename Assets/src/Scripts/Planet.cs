using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cubiquity;
using AssemblyCSharp;
using System;

public class Planet : MonoBehaviour {
	public int size = 16;
	public float heightDiff = 4.0f;

	private Terrian _terrian;
	public Terrian Terrian {
		get { return _terrian; }
	}

	private bool drawNormals = false;
	private bool drawConnections = false;

	private ColoredCubesVolume volume;

	private ColoredCubesVolumeCollider volumeCollider;
	private ColoredCubesVolumeRenderer volumeRenderer;

	public class BlockPrefab {
		public readonly GameObject gameObject;
		public readonly BlockComponent blockComponent;

		public BlockPrefab(GameObject gameObject, BlockComponent blockComponent) {
			this.gameObject = gameObject;
			this.blockComponent = blockComponent;
		}
	}

	public BlockPrefab Create(string name, Surface surface) {
		var resourcePath = "Prefabs/" + name;
		GameObject obj = Instantiate(Resources.Load(resourcePath)) as GameObject;
		obj.transform.parent = gameObject.transform.parent;

		BlockComponent blockComponent;

		if (name.Equals (BlockPrefabType.Critter)) {
			blockComponent = obj.GetComponent<Critter> ();
		} else if (name.Equals (BlockPrefabType.Spawner)) {
			blockComponent = obj.GetComponent<Spawner> ();
		} else if (name.Equals (BlockPrefabType.Turrent)) {
			blockComponent = obj.GetComponent<Turrent> ();
		} else {
			throw new Exception ("Invalid component " + name);
		}

		SetSurface (blockComponent, surface);

		var prefab = new BlockPrefab (obj, blockComponent);
		blockComponent.currentSurface = surface;
		return prefab;
	}

	// Use this for initialization
	void Start () {
		_terrian = new Terrian (size, heightDiff);
		Game.Instance.Planet = this;

		volume = gameObject.GetComponent<ColoredCubesVolume> ();
		if (volume == null) {
			Debug.LogError ("Component 'Planet' requires 'ColoredCubesVolume'");
		}

		volumeCollider = gameObject.GetComponent<ColoredCubesVolumeCollider> ();
		if (volumeCollider == null) {
			Debug.LogError ("Component 'Planet' requires 'ColoredCubesVolumeCollider'");
		}

		volumeRenderer = gameObject.GetComponent<ColoredCubesVolumeRenderer> ();
		if (volumeRenderer == null) {
			Debug.LogError ("Component 'Planet' requires 'ColoredCubesVolumeRenderer'");
		}

		var center = new Vector3 (-size / 2, -size / 2, -size / 2) + (new Vector3() * 0.5f);
		gameObject.transform.position = center;

		ColoredCubesVolumeData data = VolumeData.CreateEmptyVolumeData<ColoredCubesVolumeData>(new Region(0, 0, 0, size, size, size));
		volume.data = data;

		Terrian.Init ();

		loadData ();
	}

	private void loadData() {
		var data = volume.data;

		foreach (var kv in Terrian.map) {
			var coord = kv.Key;
			var block = kv.Value;
			var color = block.GetColor ();
			data.SetVoxel (coord.x, coord.y, coord.z, new QuantizedColor (
				(byte)(color.r * 255), 
				(byte)(color.g * 255), 
				(byte)(color.b * 255), 
				(byte)(color.a * 255)
			));
		}
	}

	void Update () {
		if (drawNormals) {
			foreach (var block in Terrian.map.Values) {
				foreach (var surface in block.surfaceMap.Values) {
					Debug.DrawLine (
						gameObject.transform.TransformPoint(surface.point),
						gameObject.transform.TransformPoint(surface.pointAbove),
						Color.red);
				}
			}
		}

		if (drawConnections) {
			foreach (var connection in Terrian.connectionLookUp.Values) {
				var point1 = gameObject.transform.TransformPoint (connection.a.pointAbove);
				var point2 = gameObject.transform.TransformPoint (connection.b.pointAbove);
				Debug.DrawLine (point1, point2, Color.red);
			}
		}

		if (Input.GetKeyDown (KeyCode.BackQuote)) {
			volumeRenderer.enabled = !volumeRenderer.enabled;
		}

		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			drawNormals = !drawNormals;
		}

		if (Input.GetKeyDown (KeyCode.Alpha2)) {
			drawConnections = !drawConnections;
		}
	}
		
	public bool CanSetSurface(BlockComponent blockComponent, Surface surface) {
		return !surface.hasObject;
	}

	public bool SetSurface(BlockComponent blockComponent, Surface surface) {
		if (surface.hasObject) {
			return false;
		}

		if (blockComponent.currentSurface != null) {
			blockComponent.currentSurface.blockComponent = null;
		}

		var position = gameObject.transform.TransformPoint (surface.pointAbove);
		blockComponent.transform.position = position;
		blockComponent.currentSurface = surface;
		surface.blockComponent = blockComponent;

		blockComponent.transform.localRotation = surface.rotation;

		return true;
	}

	public Connection RandomConnection(string surfaceIdentifier) {
		var connections = Terrian.connectionBySurfaceIdentifier [surfaceIdentifier];
		var index = UnityEngine.Random.Range (0, connections.Count - 1);
		return connections [index];	
	}

	public Surface RandomSurface(Surface surface, int num = 1) {
		var identifier = surface.identifier;

		for (var i = 0; i < num; i++) {
			var connection = RandomConnection (identifier);
			identifier = connection.OtherSurface (identifier).identifier;
		}

		return Terrian.surfaceByIdentifier [identifier];
	}

	// Ratio should be more than 0 and less than or equal to 1
	public bool LerpSurface(BlockComponent blockComponent, Surface surface1, Surface surface2, float ratio) {
		if (surface1.identifier.Equals (surface2.identifier)) {
			throw new Exception ("Invalid surface");
		}
		if (blockComponent.currentSurface != null) {
			blockComponent.currentSurface.blockComponent = null;
		}

		var position1 = surface1.pointAbove;
		var position2 = surface2.pointAbove;

		var position = position1 + (position2 - position1) * ratio;

		blockComponent.transform.position = gameObject.transform.TransformPoint (position);
		Debug.Log (blockComponent.transform.position);
		Debug.Log (ratio);

		if (ratio == 1.0f) {
			blockComponent.currentSurface = surface2;
		}

		surface2.blockComponent = blockComponent;

		blockComponent.transform.localRotation = surface2.rotation;

		return true;
	}

	public Surface GetSurface() {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		return GetSurface (ray);
	}

	public Surface GetSurface(Ray ray) {
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit)) {
			var diff = hit.point - ray.origin;

			var pointAbove = hit.point - diff.normalized * 0.01f;
			pointAbove = transform.InverseTransformPoint (pointAbove);

			var coordAbove = new Vector3i (
				(int)Mathf.Round (pointAbove.x),
				(int)Mathf.Round (pointAbove.y),
				(int)Mathf.Round (pointAbove.z));

			var pointBelow = hit.point + diff.normalized * 0.01f;
			pointBelow = transform.InverseTransformPoint (pointBelow);

			var coordBelow = new Vector3i (
				(int)Mathf.Round (pointBelow.x),
				(int)Mathf.Round (pointBelow.y),
				(int)Mathf.Round (pointBelow.z));

			return Terrian.GetSurface (coordBelow, coordAbove);
		}

		return null;
	}
}
