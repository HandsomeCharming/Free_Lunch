using UnityEngine;
using System.Collections;
using System.IO;

[System.Serializable] 
public class Skill {
	public int skillType {get; set;}
	public int skillCount {get; set;}

	public Skill() { skillType=0; skillCount=0;}
}

[System.Serializable] 
public class SkillTree {
	public Skill[] skills;
	public SkillDiv skillDiv = SkillDiv.Division;
	public int skillLevel = 0;

	public SkillTree() {
		skills = new Skill[10];
	}
}

// Skill Types:
// Division:
// 0: attack apply dot, 1: attack slow
// 2: Seed  3: graves ult
// 4: Dodge slow 5: dodge reduce cd
// 6: Active1 ice ring, 7: Active 1 division
// 8: Amplify damage when attack slowed enemy 9: Apply dot to each enemy hit by seed
// 10: Active2 ice cone 11: Active2 Spreadable dot
// 12: Sit down ascendence  13: Immune
// 14: Add dot damage 15: Chance to cast a second time

public class SkillTreeHandler : MonoBehaviour {

	public static SkillTreeHandler skillTreeHandler;

	SkillTree skilltree;
	
	// Use this for initialization
	void Start () {
		skillTreeHandler = this;
		if(!loadSkillsFromDisk()) {
			skilltree = new SkillTree();
		} else {
			skilltree = GameSave.current.skilltree;
		}
		for(int a=0;a!=transform.childCount;++a) {
			SkillButton button = transform.GetChild(a).GetComponent<SkillButton>();
			for(int b=0;b!=skilltree.skillLevel;++b) {
				if(skilltree.skills[b].skillType == button.skillType) {
					button.skillCount = skilltree.skills[b].skillCount;
					button.LoadedClick();
					break;
				}
			}
		}
	}

	public bool clickSkill(SkillButton button) {
		if(skilltree.skillLevel != button.skillLevel || button.skillCount >= button.skillCountMax) {
			return false;
		}
		button.skillCount++;
		if(button.skillCount == button.skillCountMax) {
			skilltree.skills[skilltree.skillLevel] = new Skill();
			skilltree.skills[skilltree.skillLevel].skillType = button.skillType;
			skilltree.skills[skilltree.skillLevel].skillCount = button.skillCount;
			skilltree.skillLevel++;
		}
		return true;
	}

	public bool rightClickSkill(SkillButton button) {
		if(skilltree.skillLevel-1 != button.skillLevel || button.skillCount == 0) {
			return false;
		}
		button.skillCount--;
		if(button.skillCount == 0) {
			skilltree.skillLevel--;
		}
		return true;
	}

	public bool saveSkillsToDisk() {
		GameSave save;
		if(GameSave.current == null) 
			save = new GameSave();
		else 
			save = GameSave.current;
		Debug.Log("saved");
		save.skilltree = skilltree;
		SaveLoad.Save();
		return true;
	}

	bool loadSkillsFromDisk() {
		if(SaveLoad.Load()) {
			Debug.Log("loaded");
			GameSave save = GameSave.current;
			skilltree = save.skilltree;
			return true;
		}
		return false;
	}
}
