using UnityEngine;
using System.Collections;

public class GameGUI : MonoBehaviour {

	public GUISkin customSkin;
	public int myHeight = 0;


	public int posX = 0;
	public int posY = 52;
	public int textLength = 100;
	public int textHeight = 20;

	// Use this for initialization
	void Start () {
		// shuoming = "Click on the box TWO Times to find the right two parts of robot";
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI () {
		
		GUI.skin = customSkin;
		
		int buttonW = 150;
		int buttonH = 50;
		
		float halfScreenW = Screen.width / 2;
		float halfButtonW = buttonW / 2;
		
		if (GUI.Button (new Rect (posX, posY, buttonW, buttonH), "Title")) {
			Application.LoadLevel("title");
		}

		// 说明 
		//GUI.Label(new Rect(posX, posY ,textLength,textHeight), shuoming);

	}
}
