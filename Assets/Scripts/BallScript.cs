using UnityEngine;
using System.Collections;

public class BallScript : MonoBehaviour {

	[SerializeField]
	float force = 4.5f;

	Rigidbody2D myRigidBody;

	// Use this for initialization
	void Start () {
		myRigidBody = GetComponent<Rigidbody2D> ();
		myRigidBody.AddForce (new Vector2(0, -force));
	}
	
	// Update is called once per frame
	void Update () {
		myRigidBody.AddForce (new Vector2(0, -force));
	}
}
