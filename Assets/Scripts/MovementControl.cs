using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementControl : MonoBehaviour {

	public LayerMask collisionMask;

	const float skinWidth = .015f;
	int horizontalRayCount = 4;
	int verticalRayCount = 4;
	float horizontalRaySpacing;
	float verticalRaySpacing;
	float maxClimbAngle = 80;

	public Vector2 playerInput = Vector2.zero;
	public Vector2 accumulatedVelocity = Vector2.zero;

	public bool facingLeft = false;
	public Vector2 facingDir;

	public Vector2 velocity;

	BoxCollider2D bCollider;
	RaycastOrigins raycastOrigins;
	public CollisionInfo collisions;
	SpriteRenderer sprite;
	List<Vector2> CharForces = new List<Vector2>();
	//List<float> timeForces = new List<float>();
	Vector2 playerForce = new Vector2();
	public float dropThruTime = 0.0f;
	Animator anim;
	public bool canMove = false;

	// Use this for initialization
	void Start () {
		bCollider = GetComponent<BoxCollider2D> ();
		sprite = GetComponent<SpriteRenderer> ();
		CalculateRaySpacing ();
		canMove = true;
	}

	public void addToVelocity(Vector2 veloc ) {
		accumulatedVelocity.x += veloc.x;
		addSelfForce (new Vector2 (0f, veloc.y), 0f);
	}
	public void addSelfForce(Vector2 force, float duration) {
		CharForces.Add (force);
		//timeForces.Add (duration);
	}

	public void Move(Vector2 veloc, Vector2 input) {
		playerInput = input;
		playerForce = veloc;
	}

	// Update is called once per frame
	void Update () {}

	void FixedUpdate() {
		//Debug.Log (Time.deltaTime);
		dropThruTime = Mathf.Max(0f,dropThruTime - Time.fixedDeltaTime);
		if (Mathf.Abs(accumulatedVelocity.x) > 0.3f) {
			if (collisions.below) {
				accumulatedVelocity.x *= (1.0f - Time.fixedDeltaTime * 2.0f);
			} else {
				accumulatedVelocity.x *= (1.0f - Time.fixedDeltaTime * 3.0f);
			}
		} else {
			accumulatedVelocity.x = 0f;
		}
		processMovement ();
	}

	void processMovement() {
		if (!canMove) {
			playerInput = Vector2.zero;
		}
		playerForce = playerForce * Time.fixedDeltaTime;
		velocity.x = playerForce.x;
		velocity.y = playerForce.y;
		//bool Yf = false;

		/*for (int i = CharForces.Count - 1; i >= 0; i--) {
			Vector2 selfVec = CharForces [i];

			if (timeForces [i] < Time.fixedDeltaTime) {
				velocity += (selfVec * Time.fixedDeltaTime);
			} else {
				velocity += (selfVec * Time.fixedDeltaTime);
			}
			timeForces [i] = timeForces [i] - Time.fixedDeltaTime;
			if (timeForces [i] < 0f) {
				CharForces.RemoveAt (i);
				timeForces.RemoveAt (i);
			}
		}*/
		/*if (velocity.y > terminalVelocity){
			if (Yf || !collisions.below) {
				velocity.y += gravityScale * Time.fixedDeltaTime;
			} else if (collisions.below) { //force player to stick to slopes
				velocity.y += gravityScale * Time.fixedDeltaTime * 6f;
			}
		}*/
		velocity.y += (accumulatedVelocity.x * Time.fixedDeltaTime);
		velocity.x += (accumulatedVelocity.x * Time.fixedDeltaTime);
		UpdateRaycastOrigins ();
		collisions.Reset ();

		if (velocity.x != 0) {
			HorizontalCollisions (ref velocity);
		}
		if (velocity.y != 0) {
			VerticalCollisions (ref velocity);
		}

		transform.Translate (velocity);
	}

	void HorizontalCollisions(ref Vector2 velocity) {
		float directionX = Mathf.Sign (velocity.x);
		float rayLength = Mathf.Abs (velocity.x) + skinWidth;

		for (int i = 0; i < horizontalRayCount; i ++) {
			Vector2 rayOrigin = (directionX == -1)?raycastOrigins.bottomLeft:raycastOrigins.bottomRight;
			rayOrigin += Vector2.up * (horizontalRaySpacing * i);
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);
			if (hit) {
				Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength,Color.red);
			} else {
				Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength,Color.green);
			}

			if (hit && !hit.collider.isTrigger) {
				velocity.x = (hit.distance - skinWidth) * directionX;
				rayLength = hit.distance;

				collisions.left = directionX == -1;
				collisions.right = directionX == 1;
			}
		}
	}

	void VerticalCollisions(ref Vector2 velocity) {
		float directionY = Mathf.Sign (velocity.y);
		float rayLength = Mathf.Abs (velocity.y) + skinWidth;

		for (int i = 0; i < verticalRayCount; i ++) {
			Vector2 rayOrigin = (directionY == 1)?raycastOrigins.topRight:raycastOrigins.bottomRight;
			rayOrigin += Vector2.left * (horizontalRaySpacing * i);
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);
			if (hit) {
				Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength,Color.red);
			} else {
				Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength,Color.green);
			}

			if (hit && !hit.collider.isTrigger) {
				velocity.y = (hit.distance - skinWidth) * directionY;
				rayLength = hit.distance;

				collisions.below = directionY == -1;
				collisions.above = directionY == 1;
			}
		}
	}

	void CalculateRaySpacing() {
		Bounds bounds = bCollider.bounds;
		bounds.Expand (skinWidth * -2);

		horizontalRayCount = Mathf.Clamp (horizontalRayCount, 2, int.MaxValue);
		verticalRayCount = Mathf.Clamp (verticalRayCount, 2, int.MaxValue);

		horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
		verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
	}
	struct RaycastOrigins {
		public Vector2 topLeft, topRight;
		public Vector2 bottomLeft, bottomRight;
	}

	void ClimbSlope(ref Vector2 velocity, float slopeAngle) {
		float moveDistance = Mathf.Abs (velocity.x);
		float climbVelocityY = Mathf.Sin (slopeAngle * Mathf.Deg2Rad) * moveDistance;

		if (velocity.y <= climbVelocityY) {
			velocity.y = climbVelocityY;
			velocity.x = Mathf.Cos (slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign (velocity.x);
			collisions.below = true;
			collisions.climbingSlope = true;
			collisions.slopeAngle = slopeAngle;
		}
	}


	public struct CollisionInfo {
		public bool above, below;
		public bool left, right;

		public bool climbingSlope;
		public float slopeAngle, slopeAngleOld;

		public void Reset() {
			above = below = false;
			left = right = false;
			climbingSlope = false;

			slopeAngleOld = slopeAngle;
			slopeAngle = 0;
		}
	}

	void UpdateRaycastOrigins() {
		Bounds bounds = bCollider.bounds;

		bounds.Expand (skinWidth * -2);

		raycastOrigins.bottomLeft = new Vector2 (bounds.min.x , bounds.min.y);
		raycastOrigins.bottomRight = new Vector2 (bounds.max.x, bounds.min.y);
		raycastOrigins.topLeft = new Vector2 (bounds.min.x, bounds.max.y);
		raycastOrigins.topRight = new Vector2 (bounds.max.x, bounds.max.y);
	}

}
