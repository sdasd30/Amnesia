using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum direction {LEFT,RIGHT,UP,DOWN};
[RequireComponent (typeof (Rigidbody2D))]

public class Movement_Player : MonoBehaviour {
	public float speed = 1f;
	public float rotSpeed = 360f;
	private direction m_dir;
	private bool isMoving = false;
	AnimatorSprite m_anim;
	Rigidbody2D m_body;
	// Use this for initialization
	void Start () {
		m_body = GetComponent<Rigidbody2D> ();
		m_anim = GetComponent<AnimatorSprite> ();
	}

	// Update is called once per frame
	void Update () {
		//Movement Up, Down, Left, Right
		isMoving = false;
		if (Input.GetKey("a")){
			m_body.transform.Translate  (new Vector2(-speed, 0f) * Time.deltaTime,Space.World);
			m_dir = direction.LEFT;
			isMoving = true;
		}
		if (Input.GetKey("d")){
			m_body.transform.Translate (new Vector2(speed, 0f) * Time.deltaTime,Space.World);
			m_dir = direction.RIGHT;
			isMoving = true;
		}
		if (Input.GetKey("w")){
			m_body.transform.Translate (new Vector2(0f, speed) * Time.deltaTime,Space.World);
			m_dir = direction.UP;
			isMoving = true;
		}
		if (Input.GetKey ("s")) {
			m_body.transform.Translate (new Vector2 (0f, -speed) * Time.deltaTime, Space.World);
			m_dir = direction.DOWN;
			isMoving = true;
		}
		Animation();

	}

	void Animation(){
		if (isMoving == true) {
			if (m_dir == direction.UP) {
				m_anim.Play ("walk_u");
			} 
			else if (m_dir == direction.DOWN) {
				m_anim.Play ("walk_d");
			} 
			else if (m_dir == direction.UP && (Input.GetKey("a") || Input.GetKey("d"))) {
				m_anim.Play ("walk_u");
			} 
			else if (m_dir == direction.DOWN && (Input.GetKey("a") || Input.GetKey("d"))) {
				m_anim.Play ("walk_d");
			} 
			else if (m_dir == direction.LEFT && !(Input.GetKey("w") && Input.GetKey("s"))) {
				GetComponent<SpriteRenderer> ().flipX = true;
				m_anim.Play ("walk_s");
			} 
			else if (m_dir == direction.RIGHT && !(Input.GetKey("w") && Input.GetKey("s"))) {
				GetComponent<SpriteRenderer> ().flipX = false;
				m_anim.Play ("walk_s");
			} 
		}

		else {
			if (m_dir == direction.UP)
				m_anim.Play ("idle_u");
			else if (m_dir == direction.RIGHT || m_dir == direction.LEFT)
				m_anim.Play ("idle_s");
			else if (m_dir == direction.DOWN)
				m_anim.Play ("idle_d");
		}
	}
}
