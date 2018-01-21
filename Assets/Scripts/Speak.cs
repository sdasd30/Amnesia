using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Speak : MonoBehaviour {

	public GameObject speechBubble;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.R)) {
			Talk ("Howdy!");
		}
		
	}

	public void Talk(string s) {
		Vector3 spawnPosition = this.gameObject.transform.position;
		spawnPosition.x = this.gameObject.transform.position.x + 
			this.gameObject.GetComponent<Renderer> ().bounds.extents.x;
		spawnPosition.y = this.gameObject.transform.position.y + 
			this.gameObject.GetComponent<Renderer> ().bounds.extents.y;
		GameObject speech=Instantiate (speechBubble, this.gameObject.transform);
		speech.GetComponentInChildren<Text> ().text = s;
		StartCoroutine (Talking (speech));
	}

	IEnumerator Talking(GameObject g)
	{
		yield return new WaitForSeconds (4.0f);
		Destroy (g);
	}
}
