using UnityEngine;
using System.Collections;

public class BallScript : MonoBehaviour {

	[SerializeField]
	float m_speed = 5;

	Rigidbody2D m_rigidBody;

	// Use this for initialization
	void Start () {
		m_rigidBody = GetComponent<Rigidbody2D> ();
		m_rigidBody.velocity = Vector2.down * m_speed;
	}
}
