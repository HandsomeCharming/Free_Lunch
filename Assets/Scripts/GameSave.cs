using UnityEngine;
using System.Collections;

[System.Serializable] 
public class GameSave {
	public static GameSave current;

	public GameSave() {
		characterLevel = 0;
		skilltree = new SkillTree();
	}
	public int characterLevel = 0;
	public SkillTree skilltree;
}
