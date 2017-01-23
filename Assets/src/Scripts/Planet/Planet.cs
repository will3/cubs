using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cubiquity;
using AssemblyCSharp;
using System;
using System.Linq;

public class Planet : MonoBehaviour {
	public int size = 24;
	public float treesNum = 1.0f;

	private Terrian _terrian;
	public Terrian Terrian {
		get { return _terrian; }
	}

	public bool drawNormals = false;
	public bool drawConnections = false;

	private Chunks chunks;

	private Water water;

	private Trees trees;

	/// <summary>
	/// Create and place a block component 
	/// </summary>
	/// <param name="name">Name.</param>
	/// <param name="blockCoord">Block coordinate.</param>
	public GameObject Create(string name, BlockCoord blockCoord) {
		var obj = Prefabs.Create (name);
		obj.transform.parent = gameObject.transform.parent;

		var block = (IBlock)obj.GetComponent<BlockComponent> ();

		SetSurface (obj, block, blockCoord);

		return obj;
	}

	public GameObject Create(string name, Surface surface) {
		return Create (name, new BlockCoord (surface));
	}

	/// <summary>
	/// Add block to terrian
	/// </summary>
	/// <param name="coord">Coordinate.</param>
	/// <param name="block">Block.</param>
	public void AddBlock(Vector3i coord, TerrianBlock block) {
		chunks.Set (coord.x, coord.y, coord.z, block.ToVoxel());

		Terrian.ReloadConnectionsAround (coord);
	}
		
	void Start () {
		_terrian = new Terrian (size);
		Game.Instance.Planet = this;
		Game.Instance.Terrian = _terrian;
			
		chunks = GetComponent<Chunks> ();
		Debug.Assert (chunks != null);

		water = GetComponent<Water> ();
		Debug.Assert (water != null);

		var center = new Vector3 (-size / 2, -size / 2, -size / 2) + (new Vector3() * 0.5f);
		gameObject.transform.position = center;

		Terrian.Generate ();

		trees = new Trees (this, _terrian);
		trees.treesNum = treesNum;
		trees.Generate ();

		loadData ();

		chunks.UpdateMesh ();
		water.Load (chunks);

		var dragCamera = Camera.main.GetComponent<DragCamera> ();
		if (dragCamera != null) {
			dragCamera.distance = size * 1.3f;
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

	private void loadData() {
		foreach (var kv in Terrian.map) {
			var coord = kv.Key;
			var block = kv.Value;
			chunks.Set (coord.x, coord.y, coord.z, block.ToVoxel());
		}
	}
}
