using UnityEngine;
using System.Collections;

//Don't modify this file by yourself!

public class CharacterStatus {
	float hp;
	int level;
	float defense;
	float defensePenetration;
	float moveSpeed;
	float healthRegen;
	Vector2 facingDirection;
	bool canMove;
	bool canAttack;
	bool canUseSkill;
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
	public float[] activeCds;
	public int activeNum;

	public int[] negativeEffects;

	public virtual void moveTo(Vector2 pos) {}
	
	public virtual void moveToward(Vector2 dir) {
		Vector3 pos = gameObject.transform.position;
		pos.x += dir.x;
		pos.z += dir.y;
		transform.position = pos;
	}	

	public virtual void attackToward(Vector2 dir) {
		Projectile.ShootProjectile(this, dir, ProjectileType.Attack);
	}

	public virtual void chargedAttackToward(Vector2 dir) {}

	public virtual void dodgeToward(Vector2 dir) {}

	public virtual void blockToward(Vector2 dir) {}

	//skill index is the index of active skills.
	public virtual void useSkillToward(int skillIndex, Vector2 dir) {}

	public virtual void hit(Character other, CharacterSkillType skillType) {}

	void Start() {
		state = CharacterState.Stand;
	}
}
