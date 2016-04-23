﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FinishPoint : MonoBehaviour {

	public int type;  //0: level 1: chapter

	public Text text;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider col) {
		//text.text = "You win!";
		if(col.tag == "Player") {
			InputHandler.current.stopInput();
			if(type == 0) {
				LevelCompleteMenu menu = ((GameObject)Instantiate(Resources.Load("Prefabs/LevelComplete"))).GetComponent<LevelCompleteMenu>();
			} else {
				LevelCompleteMenu menu = ((GameObject)Instantiate(Resources.Load("Prefabs/ChapterComplete"))).GetComponent<LevelCompleteMenu>();
			}
		}

	}
}
