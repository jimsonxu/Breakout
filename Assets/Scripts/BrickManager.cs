using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class BrickManager : MonoBehaviour {

	Vector2 kTopLeft;
	const float kCfgPixel = 0.5f; // 1 cfg pixel is 0.5 game pixels
	Vector2 kDefaultScale;

	[SerializeField]
	Transform m_brickPrefab;

	Vector2 m_brickSize;
	SpriteRenderer m_renderer;

	// Use this for initialization
	void Start () {
		kTopLeft = new Vector2 (-6.87f, 4.6f);
		kDefaultScale = new Vector2 (0.75f, 0.25f);
		m_renderer = m_brickPrefab.GetComponent<SpriteRenderer> ();
		LoadBricksCfg ();
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

							m_renderer.transform.localScale = new Vector3(m_brickSize.x * kDefaultScale.x, m_brickSize.y * kDefaultScale.y, 0);
						} else if (values.Length == 1 && values[0].Length > 0) {
							// no '=' found and not a blank line
							// brick coordinate
							coord = line.Split(',');
							if (coord.Length > 0) {
								x = Int32.Parse(coord[0]);
								y = Int32.Parse(coord[1]);

								SpawnBrick(new Vector2(x, y));
							}
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

	void SpawnBrick(Vector2 pos) {
		// convert config coordinate to game world coordinates
		Vector2 gamePos = new Vector2(kTopLeft.x + (pos.x * kCfgPixel), + kTopLeft.y - (pos.y * kCfgPixel));

		// brick position is at its center. We want to position this brick's top left corner at the current
		// gamePos. Not pixel perfect but seems to look okay.
		gamePos.x += m_renderer.bounds.extents.x;
		gamePos.y -= m_renderer.bounds.extents.y;

		m_renderer.color = Color.cyan;
		Instantiate (m_brickPrefab, gamePos, Quaternion.identity);
	}
}
