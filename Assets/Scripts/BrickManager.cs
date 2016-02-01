using UnityEngine;
using System.Collections;

public class BrickManager : MonoBehaviour {

	[SerializeField]
	Transform m_brickPrefab;

	// Use this for initialization
	void Start () {
		for (int x = 0; x < 5; x++) {
			for (int y = 0; y < 5; y++) {
				Instantiate (m_brickPrefab, new Vector2 (x, y), Quaternion.identity);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
