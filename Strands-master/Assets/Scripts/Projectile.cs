using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	public Vector3 projectileDir;
	public float projectileSpeed = 5f;
	public float duration = 0.5f;
	public float damage = 10f;
	public Attackable creator;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += (projectileSpeed * Time.deltaTime * projectileDir);
		duration = duration - Time.deltaTime;
		if (duration < 0.0f) {
			Destroy (gameObject);
		}
	}
	internal void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.GetComponent<Attackable> ()) {
			Attackable otherAttack = other.gameObject.GetComponent<Attackable> ();
			if (creator == null || otherAttack.faction != creator.faction) {
				otherAttack.modifyHealth (-damage);
				duration = 0f;
			}
		}
	}
}
