using UnityEngine;
using System.Collections;

public class TestWait : MonoBehaviour {

	// Use this for initialization
	void Start () {

		StartCoroutine(MyMethod());
			
	}

	IEnumerator MyMethod() {
		Debug.Log("Before Waiting 2 seconds");
		yield return new WaitForSeconds(5);
		Debug.Log("After Waiting 2 Seconds");
	}

	// Update is called once per frame
	void Update () {
	
	}
}
