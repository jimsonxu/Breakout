using UnityEngine;
using System.Collections;

public class BrickManager : MonoBehaviour {

	[SerializeField]
	Transform m_brickPrefab;

	// Use this for initialization
	void Start () {
		Color colorOdd = Color.white;
		Color colorEven = Color.cyan;
		SpriteRenderer renderer = m_brickPrefab.GetComponent<SpriteRenderer>();
		renderer.color = colorOdd;
		int cnt = 1;

		for (int x = 0; x < 5; x++) {
			for (int y = 0; y < 5; y++) {
				if (cnt % 2 == 0)
					renderer.color = colorEven;
				else
					renderer.color = colorOdd;
				
				Instantiate (m_brickPrefab, new Vector2 (x, y), Quaternion.identity);
				cnt++;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
