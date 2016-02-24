using UnityEngine;
using System.Collections;


// To Check types, see skill list.xlsx

public class CharacterStatus {
	public CharacterStatus() {
		hp = 100f;
		level = 0;
		defense = 0;
		defensePenetration = 0;
		moveSpeed = 0.6f;
		regularMoveSpeed = 0.6f;
		healthRegen = 0;
		facingDirection = new Vector2(1f,0);
		canMove = true;
		canAttack = true;
		canUseSkill = true;
	}
	public float hp;
	public int level;
	public float defense;
	public float defensePenetration;
	public float moveSpeed;
	public float regularMoveSpeed;
	public float healthRegen;
	public Vector2 facingDirection;
	public bool canMove;
	public bool canAttack;
	public bool canUseSkill;
}

public class AttackModifier {
	public AttackModifier(){}
	public AttackModifier(int ptype) {
		type = ptype;
		switch(type) {
		case 3:
			damage = 34f;
			break;
		default:
			break;
		}
	}

	public int type;
	public float damage;
	public int negativeEffectCount;
	public int negativeEffects;
}

public class ChargedAttackModifier {
	public int type = 0;
	public float chargeTime = 1.5f;
}

public class DodgeModifier {
	public int type = 0;
	public float dodgeTime = 1f;
}

public class BlockModifier {
	int type;
}

public enum CharacterState {
	Stand = 0,
	Move,
	Attack,
	ChargedAttack,
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
