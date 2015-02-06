using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameScript : MonoBehaviour {

	int cols; // 设定为列 多少数列
	int rows; // 设定为行 多少行
	//  int numCards;
	int matchesNeededToWin ;

	int matchesMade = 0;  // 已经配对成功的数量.

	int cardW = 80;
	int cardH = 120;


	List<Card> aCards;
	List<Card> aCardsFlipped;
	Card[,] aGrid; // 为记录对应位置 设置的对象. 
	List<string> preMatches;


	bool playerCouldClick;
	bool playerHasWon;

	// 播放声音对象.
	public AudioSource pew;
	public AudioSource backGround;

	// temp
	public int diaozhengH;

	// 时间停止.
	bool isTimePause = false;

	// GUI position.
	public int posX = 276;
	public int posY = 14;
	public int textLength = 1050;
	public int textHeight = 34;
	string shuoming = "Click on the boxes TWO Times to find the right two parts of robot";
	public GUISkin customSkin;

	// Use this for initialization
	// 初始化函数.
	void Start () 
	{
		cols 	= 6; // 设定为列 多少数列
		rows = 3; // 设定为行 多少行
		//numCards = 6 * 3; // 18
		matchesNeededToWin = cols * rows / 2;

		diaozhengH = 96;

		preMatches = new List<string> ();
		// 增加预设值的配对值.
		string strMatch = string.Empty;

		strMatch = "knightlongsword";
		preMatches.Add (strMatch);

		strMatch = "longswordknight";
		preMatches.Add (strMatch);

		strMatch = "longbowarcher";
		preMatches.Add (strMatch);

		strMatch = "archerlongbow";
		preMatches.Add (strMatch);

		strMatch = "wizardmagicwand";
		preMatches.Add (strMatch);

		strMatch = "magicwandwizard";
		preMatches.Add (strMatch);

		strMatch = "bardlute";
		preMatches.Add (strMatch);
		
		strMatch = "lutebard";
		preMatches.Add (strMatch);
		
		
		aCards = new List<Card>();
		aCardsFlipped = new List<Card> (); 
		
		playerCouldClick = true;
		playerHasWon = false;
		
		aGrid = new Card[rows, cols];
		
		// 为每个元素的img值分配不同的类型.
		// 来满足游戏的要求.
		// aCards里是放置的随机的16张可以配对的卡牌.
		
		
		BuildDeck ();
		
		// 初始化每个元素.
		// 把引用指向实际的对象.

		// test


		//print(yutest.ToString());

		// Unity内部奇怪的机制 导致出问题!!!
		for (int i = 0; i < rows; i++) 
		{
			for(int j = 0; j < cols; j++)
			{
				int yutest  = Random.Range(0, aCards.Count);
				// 在Deck里随机抽取卡牌
				// randomNum = Random.Range(0, aCards.Count);
				// print(randomNum.ToString());
				// aGrid是个引用 指向aCard里的元素.
				aGrid[i,j] = aCards[yutest];
				
				// 抽取后 删除原有元素
				aCards.RemoveAt(yutest);
			}
		}
		
		
	}

	// 根据card的状态和游戏状态 设置img
	string GetShowImg (Card card)
	{
		string strRtn = string.Empty;

		// card的状态还有Match Match后应该显示为空.
		if (card.isMatched) 
		{
			strRtn = string.Empty;
		} 
		else // 否则卡片还是需要继续处理
		{
			if (card.isFaceUp) 
			{
				strRtn = card.img;
			} 
			else
			{
				strRtn = "wenhao";
			}	
		}

		return strRtn;
	}

	// Update is called once per frame
	// 不停的调用界面的函数.
	void OnGUI () {
		
		GUI.skin = customSkin;


		// 处理声音逻辑
		/*

		// 播放声音.
		if (backGround.isPlaying == false) 
		{
			backGround.Play ();
		}
		*/
		
		/*
		bool test = clockScript.bTimeisUp;
		if (test == true) 
		{
			print("In GameScript.  Get isTimeUp");
		}
		*/
		//GUI.Label(new Rect(posX, posY ,textLength,textHeight), shuoming);


		// 按键区域的设定，把区域放在下部.


		
		// 画框函数。
		// 不停地画出Grid框
		// BuildGrid ();
		
		//BuildWinPrompt ();



		//GUILayout.BeginVertical ();
		//GUILayout.FlexibleSpace();


		// 在这里画18个框. 再来调整位置.
		// 布局格式里最好抽出函数处理逻辑.
		GUILayout.BeginArea (new Rect(0,diaozhengH,Screen.width, Screen.height));


		GUILayout.BeginVertical ();
		GUILayout.FlexibleSpace();
		// 用了2维数组.
		for (int i = 0; i < rows; i++) 
		{
			GUILayout.BeginHorizontal ();
			GUILayout.FlexibleSpace ();
			
			for (int j = 0; j < cols; j++)
			{
				// 1 提取出aGrid[i,j]里指向Card元素里的img属性

				string strShow = GetShowImg(aGrid[i,j]);
				Texture2D t = Resources.Load(strShow) as Texture2D;

				if(GUILayout.Button(t, GUILayout.Width(cardW),GUILayout.Height(cardH)))
				{
					//string strIM = string.Format("{0}_{1}",i,j);
					//print(strIM);

					// aGrid[i,j].isFaceUp = true;

					// 如果玩家可以继续点击 才执行翻牌操作
					if(playerCouldClick)
					{
						FlipCardFaceUp(aGrid[i,j]);
					}


				}


			}

			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();

		}
		GUILayout.FlexibleSpace();
		GUILayout.EndVertical ();

		GUILayout.EndArea ();


		// 胜利条件判断
		if (playerHasWon) 
		{
			// 停止计数.
			clockScript.isPaused = true;
			
			BuildWinPrompt ();
		}
	}



	// 为Deck随机安排卡片函数.
	void BuildDeck ()
	{

		//  设置放置多少牌. 18
		// 18 / 2 = 9 次配对牌

		// 一共多少个角色 4

		List<string> aCharacters = new List<string>{"knight", "archer", "wizard","bard"};

		int nknight 	= 0;
		int narcher		= 0;
		int nwizard 	= 0;
		int nbard 		= 0;

		// 卡牌
		Card each;

		string character = string.Empty;
		string weapon = string.Empty;

		for (int i = 0; i < (cols * rows/2); i++) 
		{
			// 首先遍历查看这些整数，如果为0 则加入.
			// 首先选出角色.
			if((0 == nknight) || (0 == narcher) || (0 == nwizard) || (0 == nbard))
			{
				if(0 == nknight)
				{
					character = "knight";
					each = new Card(character, 0);
					nknight++;
				}else if(0 == narcher) 
				{
					character = "archer";
					each = new Card(character, 0);
					narcher++;
				}else if(0 == nwizard)
				{
					character = "wizard";
					each = new Card(character, 0);
					nwizard++;
				}else if(0 == nbard)
				{
					character = "bard";
					each = new Card(character, 0);
					nbard++;
				}

			}
			else
			{
				// 用随机数求出角色后加入.
				int someNum = Random.Range(0, aCharacters.Count);
				character= aCharacters[someNum];


				// 这里暂时不需要计数.
			}
			each = new Card(character, 0);
			aCards.Add(each); // 加入人物

			// 根据角色选出武器.
			switch(character)
			{
				case "knight":weapon = "longsword";break;
				case "archer":weapon = "longbow";break;
				case "wizard":weapon = "magicwand";break;
				case "bard":weapon = "lute";break;

			}
			each = new Card(weapon,0);
			aCards.Add(each); // 加入和人物配对的武器


		}




		/*
		int totalRobots = 4; // 一共只有4种类型的机器人.
		Card each;
		int id = 0;

		// 循环四次 每种机器人取一次
		// 内循环里 随机取每种机器人的某个部件.
		for (int i = 0; i < totalRobots; i++) 
		{
			List<string> aRobotParts = new List<string>{"Head", "Arm", "Leg"};
			for(int j = 0; j < 2; j++)
			{
				int someNum = Random.Range(0, aRobotParts.Count);
				string theMissingPart = aRobotParts[someNum];

				aRobotParts.RemoveAt(someNum);

				each = new Card("robot" + (i+1) + "Missing" + theMissingPart, id);
				aCards.Add(each);

				each = new Card("robot" + (i+1)  + theMissingPart, id);
				aCards.Add(each);

				id++;
			}
		}
		*/
	}



	// 卡片翻开后 判断函数
	void FlipCardFaceUp (Card each)
	{
		each.isFaceUp = true;


		// 判断如果是选择的同一张图. 不加入aCardsFlipped.

		if (aCardsFlipped.IndexOf (each) < 0) 
		{
			aCardsFlipped.Add (each);
		}

		//Debug.Log (aCardsFlipped.Count.ToString());

		if (aCardsFlipped.Count == 2)
		{

			// 如果打开了2个卡片时. 需要显示2个图片后
			// 再关闭2个图片

			// 如果有2个对象加入.　不能再按键．
			playerCouldClick = false;

			// 显示2个图片数秒后 在MyMethod函数里 再把条件设置回去.
			StartCoroutine(WaitFlip());
		}


}

	// 判断2张卡是否匹配.
	bool isRightTwoCards (Card card, Card card2)
	{
		// 组合后 在preMatches里查找
		string strThis = string.Format ("{0}{1}", card.img, card2.img);

		Debug.Log (strThis);

		return preMatches.Contains (strThis);
	}

	// 和JavaScript不同 在异步的控制里 控制住关键条件.
	// 在IEnumerator里 函数必须等待执行.
	// 翻开后 等待逻辑处理函数
	IEnumerator WaitFlip() {
		yield return new WaitForSeconds(0.8f);

		//Debug.Log("Get 1 seconds");

		// 再把卡翻过去.

		//  判断2张牌是否匹配.
		bool bCheck = isRightTwoCards (aCardsFlipped [0], aCardsFlipped [1]);


		if (bCheck) 
		{
			aCardsFlipped [0].isMatched = true;
			aCardsFlipped [1].isMatched = true;

			// 发音逻辑
			/*
			// 判断机器人的种类来决定播放的声音.
			string name = aCardsFlipped[0].img;
			char sortofRobot = name[5];

			string strSound = string.Empty;
			// 1 yellow.
			// 2 red
			// 3 blue
			// 4 green

			Debug.Log(sortofRobot.ToString());

			switch(sortofRobot)
			{
				case '1': strSound = "Yellow_out";break;
				case '2': strSound = "Red_out";break;
				case '3': strSound = "Blue_out";break;
				case '4': strSound = "Green_out";break;
			}

			Debug.Log(strSound);

			AudioClip a = Resources.Load(strSound) as AudioClip;
			pew.clip = a;

			pew.Play();

			*/

			matchesMade ++;
			if(matchesMade >= matchesNeededToWin)
			{
				playerHasWon = true;
			}

		} 
		else
		{
			aCardsFlipped[0].isFaceUp = false;
			aCardsFlipped[1].isFaceUp = false;
		}

		aCardsFlipped.Clear();
		
		playerCouldClick = true;
	}


	// 游戏不停循环执行。
	// 循环不断更新界面
	void BuildGrid ()
	{
		/*
		GUILayout.BeginVertical ();
		GUILayout.FlexibleSpace();
		// 用了2维数组.
		for (int i = 0; i < rows; i++) 
		{
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();

			for(int j = 0; j < cols; j++)
			{
				Card each = aGrid[i,j];

				// 用Resources.load读取一个资源 转化为Texture类型
				string strShow = each.img;
				// GUILayout.Button 起作用时， 按钮和图片已经放置.

				if(each.isMatched)
				{
					strShow = "blank";
				}
				else
				{
					if(each.isFaceUp)
					{
						strShow = each.img;
					}
					else
					{
						strShow = "wrench";
					}
				}

				// 如何定位是哪个按钮被按下去的呢
				// 如何把button和each对象关联起来的呢....
				Texture t = Resources.Load(strShow) as Texture;

				// 增加逻辑，如何时间结束. 则不能点击.
				if(true == clockScript.bTimeisUp)
				{
					GUI.enabled = false;
				}
				else
				{
					GUI.enabled = !each.isMatched;
				}

				if(GUILayout.Button(t, GUILayout.Width(cardW)))
				{

					if(playerCanClick)
					{
						// 播放声音.


						// 翻开图片.
						FlipCardFaceUp(each);
						//Debug.Log(each.img);
					}

				}
				GUI.enabled  = true;

			}

			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
		}

		GUILayout.FlexibleSpace();
		GUILayout.EndVertical ();

	*/






	}

	#region 提示框函数

	// 胜利时，创建一个提示框
	void BuildWinPrompt ()
	{
		float winPromptW = 150f;
		float winPromptH = 90f;
		
		
		float halfScreenW = Screen.width / 2f;
		float halfScreenH = Screen.height / 2f;
		
		//Debug.Log (halfScreenW.ToString ());
		
		float halfPromptW = winPromptW / 2f;
		float halfPromptH = winPromptH / 2f;
		//Debug.Log (halfPromptW.ToString ());
		
		Rect r = new Rect(halfScreenW - halfPromptW, halfScreenH - halfPromptH, winPromptW, winPromptH);
		
		
		GUI.BeginGroup (r);
		
		
		GUI.Box (new Rect(0,0,(int)winPromptW,(int)winPromptH), "A Winner is You!!");
		
		if (GUI.Button (new Rect (40, 40, 80, 40), "Play Again"))
		{
			Application.LoadLevel("Title");		
		}
		
		
		GUI.EndGroup ();
		
	}




	#endregion


	

}



// 卡片类.
class Card: System.Object
{
	public bool isFaceUp = false; // check if the card is faced up.  如果faceup为true 才显示，否则显示背景.
	public bool isMatched = false; // set if the card is matched already.
	public string img; // 这个img貌似是临时定义的
	public int id; // 为了区分每一张卡片


	public Card(string img, int id)
	{
		this.img = img;
		this.id = id;
	}


}