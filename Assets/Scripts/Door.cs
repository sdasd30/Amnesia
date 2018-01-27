using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

	public DoorDir doorDirection;
	// Use this for initialization
	void Start () {	}
	
	// Update is called once per frame
	void Update () {
		
	}

	internal void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.GetComponent<Movement_Player> ()) {
			Debug.Log ("collision detected with door");
			LevelManager.Get ().OnEnterDoor (doorDirection);
			if (doorDirection.Equals (DoorDir.RIGHT)) {
				other.gameObject.transform.position = new Vector3 (-3.5f, 0.4f, 0f);
			} else if (doorDirection.Equals (DoorDir.LEFT)) {
				other.gameObject.transform.position = new Vector3 (4.3f, 0.4f, 0f);
			} else if (doorDirection.Equals (DoorDir.UP)) {
				other.gameObject.transform.position = new Vector3 (0f, -1.2f, 0f);
			} else if (doorDirection.Equals (DoorDir.DOWN)) {
				other.gameObject.transform.position = new Vector3 (0f, 2.8f, 0f);
			}
		}
	} 
}
