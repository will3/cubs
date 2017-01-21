using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class Arrow : MonoBehaviour {
	public Vector3 velocity = new Vector3(0.0f, 0.0f, 0.0f);
	public float timeToLive = 2;
	public Damage damage = new Damage(10.0f, "arrow");
	public IDamagable target;

	private Planet planet;
	private bool hitSurface;
	private double destroyTimestamp;

	void Start () {
		destroyTimestamp = Time.time + timeToLive;
	}

	void Update () {
		if (Time.time > destroyTimestamp) {
			Destroy (gameObject);
			return;
		}

		var planet = Game.Instance.Planet;
		if (planet == null) {
			return;
		}

		if (hitSurface) {
			return;
		}

		var dir = velocity.normalized;
	
		transform.LookAt (Camera.main.transform, dir);

		var dis = velocity.magnitude * Time.deltaTime;

		var ray = new Ray (transform.position, velocity);

		RaycastHit hitInfo;
		if (Physics.Raycast (ray, out hitInfo, dis)) {
			transform.position = hitInfo.point;
			hitSurface = true;
		} else {
			transform.position += velocity * Time.deltaTime;
		}
	}

	void OnDestroy() {
		if (target != null) {
			target.ApplyDamage (damage);
		}
	}
}
