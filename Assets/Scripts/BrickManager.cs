using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Collections.Generic;

public class BrickManager : MonoBehaviour {

	// "Constants"
	Vector2 kTopLeft;
	const float kCfgPixel = 0.5f; // 1 cfg pixel is 0.5 game pixels
	Vector2 kDefaultScale;

	bool kPowOn = true;
	const string kPowerTagString = "Power";
	Color kPowerColor;

	// Amplitude Analytics
	const string kGameStartEvent = "Game Started";
	const string kGameOverEvent = "Game Ended";

	// State Management
	HashSet<Transform> m_brickSet;
	int m_ballCnt = 1;
	int m_numBallsLost = 0;

	bool isGameStarted = false;

	// Events
	public delegate void SpawnBall();
	public static event SpawnBall OnSpawnBall;

	[SerializeField]
	Transform m_brickPrefab;

	Vector2 m_brickSize;
	SpriteRenderer m_renderer;

	// Use this for initialization
	void Start () {
		kTopLeft = new Vector2 (-6.87f, 4.6f);
		kDefaultScale = new Vector2 (0.75f, 0.25f);
		kPowerColor = Color.yellow;
		m_brickSize = Vector2.one;
		m_brickSet = new HashSet<Transform> ();

		m_renderer = m_brickPrefab.GetComponent<SpriteRenderer> ();
		LoadBricksCfg ();
	}

	void Awake () {
		Amplitude amplitude = Amplitude.Instance;
		amplitude.logging = true;
		amplitude.init("cc59e0585575b5e3acb48067595423eb");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnEnable() {
		Brick.OnBrickDestroyed += Brick_OnBrickDestroyed;
		Pit.OnBallDestroyed += HandleBallDestroyed;
		PaddleScript.OnReleaseBall += GameStarted;
	}

	void OnDisable() {
		Brick.OnBrickDestroyed -= Brick_OnBrickDestroyed;
		Pit.OnBallDestroyed -= HandleBallDestroyed;
		PaddleScript.OnReleaseBall -= GameStarted;
	}

	void GameStarted()
	{
		if (!isGameStarted) {
			Amplitude.Instance.logEvent (kGameStartEvent);
			isGameStarted = true;
		}
	}

	void Brick_OnBrickDestroyed (Transform brick)
	{
		if (brick.tag == kPowerTagString) m_ballCnt++;

		m_brickSet.Remove (brick);
		if (m_brickSet.Count <= 0) {
			// enable gameobject text "GameOverText" to display the game over text
			GameObject.Find ("GameOverUI").transform.GetChild (0).gameObject.SetActive (true);

			Dictionary<string, object> gameStats = new Dictionary<string, object> () {
				{ "Balls Lost", m_numBallsLost }
			};
			Amplitude.Instance.logEvent (kGameOverEvent, gameStats);
		}
	}

	void HandleBallDestroyed() {
		m_ballCnt--;
		m_numBallsLost++;
		if (m_ballCnt < 1) {
			if (OnSpawnBall != null) {
				OnSpawnBall ();
			}
		}
	}

	/***
	 * Valid config line types:
	 * size=2,1
	 * [empty line]
	 * 1,1
	 * pow 1,1
	 * 
	 * Returns true on successful load and false on failure
	 */
	bool LoadBricksCfg() {
		try {
			TextReader reader = new StreamReader ("Assets/bricks.cfg");

			using (reader) {
				string line = reader.ReadLine();
				string[] values;
				Vector2 pos;

				while (line != null) {
					values = line.Split('=');
					if (values.Length > 0) {
						if (values[0] == "size") {
							pos = ParseVector2(values[1]);
							if (pos.x >= 1 && pos.y >= 1) m_brickSize = pos;

							m_renderer.transform.localScale = new Vector3(m_brickSize.x * kDefaultScale.x, m_brickSize.y * kDefaultScale.y, 0);
						} else if (values.Length == 1 && values[0].Length > 0) {
							// no '=' found and not a blank line

							values = line.Split(' ');
							if (values.Length > 1 && values[0] == "pow") {
								pos = ParseVector2(values[1]);
								if (pos.x >= 0 && pos.y >= 0) SpawnBrick(pos, kPowOn);
							}
							else {
								// a bit fragile right now, cannot have spaces in the coordinates
								// brick coordinate
								pos = ParseVector2(line);
								if (pos.x >= 0 && pos.y >= 0) SpawnBrick(pos);
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

	// vector must have the format: <number>,<number>
	// no spaces allowed!
	//
	// This will parse the string and return a Vector x,y with the values filled in.
	// Returns -1, -1 on error, as this is an invalid config file value.
	Vector2 ParseVector2(string v) {
		string[] arr = v.Split(',');
		int x = -1;
		int y = -1;

		if (arr.Length > 1) {
			x = Int32.Parse (arr [0]);
			y = Int32.Parse (arr [1]);
		}

		return new Vector2 (x, y);
	}

	void SpawnBrick(Vector2 pos, bool powOn = false) {
		// convert config coordinate to game world coordinates
		Vector2 gamePos = new Vector2(kTopLeft.x + (pos.x * kCfgPixel), + kTopLeft.y - (pos.y * kCfgPixel));

		// brick position is at its center. We want to position this brick's top left corner at the current
		// gamePos. Not pixel perfect but seems to look okay.
		gamePos.x += m_renderer.bounds.extents.x;
		gamePos.y -= m_renderer.bounds.extents.y;

		m_renderer.color = Color.cyan;
		Transform newObj = Instantiate (m_brickPrefab, gamePos, Quaternion.identity) as Transform;
		m_brickSet.Add (newObj);

		if (powOn) {
			newObj.tag = kPowerTagString;
			SpriteRenderer renderer = newObj.GetComponent<SpriteRenderer> ();
			renderer.color = kPowerColor;
		}
	}
}
