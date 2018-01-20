using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (MovementControl))]
[RequireComponent (typeof (Attackable))]

public class PlayerControl : MonoBehaviour {
	public float moveSpeed = 8.0f;
	public float acceleration = 0.1f;


	Vector2 velocity;
	MovementControl m_movement;
	Animator anim;
	Vector2 direction;
	Vector2 lastMajorDir = new Vector2 (1f, 0f);
	float velocityXSmoothing;
	float velocityYSmoothing;
	FireController m_fireControl;

	// Use this for initialization
	void Start () {
		m_movement = GetComponent<MovementControl> ();
		m_fireControl = GetComponent<FireController> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (m_movement.canMove) {
			float inputX = Input.GetAxis("Horizontal");
			float inputY = Input.GetAxis("Vertical");
			Vector2 inp = new Vector2 (inputX, inputY);
			if (inputY > 0.5) {
				direction.y = 1;
			} else if (inputY < -0.5) {
				direction.y = -1;
			}

			if (inputX > 0.5) {
				direction.x = 1;
			} else if (inputX < -0.5) {
				direction.x = -1;
			}
			Vector2 targetVel = inp * moveSpeed;
			velocity.x = Mathf.SmoothDamp (velocity.x, targetVel.x, ref velocityXSmoothing, acceleration);
			velocity.y = Mathf.SmoothDamp (velocity.y, targetVel.y, ref velocityYSmoothing, acceleration);
			m_movement.Move (velocity, inp);
			if (Input.GetButtonDown ("Fire1")) {
				if (velocity.magnitude < 0.1f) {
					m_fireControl.fire (lastMajorDir);
				} else {
					lastMajorDir = velocity.normalized;
					m_fireControl.fire (velocity.normalized);
				}
			}
			if (Input.GetButtonDown("Fire2")) {
				if (GetComponent<Attackable> ().pagesUsed < GetComponent<Attackable> ().maxPages) {
					LevelManager.Get ().saveRoom ();
					GetComponent<Attackable> ().pagesUsed++;
				}
			}

		}
	}
		


}
