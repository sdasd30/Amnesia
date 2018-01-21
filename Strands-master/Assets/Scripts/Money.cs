using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : MonoBehaviour {
	public int value = 20;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	internal void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.GetComponent<Attackable> ()) {
			Attackable atk = other.gameObject.GetComponent<Attackable> ();
			if (atk.isAlive) {
				atk.money += value;
				Destroy (gameObject);
			}
		}
	} 
}
