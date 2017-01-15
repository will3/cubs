using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	public Vector3 velocity;
	public float lifeTime;
	private float lifeTimerCounter;
	public float damage;

	public BlockComponent target;

	// Use this for initialization
	void Start () {
			
	}
	
	// Update is called once per frame
	void Update () {
		lifeTimerCounter += Time.deltaTime;
		gameObject.transform.position += velocity * Time.deltaTime;

		if (lifeTimerCounter > lifeTime) {
			Destroy (gameObject);

			target.hitPoints -= this.damage;
		}
	}
}
