using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attackable : MonoBehaviour {

	public float health;
	public float maxHealth;
	public int money = 40;
	public bool isAlive = true;
	public string faction = "enemy";
	public GameObject moneyObj;
	private AudioSource hurtSound;
	private AudioSource deathSound;
	public float pagesUsed = 0;
	public float maxPages = 5;

	// Use this for initialization
	void Start () {
		AudioSource[] sounds = GetComponents<AudioSource> ();
		if (sounds.Length > 0) {
			hurtSound = sounds [0];
			deathSound = sounds [1];
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void die() {
	}
	public float modifyHealth(float diff) {
		float prevHealth = health;
		if (diff < 0 && hurtSound != null) {
			hurtSound.Play ();
		}
		health = Mathf.Min(maxHealth,Mathf.Max (0, health + diff));
		//Debug.Log ("New health: " + health + " damage done: " + diff);
		if (health == 0f) {
			setAlive (false);
		}
		return health - prevHealth;
	}
	public void setAlive(bool alive) {
		isAlive = alive;
		if (isAlive == false) {
			if (GetComponent<MovementControl> ()) {
				GetComponent<MovementControl> ().canMove = false;
				if (GetComponent<PlayerControl>() == null) {
					StartCoroutine (death());
					while (money > 0) {
						Vector3 mPos = transform.position;
						Instantiate(moneyObj,new Vector3(mPos.x + Random.Range(-1f,1f),mPos.y + Random.Range(-1f,1f),0f),Quaternion.identity);
						money -= 20;
					}
				}
			}
		}
	}

	IEnumerator death()
	{
		this.GetComponent<SpriteRenderer> ().enabled = false;
		deathSound.Play ();
		yield return new WaitForSeconds (1f);
		Destroy (gameObject);
	}
}

