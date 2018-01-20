using UnityEngine;
using System.Collections.Generic;

public class ContinuousHitbox : MonoBehaviour {
	List<Attackable> overlappingControl = new List<Attackable> (); 

	public float damage = 10.0f;
	//public bool fixedKnockback = false;
	public Vector2 knockback = new Vector2(0.0f,4.0f);
	public float hitboxDuration = 1.0f;
	public bool timedHitbox = true;
	public Sprite emptySprite;
	public bool finite = false;
	public float finiteAmount = 30f;
	public float m_amountDamage = 0f;
	bool active = true;
	public string fountainColor = "red";
	public string fountainType = "health";

	// Use this for initialization
	void Start () {}

	// Update is called once per frame
	void Update () {
		if (hitboxDuration > 0.0f || !timedHitbox) {
			if (finite && m_amountDamage > finiteAmount) {
				if (active) {
					active = false;
					GetComponent<SpriteRenderer> ().sprite = emptySprite;
				}
			} else {
				foreach (Attackable cont in overlappingControl) {
					if (fountainType == "health") {
						float diff = cont.modifyHealth (-damage * Time.deltaTime);
						m_amountDamage += Mathf.Abs (diff);
					} else if (fountainType == "poison") {
						float diff = cont.modifyHealth (damage * Time.deltaTime * 0.75f);
						m_amountDamage += Mathf.Abs (diff);
					} else if (fountainType == "power") {
						float diff = cont.GetComponent<FireController> ().modifyPowerUp (-damage * Time.deltaTime);
						m_amountDamage += Mathf.Abs (diff);
					}
				}
			}
		} else {
			GameObject.Destroy (gameObject);
		}
	}
	/*
	public void setKnockback(Vector2 kb) {
		knockback = kb;
	}

	public void setFixedKnockback(bool fixedKB) {
		fixedKnockback = fixedKB;
	}
	*/

	public void setDamage(float dmg) {
		damage = dmg;
	}
	public void setHitboxDuration (float time) {
		hitboxDuration = time;
	}
	public void setScale(Vector2 scale) {
		transform.localScale = scale;
	}
	public void setTimedHitbox(bool timed) {
		timedHitbox = timed;
	}
	void OnDrawGizmos() {
		Gizmos.color = new Color (1, 0, 0, .5f);
		Gizmos.DrawCube (transform.position, transform.localScale);
	}
	internal void OnTriggerEnter2D(Collider2D other) {
		//Debug.Log ("collision detected with Continuous hitbox");
		if (other.gameObject.GetComponent<Attackable>()) {
			overlappingControl.Add (other.gameObject.GetComponent<Attackable> ()); 
		}
	} 
	internal void OnTriggerExit2D(Collider2D other) {
		//Debug.Log ("Collision ended with Continuous hitbox");
		if (other.gameObject.GetComponent<Attackable> () && other.gameObject.GetComponent<FireController>()) {
			overlappingControl.Remove (other.gameObject.GetComponent<Attackable> ()); //Removes the object from the list
		}
	}
}
