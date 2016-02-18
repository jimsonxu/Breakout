using UnityEngine;
using System.Collections;

public class BallScript : MonoBehaviour {

	public float speed = 5;

	bool isStopped = true;

	Rigidbody2D m_rigidBody;

	// Use this for initialization
	void Start () {
	}

	public void StartMoving(Vector2 dir) {
		m_rigidBody = GetComponent<Rigidbody2D> ();
		m_rigidBody.velocity = dir.normalized * speed;
		isStopped = false;
	}

	void OnCollisionEnter2D(Collision2D col) {
		if (col.gameObject.name == "Paddle") {
			/* Angle is determined by the ball's distance from center of paddle. Greater the distance,
			 * sharper the angle (smaller angle).
			 */
			float ballToPaddleX = transform.position.x - col.transform.position.x;
			Vector2 newDir = new Vector2 (ballToPaddleX, 1).normalized;
			m_rigidBody.velocity = newDir * m_rigidBody.velocity.magnitude;
		}
	}
}
