using UnityEngine;
using System.Collections;

public class Brick : MonoBehaviour {

	[SerializeField]
	Transform ballPrefab;

	const string kPowerTagString = "Power";

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionExit2D(Collision2D col) {
		if (gameObject.tag == kPowerTagString) {
			Instantiate (ballPrefab, Vector3.zero, Quaternion.identity);
		}

		Destroy (this.gameObject);
	}
}
