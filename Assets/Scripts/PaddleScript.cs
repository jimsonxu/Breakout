using UnityEngine;
using System.Collections;

public class PaddleScript : MonoBehaviour {

	// constants
	const float kLeftLimit = -5.28f; // x position that we cannot go past
	const float kRightLimit = 5.28f;

	public delegate void ReleaseBall();
	public static event ReleaseBall OnReleaseBall;

	Transform myTransform;

	// serialized fields
	[SerializeField]
	float speed = 0.01f;

	[SerializeField]
	Transform m_ballPrefab;

	//state
	bool m_doHaveBall = true; // is there a ball on top of this paddle?
	Transform m_ball;

	// Use this for initialization
	void Start () {
		myTransform = GetComponent<Transform> ();
		HandleSpawnBall ();
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 curPos = myTransform.position;
		float newX = 0f;

		if (Input.GetKey ("left")) {
			newX = curPos.x - speed;
			if (newX < kLeftLimit)
				newX = kLeftLimit;
			
			myTransform.position = new Vector2 (newX, curPos.y);
		} else if (Input.GetKey ("right")) {
			newX = curPos.x + speed;
			if (newX > kRightLimit)
				newX = kRightLimit;
			
			myTransform.position = new Vector2 (newX, curPos.y);
		} else if (Input.GetKey (KeyCode.Space)) {
			if (m_doHaveBall) {
				m_doHaveBall = false;
				m_ball.parent = null;

				Rigidbody2D ballRigidBody = m_ball.GetComponent<Rigidbody2D> ();
				BallScript ballScript = m_ball.GetComponent<BallScript> ();
				ballScript.StartMoving (Vector2.right + Vector2.up);

				if (OnReleaseBall != null)
					OnReleaseBall ();
			}
		}
	}

	void OnEnable() {
		BrickManager.OnSpawnBall += HandleSpawnBall;
	}

	void OnDisable() {
		BrickManager.OnSpawnBall -= HandleSpawnBall;
	}

	void HandleSpawnBall () {
		SpriteRenderer renderer = GetComponent<SpriteRenderer> ();
		SpriteRenderer ballRenderer = m_ballPrefab.GetComponent<SpriteRenderer> ();
		// the ball's center is the spawning point
		float buffer = 0.05f; // for aesthetics
		float y = myTransform.position.y + renderer.bounds.extents.y + ballRenderer.bounds.extents.y + buffer;
		Vector3 pos = new Vector3 (myTransform.position.x, y, 0);
		m_ball = Instantiate(m_ballPrefab, pos, Quaternion.identity) as Transform;
		m_ball.parent = myTransform;
		BallScript ballScript = m_ball.GetComponent<BallScript> ();

		m_doHaveBall = true;
	}
}
