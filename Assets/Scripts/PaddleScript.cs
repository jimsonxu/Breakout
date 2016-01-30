using UnityEngine;
using System.Collections;

public class PaddleScript : MonoBehaviour {
	
	Transform myTransform;

	[SerializeField]
	float speed = 0.01f;

	// Use this for initialization
	void Start () {
		myTransform = GetComponent<Transform> ();
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 curPos = myTransform.position;
		if (Input.GetKey ("left")) {
			myTransform.position = new Vector2 (curPos.x - speed, curPos.y);
		} else if (Input.GetKey ("right")) {
			myTransform.position = new Vector2 (curPos.x + speed, curPos.y);
		}
	}
}
