using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (MovementControl))]
[RequireComponent (typeof (Attackable))]

public class AIControl : MonoBehaviour {

	public float maxChaseDistance = 2f;
	public float minChaseDistance = 5f;

	public float moveSpeed = 3.0f;
	PlayerControl playerObj;
	MovementControl movement;
	Vector2 velocity;
	Vector2 lastMajorVelocity;
	FireController m_fireControl;

	// Use this for initialization
	void Start () {
		playerObj = FindObjectOfType<PlayerControl> ();
		movement = GetComponent<MovementControl> ();
		m_fireControl = GetComponent<FireController> ();
	}
	// Update is called once per frame
	void Update () {
		float dist = Vector3.Distance (playerObj.transform.position, transform.position);

		if (dist < maxChaseDistance) {
			if (dist > minChaseDistance) {
				Vector3 otherPos = playerObj.transform.position;
				Vector3 myPos = transform.position;
				float xDiff = otherPos.x - myPos.x;
				float yDiff = otherPos.y - myPos.y;
				float angle = Mathf.Atan2 (yDiff , xDiff);
				velocity = new Vector2 (Mathf.Cos (angle) * moveSpeed, Mathf.Sin (angle) * moveSpeed);
				movement.Move (velocity, velocity.normalized);
			}
			m_fireControl.fire ((playerObj.transform.position - transform.position).normalized);
		}
	}
}
