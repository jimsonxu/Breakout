﻿using UnityEngine;
using System.Collections;

public class BallScript : MonoBehaviour {

	[SerializeField]
	float m_speed = 5;

	Rigidbody2D m_rigidBody;
	Collider2D m_collider;

	// Use this for initialization
	void Start () {
		m_rigidBody = GetComponent<Rigidbody2D> ();
		m_rigidBody.velocity = Vector2.down * m_speed;

		m_collider = GetComponent<Collider2D> ();
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
