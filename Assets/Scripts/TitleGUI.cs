using UnityEngine;
using System.Collections;

public class TitleGUI : MonoBehaviour {

	public GUISkin customSkin;

	public int myHeight = 520;


	public int textLength = 687;
	public int textHeight = 133;
	public int textposY = 415;
	public int textposX = 136;

	string shuoming;

	// Use this for initialization
	void Start () {
		shuoming = "In laboratory of Professor Wrecker, A storm broke all the" +
						" robots apart into many box. Find out the right two parts to" +
						" repair all the robots within limited time.";
	}
	
	// Update is called once per frame
	void OnGUI () {

		GUI.skin = customSkin;

		int buttonW = 150;
		int buttonH = 50;

		float halfScreenW = Screen.width / 2;
		float halfButtonW = buttonW / 2;

		if (GUI.Button (new Rect (halfScreenW - halfButtonW - 200, myHeight, buttonW, buttonH), "EASY")) {
			GlobeSet.GameLevel = 0;
			Application.LoadLevel("game");
		}
		if (GUI.Button (new Rect (halfScreenW - halfButtonW, myHeight, buttonW, buttonH), "NORMAL")) {
			GlobeSet.GameLevel = 1;
			Application.LoadLevel("game");
		}
		if (GUI.Button (new Rect (halfScreenW - halfButtonW + 200, myHeight, buttonW, buttonH), "HARD")) {
			GlobeSet.GameLevel = 2;
			Application.LoadLevel("game");
		}


		GUI.Label(new Rect(textposX, textposY ,textLength,textHeight), shuoming);
	}
}
