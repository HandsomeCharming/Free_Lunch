using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public enum PauseMenuButtonType {
	Return = 0,
	Restart,
	ReturnToLevelSelect,
	BaseSetting
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
}
