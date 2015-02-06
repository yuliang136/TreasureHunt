using UnityEngine;
using System.Collections;

public class clockScript : MonoBehaviour {

	public static bool isPaused = false;
	float setTime;
	float timeRemaining;

	float percent;

	public Texture2D clockBG;
	public Texture2D clockFG;
	float clockFGMaxWidth;

	public Texture2D rightSide;
	public Texture2D leftSide;
	public Texture2D back;
	public Texture2D blocker;
	public Texture2D shiny;
	public Texture2D finished;

	public static bool bTimeisUp = false;

	public int gap = 40;

	public float startTime; // Record when the game is begin.

	void initialGame ()
	{
		bTimeisUp = false;
		isPaused = false;

		guiText.material.color = Color.black;

		switch (GlobeSet.GameLevel) 
		{
			case 0: setTime = 100.0f; break;
			case 1: setTime = 60.0f; break;
			case 2: setTime = 40.0f; break;
		}

		clockFGMaxWidth = clockFG.width;
		
		// timeRemaining = startTime - Time.time;
		startTime = Time.time;
		// 这里剩余时间也就是设定时间.
		timeRemaining = setTime;
	}

	// Use this for initialization
	void Start () {
		initialGame ();
	}

	void DoCountdown ()
	{
		// 这里的剩余时间 = 设定的总时间 - 游戏经过时间.
		// 游戏经过时间 = Time.time - 开始记录游戏开始的时间.
		timeRemaining = setTime - (Time.time - startTime);

		percent = timeRemaining / setTime * 100;

		ShowTime ();

		// Debug.Log ("time remaining = " + timeRemaining);

		if (timeRemaining < 0) {
				
			timeRemaining = 0;
			isPaused = true;
			TimeIsUp();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!isPaused) 
		{
			DoCountdown();		
		}
	}


	void PauseClock()
	{
		isPaused = true;
	}

	void UnpauseClock()
	{
		isPaused = false;
	}

	void ShowTime()
	{
		int minutes;
		int seconds;
		string timeStr;

		minutes = (int)timeRemaining / 60;
		seconds = (int)timeRemaining % 60;

		timeStr = minutes.ToString () + ":";
		timeStr += seconds.ToString("D2");

		guiText.text = timeStr;
	}

	void ShowTimeIsUp()
	{
		float winPromptW = 200f;
		float winPromptH = 90f;
		
		
		float halfScreenW = Screen.width / 2f;
		float halfScreenH = Screen.height / 2f;
		
		//Debug.Log (halfScreenW.ToString ());
		
		float halfPromptW = winPromptW / 2f;
		float halfPromptH = winPromptH / 2f;
		//Debug.Log (halfPromptW.ToString ());
		
		Rect r = new Rect(halfScreenW - halfPromptW, halfScreenH - halfPromptH, winPromptW, winPromptH);
		
		
		GUI.BeginGroup (r);
		
		
		GUI.Box (new Rect(0,0,(int)winPromptW,(int)winPromptH), "Time is Up!!");
		
		if (GUI.Button (new Rect (15, 40, 160, 40), "Play Again"))
		{
			Start ();

			Application.LoadLevel("game");		
		}
		
		
		GUI.EndGroup ();
	}

	void TimeIsUp()
	{
		bTimeisUp = true;

	}


	void OnGUI()
	{
		if (bTimeisUp) {
			ShowTimeIsUp();		
		}

		int pieClockX = 100;
		int pieClockY = 50;

		int pieClockW = 64;
		int pieClockH = 64;

		int pieClockHalfW = pieClockW / 2;
		int pieClockHalfH = pieClockH / 2;
		float newBarWidth = (percent / 100) * clockFGMaxWidth;


		GUI.BeginGroup (new Rect (0, 0, clockBG.width, clockBG.height));

		// BG作为背景不变.
		GUI.DrawTexture (new Rect(0,0,clockBG.width, clockBG.height), clockBG);

		// 在新的区域里画线条
		GUI.BeginGroup (new Rect (5, 6, newBarWidth, clockFG.height));

		GUI.DrawTexture (new Rect (0, 0, clockFG.width, clockFG.height), clockFG);

		GUI.EndGroup();
		GUI.EndGroup ();

	}

	void CircleTime()
	{
		
		/*
		
		bool isPastHalfway = (percent < 50);
		Rect clockRect = new Rect (pieClockX, pieClockY, pieClockW, pieClockH);
		
		// percent从0到100
		// rot从0度到360度
		float rot = (percent / 100) * 360;
		float xiuzheng;
		
		Vector2 centerPoint = new Vector2 (pieClockX + pieClockHalfW, pieClockY + pieClockHalfH);
		Matrix4x4 startMatrix = GUI.matrix;
		
		// 按顺序画画.
		
		// 先画出背景.
		GUI.DrawTexture (clockRect, back, ScaleMode.StretchToFill, true, 0);
		
		// 坐标轴 一直转动. 在不同的半球时，用不同的图案遮挡
		GUIUtility.RotateAroundPivot(-rot, centerPoint);	
		GUI.DrawTexture (clockRect, rightSide, ScaleMode.StretchToFill, true, 0);
		
		
		GUI.matrix = startMatrix;
		
		if (isPastHalfway) {
			// rot已经转动了180度
			// 之后加载到左半球的图案其实加载到了右边
			// 现在用右边的blocker挡住
			GUI.DrawTexture (clockRect, blocker, ScaleMode.StretchToFill, true, 0);
		}
		else {

			// rot开始从0度转动，转动到180度为一半
			// 先转动坐标轴.
			GUIUtility.RotateAroundPivot(-rot, centerPoint);

			// 在改变后的坐标轴上画图.
			GUI.DrawTexture (clockRect, rightSide, ScaleMode.StretchToFill, true, 0);

			// 不加这句背景也会转动
			// 调整回正常坐标轴
			GUI.matrix = startMatrix;

			// 画出左边半圆 覆盖了旋转的
			GUI.DrawTexture (clockRect, leftSide, ScaleMode.StretchToFill, true, 0);
		}
		
		if (percent < 0) 
		{
			// 如果结束 打上红饼
			GUI.DrawTexture(clockRect, finished, ScaleMode.StretchToFill, true, 0);		
		}
		
		// 加亮色渲染
		GUI.DrawTexture (clockRect, shiny, ScaleMode.StretchToFill, true, 0);
		
		*/
	}


}
