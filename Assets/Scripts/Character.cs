using UnityEngine;
using System.Collections;

//Don't modify this file by yourself!



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

	public virtual void faceToward(Vector2 dir) {
		//status.facingDirection = dir;
		float angle = Vector2.Angle(new Vector2(1f, 0), dir); 
		angle = dir.y>0?angle:-angle;
		float facingAngle = Vector2.Angle(new Vector2(1f, 0), status.facingDirection); 
		facingAngle = status.facingDirection.y>0?facingAngle:-facingAngle;

		float abs = Mathf.Abs(facingAngle - angle);
		if(abs > 360f) abs -= 360f;

		float lerpAngle = Mathf.LerpAngle(facingAngle, angle, abs<=15f?1:15f/(abs)); 
		status.facingDirection.x = Mathf.Cos(lerpAngle*Mathf.Deg2Rad);
		status.facingDirection.y = Mathf.Sin(lerpAngle*Mathf.Deg2Rad);

		this.transform.rotation = Quaternion.Euler(0, lerpAngle, 0);
	}

	public virtual void attackToward(Vector2 dir) {

	}

	public virtual void attack(){}

	public virtual void startCharging() {
		status.moveSpeed = status.regularMoveSpeed/2f;
	}

	public virtual void chargedAttack() {
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

	public virtual void hit(Character other, CharacterSkillType skillType) {
		TempEnemy temp = (TempEnemy) other;
		temp.hits--;
		if(temp.hits <= 0) {
			AIController.current.characters.Remove(temp);
			Destroy(temp.gameObject);
		}

	}

	void Update() {
		for(int a=0;a!=10;++a) {
			if(actionCdRemain[a]>0) {
				actionCdRemain[a] -= Time.deltaTime;
			} 
			if(actionCdRemain[a] <= 0) actionCdRemain[a] = 0;
		}
	}
}
