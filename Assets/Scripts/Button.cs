using UnityEngine;
using System.Collections;

public enum ButtonType {
	EnterMainMenu,
	EnterLevelSelect,
	EnterGame,
	EnterSkillTree,
	EndSkillTree
}

public class Button : MonoBehaviour {

	public ButtonType buttonType;

	public void OnClickButton() {
		switch(buttonType) {
		case ButtonType.EnterMainMenu:
			Application.LoadLevel("MainMenu");
			break;
		case ButtonType.EnterGame: 
			Application.LoadLevel("Level1");
			break;
		case ButtonType.EnterLevelSelect:
			Application.LoadLevel("LevelSelectMenu");
			break;
		case ButtonType.EnterSkillTree:
			Application.LoadLevel("SkillTree");
			break;
		case ButtonType.EndSkillTree:
			SkillTreeHandler.skillTreeHandler.saveSkillsToDisk();
			Application.LoadLevel("LevelSelectMenu");
			break;
		default:
			break;
		}
		//Application.Quit();
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
