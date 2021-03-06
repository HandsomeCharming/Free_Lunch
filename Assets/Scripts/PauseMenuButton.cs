﻿using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public enum PauseMenuButtonType {
	Return = 0,
	Restart,
	ReturnToLevelSelect,
	BaseSetting,
	ReturnToMainMenu
}

public class PauseMenuButton : MonoBehaviour {
	public PauseMenuButtonType type;
	public Canvas pauseMenu;
	bool paused;
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
	}

	public void returnToGame() {
		Time.timeScale = 1f;
		pauseMenu.enabled = false;
		InputHandler.current.paused = false;
	}

	public void restart() {
		Time.timeScale = 1f;
		Application.LoadLevel(Application.loadedLevel);
	}

	public void returnToLevelSelect() {
		Time.timeScale = 1f;
		Application.LoadLevel("LevelSelectMenu");
	}

	public void returnToMainMenu() {
		Time.timeScale = 1f;
		Application.LoadLevel("MainMenu");
	}

	public void returnToSkillTree() {
		Time.timeScale = 1f;
		Application.LoadLevel("SkillTree");
	}

	public void goToNextLevel() {
		Application.LoadLevel("Boss1");
	}

	public void goSetting() {
		//Time.timeScale = 1f;
	}

	void OnMouseDown() {
		print(3);
	}

	void OnMouseOver() {
		if(Input.GetMouseButtonDown(0)) {

		}
		print(4);
	}
}