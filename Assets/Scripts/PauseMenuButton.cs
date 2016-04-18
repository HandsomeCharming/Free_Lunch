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

	public void restart() {
		Application.LoadLevel(Application.loadedLevel);
	}

	public void returnToLevelSelect() {
		Application.LoadLevel("LevelSelectMenu");
	}

	public void goSetting() {

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
