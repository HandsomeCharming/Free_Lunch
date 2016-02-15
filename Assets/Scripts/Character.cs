using UnityEngine;
using System.Collections;

//Don't modify this file by yourself!

public class CharacterStatus {
	public CharacterStatus() {
		hp = 100f;
		level = 0;
		defense = 0;
		defensePenetration = 0;
		moveSpeed = 0.6f;
		healthRegen = 0;
		facingDirection = new Vector2(0,0);
		canMove = true;
		canAttack = true;
		canUseSkill = true;
	}
	public float hp;
	public int level;
	public float defense;
	public float defensePenetration;
	public float moveSpeed;
	public float healthRegen;
	public Vector2 facingDirection;
	public bool canMove;
	public bool canAttack;
	public bool canUseSkill;
}

public class AttackModifier {
	int type;
}

public class ChargedAttackModifier {
	int type;
}

public class DodgeModifier {
	int type;
}

public class BlockModifier {
	int type;
}

public enum CharacterState {
	Stand = 0,
	Move,
	Attack,
	Block,
	Dodge,
	UseSkill,
	Stunned,
	BlowedAway
};

public enum CharacterSkillType {
	Attack = 0,
	ChargedAttack = 1,
	Block,
	Dodge,
	Active1,
	Active2,
	Active3,
	Active4,
	Active5,
	Active6
};

public class Character : MonoBehaviour {

	public Character() {
		state = CharacterState.Stand;
		status = new CharacterStatus();
		status.moveSpeed = 1f;
		type = 0;

		actionCds = new float[10];
		actionCdRemain = new float[10];

		for(int a=0;a!=10;++a)
			actionCds[a] = 0.3f;
		for(int a=0;a!=10;++a)
			actionCdRemain[a] = 0;
	}
	//Character type
	public int type;
	public CharacterStatus status;

	public CharacterState state;

	public AttackModifier attackModifiers;
	public ChargedAttackModifier chargedAttackModifiers;
	public DodgeModifier dodgeModifier;
	public BlockModifier blockModifier;
	public int[] passiveSkills;

	public int[] activeSkills;
	public int activeNum;

	//Cooldown for certain actions, when set to 0, can do action.
	// 0: attack
	// 1: chargedAttack
	// 2: dodge
	// 3: block
	// 4-10: actives
	protected float[] actionCdRemain; 
	protected float[] actionCds;
	
	public int[] negativeEffects;

	public virtual void moveTo(Vector2 pos) {}
	
	public virtual void moveToward(Vector2 dir) {
		Vector3 pos = gameObject.transform.position;
		pos.x += dir.x * status.moveSpeed;
		pos.z += dir.y * status.moveSpeed;
		transform.position = pos;
		//Vector3 velo = new Vector3(dir.x * status.moveSpeed, 0, dir.y * status.moveSpeed);
		//GetComponent<Rigidbody>().velocity = velo;
	}	

	public virtual void attackToward(Vector2 dir) {}

	public virtual void chargedAttackToward(Vector2 dir) {}

	public virtual void dodgeToward(Vector2 dir) {}

	public virtual void blockToward(Vector2 dir) {}

	//skill index is the index of active skills.
	public virtual void useSkillToward(int skillIndex, Vector2 dir) {}

	public virtual void hit(Character other, CharacterSkillType skillType) {}

	void Update() {
		if(actionCdRemain[0]>0) {
			actionCdRemain[0] -= Time.deltaTime;
		} 
		if(actionCdRemain[0] <= 0) actionCdRemain[0] = 0;
	}
}
