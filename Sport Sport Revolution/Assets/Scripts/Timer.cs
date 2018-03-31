using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

	//attach script to text object

	Text text;
	float timer = 0.0f;

	// Use this for initialization
	void Start () {
		text = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		text.text = timer.ToString("F1");
	}
}
