using UnityEngine;
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
			Vector3 pos = new Vector3 (Random.Range (kMinX, kMaxX), 0, 0); 
			Instantiate (ballPrefab, pos, Quaternion.identity);
		}

		Destroy (this.gameObject);

		if (OnBrickDestroyed != null) {
			OnBrickDestroyed (transform);
		}
	}
}
