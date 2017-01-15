using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class Turrent : MonoBehaviour {

	private BlockComponent blockComponent;
	private List<Surface> surfaces = new List<Surface>();
	private Cooldown fireCooldown = new Cooldown(1.0f);

	void Start () {

		blockComponent = GetComponent<BlockComponent> ();

		var planet = Game.Instance.Planet;

		var point1 = planet.transform.TransformPoint (blockComponent.currentSurface.pointAbove);

		foreach (var kv in planet.Terrian.surfaceByIdentifier) {
			var surface = kv.Value;

			var point2 = planet.transform.TransformPoint (surface.pointAbove);

			var ray = new Ray (point1, (point2 - point1).normalized);

			if (Physics.Raycast (ray, Vector3.Distance(point2, point1))) {
				continue;
			}
				
			surfaces.Add (surface);
		}

		// Sort by distance
		surfaces.Sort ((x, y) => {
			return (int)(x.DistanceTo(blockComponent.currentSurface) - y.DistanceTo(blockComponent.currentSurface));
		});
	}

	void Update () {
		fireCooldown.Update ();
		if (fireCooldown.Ready ) {
			var target = SearchTarget ();	
			if (target != null) {
				Fire (target);
				fireCooldown.Use ();
			}
		}
	}

	BlockComponent SearchTarget() {
		foreach (Surface surface in surfaces) {
			var blockComponent = surface.blockComponent;

			if (blockComponent != null && blockComponent.targetable) {
				return blockComponent;
			}
		}

		return null;
	}

	// Shoot at object
	public void Fire(BlockComponent target) {
		var obj = Prefabs.Create (Prefabs.Bullet);
		obj.transform.position = transform.position;
		obj.transform.LookAt (target.transform);

		var bullet = obj.GetComponent<Bullet>();

		var speed = 80.0f;
		bullet.velocity = (target.transform.position - transform.position).normalized * speed;
		bullet.lifeTime = Vector3.Distance (target.transform.position, transform.position) / speed;
		bullet.target = target;
	}
}
