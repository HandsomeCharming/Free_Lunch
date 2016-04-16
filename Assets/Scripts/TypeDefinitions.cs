using UnityEngine;
using System.Collections;


// To Check types, see skill list.xlsx

public class CharacterStatus {
	public CharacterStatus() {
		hp = 100f;
		maxhp = 100f;
		level = 0;
		defense = 0;
		defensePenetration = 0;
		moveSpeed = 0.6f;
		regularMoveSpeed = 0.6f;
		healthRegen = 0;
		turningSpeed = 15f;
		facingDirection = new Vector2(1f,0);
		canMove = true;
		canAttack = true;
		canUseSkill = true;
	}
	public float hp;
	public float maxhp;
	public int level;
	public float defense;
	public float defensePenetration;
	public float moveSpeed;
	public float regularMoveSpeed;
	public float healthRegen;
	public float hitImmuneTime;
	public float hitImmuneTimeRemain;
	public float turningSpeed;
	public Vector2 facingDirection;
	public bool canMove;
	public bool canAttack;
	public bool canUseSkill;
}

public class Modifier {
	public int type; //Type of an modifier
	public int subTypeCount;  //Subtype of an modifier
	
	public float[] damage;
	public int negativeEffectCount = 0;
	public int[] negativeEffects;
	public int attackCount = 0;
	public int attackCountMax = 1;
}

public class AttackModifier : Modifier {
	public AttackModifier(){}
	public AttackModifier(int ptype) {
		type = ptype;
		switch(type) {
		case 3:
			subTypeCount = 1;
			damage = new float[1];
			damage[0] = 34f;
			break;
		case 4:
			subTypeCount = 1;
			damage = new float[1];
			damage[0] = 34f;
			negativeEffectCount = 1;
			negativeEffects = new int[1];
			negativeEffects[0] = 550;
			break;
		case 5:
			subTypeCount = 1;
			damage = new float[1];
			damage[0] = 34f;
			negativeEffectCount = 1;
			negativeEffects = new int[1];
			negativeEffects[0] = 552;
			break;
		default:
			break;
		}
	}
}

public class ChargedAttackModifier : Modifier {
	//public int type = 0;
	public ChargedAttackModifier(int type) {
		this.type = type;
		switch(type) {
		case 103:
			subTypeCount = 2;
			damage = new float[2] {30f,40f};
			break;
		case 115:
			subTypeCount = 1;
			damage = new float[1] {30f};
			chargeTime = 1f;
			break;
		case 116:
			subTypeCount = 1;
			damage = new float[1] {40f};
			chargeTime = 3f;
			break;
		default:
			break;
		}
	}

	public float chargeTime = 1.5f;
}

public class DodgeModifier : Modifier {
	//public int type = 0;
	public DodgeModifier(int type) {
		this.type = type;
		switch(type) {
		case 152:
			subTypeCount = 2;
			damage = new float[] {0f,0f};
			negativeEffectCount = 2;
			negativeEffects = new int[] {0, 556};
			break;
		case 153:
			break;
		default:
			break;
		}
	}
	public float dodgeTime = 0.5f;
}

public class BlockModifier : Modifier {
	public BlockModifier(int type) {
		this.type = type;
		switch(type) {
		case 200:
			subTypeCount = 1;
			blockTime = 1.0f;
			break;
		case 203:
			subTypeCount = 1;
			blockTime = 100.0f;
			break;
		case 204:
			subTypeCount = 1;
			blockTime = 100.0f;
			break;
		}
	}
	public float blockTime;
	//int type;
}

public class ActiveModifier : Modifier {
	public ActiveModifier(int type) {
		this.type = type;
		switch(type) {
		case 251:
			subTypeCount = 1;
			damage = new float[1];
			damage[0] = 0.5f;
			negativeEffectCount = 1;
			negativeEffects = new int[1];
			negativeEffects[0] = 557;
			break;
		case 258:
			subTypeCount = 1;
			damage = new float[1];
			damage[0] = 3f;
			existTime = 2f;
			break;
		}
	}
	public float existTime = 0f;
}

public class PassiveSkill {
	public int type;
}

public class TemporaryEffect {
	public TemporaryEffect(int ptype) {
		type = ptype;
		switch (type) {
		case 550:
			totalTime = remainTime = 5f;
			break;
		case 552:
			totalTime = remainTime = 2f;
			break;
		case 555:
			totalTime = remainTime = 2f;
			break;
		case 556:
			totalTime = remainTime = 1f;
			break;
		case 557:
			totalTime = remainTime = 5f;
			break;
		default:
			break;
		}
	}
	public int type;
	public int layer = 1;
	public int maxLayer = 1;
	public float dotCalTime = 0.3f;
	public float dotCalTimeRemain = 0.3f;
	public float totalTime;
	public float remainTime;

	public bool dotTimeout() {
		dotCalTimeRemain -= Time.deltaTime;
		if(dotCalTimeRemain <= 0) {
			dotCalTimeRemain = dotCalTime;
			return true;
		}
		return false;
	}
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
