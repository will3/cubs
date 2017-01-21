﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cubiquity;
using AssemblyCSharp;
using System;

public class Planet : MonoBehaviour {
	public int size = 16;
	public float heightDiff = 2.0f;
	public float trees = 1.0f;

	private Terrian _terrian;
	public Terrian Terrian {
		get { return _terrian; }
	}

	private bool drawNormals = false;
	private bool drawConnections = false;

	private ColoredCubesVolume volume;

	private ColoredCubesVolumeCollider volumeCollider;
	private ColoredCubesVolumeRenderer volumeRenderer;

	public GameObject Create(string name, BlockCoord blockCoord) {
		var obj = Prefabs.Create (name);
		obj.transform.parent = gameObject.transform.parent;
		var block = (IBlock)obj.GetComponent<Character> () ?? obj.GetComponent<Tree>();

		SetSurface (obj, block, blockCoord);

		return obj;
	}

	public GameObject Create(string name, Surface surface) {
		return Create (name, new BlockCoord (surface));
	}

	// Use this for initialization
	void Start () {
		_terrian = new Terrian (size, heightDiff);
		Game.Instance.Planet = this;
		Game.Instance.Terrian = _terrian;

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

		Terrian.Generate ();
		generateTrees ();

		loadData ();

		var dragCamera = Camera.main.GetComponent<DragCamera> ();
		if (dragCamera != null) {
			dragCamera.distance = size * 1.3f;
		}
	}

	private void generateTrees() {
		var g1 = new Noise ();
		g1.scale = 0.05f;
		var g2 = new Noise ();
		g2.scale = g1.scale * 2;
		var g3 = new Noise ();
		g3.scale = 2.0f;

		foreach (var surface in Terrian.AllSurfaces.Values) {
			var coord = surface.coord;
			var noise = 
				g1.get (coord.x, coord.y, coord.z) +
				g2.get (coord.x, coord.y, coord.z) * 0.5f;
			var noise2 = g3.get(coord.x, coord.y, coord.z);
			
			noise /= 1.5f;

			var min1 = 0.5 / trees;
			var min2 = 0.5 / trees;

			var size = UnityEngine.Random.Range(0.1f, 1.0f);

			var uv = new Vector2 (
				         UnityEngine.Random.Range (-0.3f, 0.3f), 
				         UnityEngine.Random.Range (-0.3f, 0.3f));
			if (noise > min1 && noise2 > min2) {
				Create (Prefabs.Objects.Trees.OfSize(size), new BlockCoord(surface, uv));
			}
		}
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
					if (surface.hasObject) {
						continue;
					}
					DebugUtil.DrawSurface (surface);
				}
			}
		}

		if (drawConnections) {
			foreach (var connection in Terrian.AllConnections.Values) {
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

	public bool SetSurface(GameObject obj, IBlock block, BlockCoord blockCoord) {
		var surface = blockCoord.surface;

		if (surface.hasObject) {
			return false;
		}

		if (block.blockCoord.surface != null) {
			block.blockCoord.surface.block = null;
		}
			
		var point = surface.pointWithUV (blockCoord.uv);
		var position = gameObject.transform.TransformPoint (point);
		obj.transform.position = position;
		block.blockCoord.surface = surface;
		surface.block = block;

		obj.transform.localRotation = surface.rotation;

		return true;
	}

	public bool SetSurface(GameObject obj, IBlock block, Surface surface) {
		var blockCoord = new BlockCoord ();
		blockCoord.surface = surface;
		return SetSurface (obj, block, blockCoord);
	}

	// Ratio should be more than 0 and less than or equal to 1
	public bool LerpSurface(IBlock block, GameObject obj, Surface surface1, Surface surface2, float ratio) {
		if (surface1.identifier == surface2.identifier) {
			throw new Exception ("Invalid surface");
		}

		ratio *= 8.0f;
		ratio = Mathf.Floor (ratio);
		ratio /= 8.0f;

		var position1 = surface1.point;
		var position2 = surface2.point;

		var position = position1 + (position2 - position1) * ratio;

		obj.transform.position = gameObject.transform.TransformPoint (position);

		if (ratio == 1.0f) {
			if (block.blockCoord.surface != null) {
				block.blockCoord.surface.block = null;
			}

			block.blockCoord.surface = surface2;
		}

		surface2.block = block;

		if (ratio < 0.5) {
			obj.transform.localRotation = surface1.rotation;	
		} else {
			obj.transform.localRotation = surface2.rotation;
		}

		return true;
	}

	public Connection RandomConnection(string surfaceIdentifier) {
		var surface = Terrian.GetSurface (surfaceIdentifier);
		var index = UnityEngine.Random.Range (0, surface.connections.Count - 1);
		return surface.connections [index];	
	}

	public Surface RandomSurface(Surface surface, int num = 1) {
		var identifier = surface.identifier;

		for (var i = 0; i < num; i++) {
			var connection = RandomConnection (identifier);
			identifier = connection.OtherSurface (identifier).identifier;
		}

		return _terrian.GetSurface (identifier);
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
