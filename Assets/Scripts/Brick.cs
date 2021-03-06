﻿using UnityEngine;
using System.Collections;

public class Brick : MonoBehaviour {

	[SerializeField]
	Transform ballPrefab;

	const string kPowerTagString = "Power";
	const float kMinX = -2.5f;
	const float kMaxX = 2.5f;

	public delegate void BrickDestroyed(Transform brick);
	public static event BrickDestroyed OnBrickDestroyed;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionExit2D(Collision2D col) {
		if (gameObject.tag == kPowerTagString) {
			Transform ball = (Transform)Instantiate (ballPrefab, transform.position, Quaternion.identity);
			ball.GetComponent<BallScript> ().StartMoving (Vector2.down);
			BrickManager.AddNewBall (ball.gameObject);
		}

		Destroy (this.gameObject);

		if (OnBrickDestroyed != null) {
			OnBrickDestroyed (transform);
		}
	}
}
