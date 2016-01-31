﻿using UnityEngine;
using System.Collections;

public class PaddleScript : MonoBehaviour {
	
	Transform myTransform;

	[SerializeField]
	float speed = 0.01f;

	float kLeftLimit = -5.28f; // x position that we cannot go past
	float kRightLimit = 5.28f;

	// Use this for initialization
	void Start () {
		myTransform = GetComponent<Transform> ();
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
		}
	}
}