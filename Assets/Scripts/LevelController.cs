﻿using UnityEngine;
using System.Collections;

public class LevelController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Projectile.InitProjectileData();

		Instantiate(Resources.Load("Prefabs/InputHandler"));
		Instantiate(Resources.Load("Prefabs/AIController"));

		//GameObject pobj = (GameObject)Instantiate(Resources.Load("Prefabs/DivisionCharacterB"));
		Character player = loadPlayer();
		MainCamera.current.character = player;
		InputHandler.current.character = player;
		AIController.current.player = player;
	}

	Character loadPlayer() {
		return ((GameObject)Instantiate(Resources.Load("Prefabs/DivisionCharacterB"))).GetComponent<Character>();
	}

	// Update is called once per frame
	void Update () {
	
	}
}