using UnityEngine;
using System.Collections;
using System.IO;

public class Skill {
	public int skillType {get; set;}
	public int skillCount {get; set;}

	public Skill() { skillType=0; skillCount=0;}
}

public class SkillTreeHandler : MonoBehaviour {
	Skill[] skills;
	int skillCount = 0;
	int skillLevel = 0;
	// Use this for initialization
	void Start () {
		if(!loadSkillsFromDisk()) {
			skills = new Skill[10];
			skillLevel = 0;
			skillCount = 0;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public bool clickSkill(SkillButton button) {
		if(skillLevel != button.skillLevel || button.skillCount >= button.skillCountMax) {
			return false;
		}
		button.skillCount++;
		if(button.skillCount == button.skillCountMax) {
			skills[skillLevel] = new Skill();
			skills[skillLevel].skillType = button.skillType;
			skills[skillLevel].skillCount = button.skillCount;
			skillLevel++;
		}
		return true;
	}

	public bool rightClickSkill(SkillButton button) {
		if(skillLevel-1 != button.skillLevel || button.skillCount == 0) {
			return false;
		}
		button.skillCount--;
		if(button.skillCount == 0) {
			skillLevel--;
		}
		return true;
	}

	bool loadSkillsFromDisk() {
		return false;
	}
}
