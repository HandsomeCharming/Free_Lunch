using UnityEngine;
using System.Collections;
using System.IO;

[System.Serializable] 
public class Skill {
	public int skillType {get; set;}
	public int skillCount {get; set;}

	public Skill() { skillType=0; skillCount=0;}
	public Skill(int type) {skillType = type;}
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
	public GameObject skillTreeButtons;
	public SkillSlot slots;
	
	// Use this for initialization
	void Start () {
		skillTreeHandler = this;
		if(!loadSkillsFromDisk()) {
			skilltree = new SkillTree();
		} else {
			skilltree = GameSave.current.skilltree;
		}
		/*for(int a=0;a!=transform.childCount;++a) {
			SkillButton button = transform.GetChild(a).GetComponent<SkillButton>();
			for(int b=0;b!=skilltree.skillLevel;++b) {
				if(skilltree.skills[b].skillType == button.skillType) {
					button.skillCount = skilltree.skills[b].skillCount;
					button.LoadedClick();
					break;
				}
			}
		}*/
	}

	public bool checkClicked(int skillType) {
		for(int a=0;a!=transform.childCount;++a) {
			SkillButton button = transform.GetChild(a).GetComponent<SkillButton>();
			if(button.skillType == skillType) {
				if(button.skillCount == button.skillCountMax)
					return true;
				else 
					return false;
			}
		}
		return false;
	}

	public bool clickSkill(SkillButton button) {
		skilltree.skills[button.skillLevel] = new Skill(button.skillType);
		SkillButton[] buttons = skillTreeButtons.GetComponentsInChildren<SkillButton>();
		foreach (SkillButton b in buttons) {
			if(b.skillLevel == button.skillLevel && b.selected == false) {
				b.disableClick();
			}
		}
		saveSkillsToDisk();
		return true;
	}

	public bool removeSkill(SkillButton button) {
		skilltree.skills[button.skillLevel] = null;
		SkillButton[] buttons = skillTreeButtons.GetComponentsInChildren<SkillButton>();
		foreach (SkillButton b in buttons) {
			if(b.skillLevel == button.skillLevel && b.selected == false) {
				b.enableClick();
			}
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
			putSkillsAfterLoad();
			return true;
		}
		return false;
	}

	void putSkillsAfterLoad() {
		SkillButton[] buttons = skillTreeButtons.GetComponentsInChildren<SkillButton>();
		for(int a=0;a!=8;++a) {
			if(skilltree.skills[a] == null)return;
			foreach (SkillButton b in buttons) {
				if(b.skillLevel == a && b.skillType == skilltree.skills[a].skillType) {
					b.transform.position = slots.slots[a].transform.position;
				} else if(b.skillLevel == a) {
					b.disableClick();
				}
			}
		}
	}
}
