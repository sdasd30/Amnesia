using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireController : MonoBehaviour {

	public float fireDelay = 0.2f;
	float currentDelay = 0f;
	public float powerUpTime = 0.0f;
	public float maxPowerUpTime = 30.0f;
	public GameObject projectile;

	// Use this for initialization
	void Start () {}
	
	// Update is called once per frame
	void Update () {
		if (powerUpTime > 0f) {
			powerUpTime = Mathf.Max (0f, powerUpTime - Time.deltaTime);
		}
		currentDelay -= Time.deltaTime;
	}
	public float modifyPowerUp(float diff) {
		float prevTime = powerUpTime;
		powerUpTime = Mathf.Min(maxPowerUpTime,Mathf.Max (0, powerUpTime + diff));
		return powerUpTime - prevTime;
	}
	public void fire(Vector2 dir) {
		if (currentDelay <= 0f) {
			Projectile proj = Instantiate (projectile,transform).GetComponent<Projectile> ();
			proj.projectileDir = new Vector3(dir.x,dir.y,0f);
			proj.creator = GetComponent<Attackable> ();
			currentDelay = fireDelay;
			if (powerUpTime > 0f) {
				proj.damage *= 2f;
			}
		}
	}
}
