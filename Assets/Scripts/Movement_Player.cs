using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent (typeof (Rigidbody2D))]

public class Movement_Player : MonoBehaviour {
	public float speed = 5f;
	public float rotSpeed = 360f;

	Rigidbody2D m_body;
	// Use this for initialization
	void Start () {
		m_body = GetComponent<Rigidbody2D> ();
	}

	// Update is called once per frame
	void Update () {
		//Movement Up, Down, Left, Right
		if (Input.GetKey("a")){
			m_body.transform.Translate  (new Vector2(-speed, 0f) * Time.deltaTime,Space.World);
		}
		if (Input.GetKey("d")){
			m_body.transform.Translate (new Vector2(speed, 0f) * Time.deltaTime,Space.World);
		}
		if (Input.GetKey("w")){
			m_body.transform.Translate (new Vector2(0f, speed) * Time.deltaTime,Space.World);
		}
		if (Input.GetKey ("s")) {
			m_body.transform.Translate (new Vector2 (0f, -speed) * Time.deltaTime, Space.World);
		}

		Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector2 currentPos = new Vector2(transform.position.x,transform.position.y);
		float direction = (Mathf.Atan2 (mousePos.y - currentPos.y, mousePos.x - currentPos.x)) * Mathf.Rad2Deg;
		float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.z, direction - 90, rotSpeed * Time.deltaTime);
		m_body.transform.rotation = Quaternion.Euler (new Vector3(0f,0f,angle));

	}
}
