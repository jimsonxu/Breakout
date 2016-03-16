using UnityEngine;
using System.Collections;

public class BallScript : MonoBehaviour {

	public float speed = 5;

	bool isStopped = true;

	Rigidbody2D m_rigidBody;
	AudioSource audioSrc;

	const float kNudgeMax = 5f;
	const float kNudgeMin = -5f;

	void Awake () {
		audioSrc = GetComponent<AudioSource> ();
	}

	public void StartMoving(Vector2 dir) {
		m_rigidBody = GetComponent<Rigidbody2D> ();
		m_rigidBody.velocity = dir.normalized * speed;
		isStopped = false;
	}

	void OnCollisionEnter2D(Collision2D col) {
		audioSrc.PlayOneShot (audioSrc.clip, 1f);

		if (col.gameObject.name == "Paddle") {
			/* Angle is determined by the ball's distance from center of paddle. Greater the distance,
			 * sharper the angle (smaller angle).
			 */
			float ballToPaddleX = transform.position.x - col.transform.position.x;
			Vector2 newDir = new Vector2 (ballToPaddleX, 1).normalized;
			m_rigidBody.velocity = newDir * m_rigidBody.velocity.magnitude;
		}
	}

	void FixedUpdate() {
		/*@!! This is a hack that nudges the ball so we aren't stuck in either the x or y axis, that is
		 * the velocity in either direction is 0.
		 */

		// ignore nudging if we're not moving
		if (isStopped)
			return;
		
		Vector2 vel = m_rigidBody.velocity;
		float newX = vel.x;
		float newY = vel.y;

		if (vel.x == 0) {
			while ((newX = Random.Range (kNudgeMin, kNudgeMax)) == 0)
				;
		}

		if (vel.y == 0) {
			while ((newY = Random.Range (kNudgeMin, kNudgeMax)) == 0)
				;
		}

		m_rigidBody.velocity = new Vector2 (newX, newY);

	}
}
