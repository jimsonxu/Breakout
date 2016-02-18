using UnityEngine;
using System.Collections;

public class BallScript : MonoBehaviour {

	public float speed = 5;

	Rigidbody2D m_rigidBody;

	// Use this for initialization
	void Start () {
		
	}

	void OnCollisionEnter2D(Collision2D col) {
		if (m_rigidBody == null) {
			m_rigidBody = GetComponent<Rigidbody2D> ();
		}

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
