using UnityEngine;
using System.Collections;

public class Pit : MonoBehaviour {

	public delegate void BallDestroyed(GameObject ball);
	public static event BallDestroyed OnBallDestroyed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D col) {
		if (OnBallDestroyed != null) {
			OnBallDestroyed (col.gameObject);
		}

		Destroy (col.gameObject);
	}
}
