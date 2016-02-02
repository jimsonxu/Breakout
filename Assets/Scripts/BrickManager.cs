using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class BrickManager : MonoBehaviour {

	[SerializeField]
	Transform m_brickPrefab;

	Vector2 m_brickSize;

	// Use this for initialization
	void Start () {
		LoadBricksCfg ();

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

	/***
	 * Valid config line types:
	 * size=2,1
	 * [empty line]
	 * 1,1
	 * 
	 * Returns true on successful load and false on failure
	 */
	bool LoadBricksCfg() {
		try {
			TextReader reader = new StreamReader ("Assets/bricks.cfg");

			using (reader) {
				string line = reader.ReadLine();
				string[] values, coord;
				int x, y;

				while (line != null) {
					values = line.Split('=');
					if (values.Length > 0) {
						if (values[0] == "size") {
							coord = values[1].Split(',');
							x = Int32.Parse(coord[0]);
							y = Int32.Parse(coord[1]);

							m_brickSize = new Vector2(x, y);
						}
					}

					line = reader.ReadLine ();
				}

				reader.Close();
				return true;
			}
		}
		catch (Exception e) {
			Debug.Log (e.Message);
			return false;
		}
	}
}
