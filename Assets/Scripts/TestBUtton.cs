using UnityEngine;
using System.Collections;

public class TestBUtton : MonoBehaviour {

	// Use this for initialization
	public Texture tex;
	Book card1;
	Book card2;
	Texture t;
	Book book1;

	void Start()
	{
		card1 = new Book ("robot",100);
		card2 = new Book("wrench",21);
	}

	// Update is called once per frame
	void OnGUI () {
		book1 = card2;
		t= Resources.Load (book1.img) as Texture;
		if (GUILayout.Button(t,GUILayout.Width(100)))
			Debug.Log(book1.ID.ToString());


		book1 = card1;
		t = Resources.Load (book1.img) as Texture;
		if (GUILayout.Button(t,GUILayout.Width(100)))
			Debug.Log(book1.ID.ToString());

	}
}


class Book: System.Object
{
	public string img; // 这个img貌似是临时定义的
	public int ID;

	public Book(string img, int id)
	{
		this.img = img;
		this.ID = id;
	}
	
	
}

