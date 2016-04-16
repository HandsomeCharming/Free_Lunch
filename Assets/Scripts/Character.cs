using UnityEngine;
using System.Collections;

//Don't modify this file by yourself!



public class Character : MonoBehaviour {

	public Character() {
		state = CharacterState.Stand;
		status = new CharacterStatus();
		status.moveSpeed = 30f;
		status.regularMoveSpeed = 20f;
		type = 0;

		actionCds = new float[10];
		actionCdRemain = new float[10];

		for(int a=0;a!=10;++a)
			actionCds[a] = 0.3f;
		for(int a=0;a!=10;++a)
			actionCdRemain[a] = 0;

		passiveSkills = new ArrayList();
		tempEffects = new ArrayList();
	}
	//Character type
	public int type;
	public CharacterStatus status;

	public CharacterState state;

	public AttackModifier attackModifier;
	public ChargedAttackModifier chargedAttackModifier;
	public DodgeModifier dodgeModifier;
	public BlockModifier blockModifier;

	public ArrayList passiveSkills;  //Permenant passive skills
	public ArrayList tempEffects;    //Temporary effects

	public ArrayList activeSkills;
	public int activeNum;

	//Cooldown for certain actions, when set to 0, can do action.
	// 0: attack
	// 1: chargedAttack
	// 2: dodge
	// 3: block
	// 4-10: actives
	public float[] actionCdRemain {get;set;}
	public float[] actionCds {get;set;}

	public int getSkillType(CharacterSkillType skillType) {
		switch (skillType) {
		case CharacterSkillType.Attack:
			return attackModifier.type;
		case CharacterSkillType.ChargedAttack:
			return chargedAttackModifier.type;
		case CharacterSkillType.Dodge:
			return dodgeModifier.type;
		case CharacterSkillType.Block:
			return blockModifier.type;
		case CharacterSkillType.Active1:
			return ((ActiveModifier)activeSkills[0]).type;
		case CharacterSkillType.Active2:
			return ((ActiveModifier)activeSkills[1]).type;
		}
		return 0;
	}

	public virtual void stand() {
		GetComponent<Rigidbody>().velocity = Vector3.zero;
	}

	public virtual void moveTo(Vector2 pos) {}
	
	public virtual void moveToward(Vector2 dir) {
		Vector3 pos = gameObject.transform.position;
		pos.x += dir.x * status.moveSpeed;
		pos.z += dir.y * status.moveSpeed;
		//transform.position = pos;
		Vector3 velo = new Vector3(dir.x * status.moveSpeed, 0, dir.y * status.moveSpeed);
		GetComponent<Rigidbody>().velocity = velo;
	}	

	public virtual void faceToward(Vector2 dir) {
		//status.facingDirection = dir;
		float angle = Vector2.Angle(new Vector2(1f, 0), dir); 
		angle = dir.y>0?angle:-angle;
		float facingAngle = Vector2.Angle(new Vector2(1f, 0), status.facingDirection); 
		facingAngle = status.facingDirection.y>0?facingAngle:-facingAngle;

		float abs = Mathf.Abs(facingAngle - angle);
		if(abs > 360f) abs -= 360f;

		float lerpAngle = Mathf.LerpAngle(facingAngle, angle, abs<=status.turningSpeed?1:status.turningSpeed/(abs)); 
		status.facingDirection.x = Mathf.Cos(lerpAngle*Mathf.Deg2Rad);
		status.facingDirection.y = Mathf.Sin(lerpAngle*Mathf.Deg2Rad);

		this.transform.rotation = Quaternion.Euler(0, -lerpAngle, 0);
	}

	public virtual void faceTowardWithoutLerp(Vector2 dir) {
		//status.facingDirection = dir;
		float angle = Vector2.Angle(new Vector2(1f, 0), dir); 
		angle = dir.y>0?angle:-angle;
		float facingAngle = Vector2.Angle(new Vector2(1f, 0), status.facingDirection); 
		facingAngle = status.facingDirection.y>0?facingAngle:-facingAngle;

		float abs = Mathf.Abs(facingAngle - angle);
		if(abs > 360f) abs -= 360f;

		status.facingDirection.x = dir.x;
		status.facingDirection.y = dir.y;

		this.transform.rotation = Quaternion.Euler(0, -angle, 0);
	}

	public virtual void attackToward(Vector2 dir) {

	}

	public virtual void attack(){}

	public virtual void startCharging() {
	}

	public virtual void stop() {
		GetComponent<Rigidbody>().velocity = Vector3.zero;
	}

	public virtual void chargedAttack(float chargingTime) {
		print("Charged Attack");
	}

	public virtual void useSkill(int skillIndex) {}

	public virtual void block() {}

	public virtual void chargedAttackToward(Vector2 dir) {}

	public virtual void dodgeToward(Vector2 dir) {

	}

	public virtual void blockToward(Vector2 dir) {}

	//skill index is the index of active skills.
	public virtual void useSkillToward(int skillIndex, Vector2 dir) {}

	public virtual void gotHit(Character other, float damage) {
		if(state == CharacterState.Dodge || (state == CharacterState.Block && blockModifier.type == 204))return;
		status.hp -= damage;
	}

	public virtual void gotHit(Character other, Modifier modifier, int subType) {
		if(state == CharacterState.Dodge || (state == CharacterState.Block && blockModifier.type == 204))return;
		status.hp -= modifier.damage[subType];
		if(modifier.negativeEffectCount > 0) {
			for(int a=0;a!=modifier.negativeEffectCount;++a) {
				other.applyTemporaryEffect(modifier.negativeEffects[a]);
			}
		}
	}

	public virtual void hit(Character other, CharacterSkillType skillType, int subType = 0) {

	}

	public virtual void applyTemporaryEffect(int type) {
		if(type <500)return;
		foreach (TemporaryEffect t in tempEffects) {
			if(t.type == type)return;
		}
		TemporaryEffect tempEffect = new TemporaryEffect(type);
		tempEffects.Add(tempEffect);
	}

	public virtual void removeTemporaryEffect(int type) {
		for(int a=0;a<tempEffects.Count;++a) {
			TemporaryEffect tempEffect = (TemporaryEffect) tempEffects[a];
			if(tempEffect.type == type) {
				tempEffects.Remove(tempEffect);
			}
		}
	}

	public virtual void calculateSpeed() {
		float scale = 1f;
		for(int a=0;a<tempEffects.Count;++a) {
			TemporaryEffect tempEffect = (TemporaryEffect) tempEffects[a];
			switch(tempEffect.type ) {
			case 552:
				scale *= 0.5f;
				break;
			case 555:
				scale *= 0.5f;
				break;
			case 556:
				scale *= 0.5f;
				break;
			case 557:
				scale *= 0f;
				break;
			default:
				break;
			}
		}
		status.moveSpeed = status.regularMoveSpeed * scale;
	}

	public virtual void calculateDot() {
		for(int a=0;a<tempEffects.Count;++a) {
			TemporaryEffect tempEffect = (TemporaryEffect) tempEffects[a];
			switch(tempEffect.type ) {
			case 550:
				if(tempEffect.dotTimeout()) {
					status.hp -= 5f;
				}
				break;
			default:
				break;
			}
		}
	}

	public void die() {
		if(AIController.current.characters.Contains(this)) {
			AIController.current.characters.Remove(this);
		}
		Destroy(this.gameObject);
	}

	protected void Update() {
		if(status.hp <= 0f) {
			die();
			return;
		}
		for(int a=0;a!=10;++a) {
			if(actionCdRemain[a]>0) {
				actionCdRemain[a] -= Time.deltaTime;
			} 
			if(actionCdRemain[a] <= 0) actionCdRemain[a] = 0;
		}
		calculateSpeed();
		calculateDot();
		for(int a=0;a<tempEffects.Count;++a) {
			TemporaryEffect tempEffect = (TemporaryEffect)tempEffects[a];
			tempEffect.remainTime -= Time.deltaTime;
			if(tempEffect.remainTime <= 0f) {
				tempEffects.Remove(tempEffect);
			}
		}
	}
}
