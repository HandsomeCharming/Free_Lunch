using UnityEngine;
using UnityStandardAssets.ImageEffects;
using System.Collections;

public enum MainCharacterDiv {
	Devour,
	Division,
	Computation
}

public class MainCharacter : Character {
	public MainCharacterDiv div;

	public ParticleSystem cloud;
	public ParticleSystem lightning;

	public MainCharacter() : base() {
		actionCds[2] = 3f;
		actionCds[3] = 1.5f;
		actionCds[4] = 5f;
		actionCds[5] = 5f;

		status.regularMoveSpeed = 40f;
		if(!loadSkills()) {
			attackModifier = new AttackModifier(3);
			dodgeModifier = new DodgeModifier(150);
			chargedAttackModifier = new ChargedAttackModifier(103);
			blockModifier = new BlockModifier(200);
			activeSkills = new ArrayList();
			//activeSkills.Add(new ActiveModifier(251));
			//activeSkills.Add(new ActiveModifier(258));
		}

		div = MainCharacterDiv.Division;
	}

	public bool loadSkills() {
		if(SaveLoad.Load()) {
			SkillTree tree = GameSave.current.skilltree;
			Skill[] skills = tree.skills;
			if(skills[0]!=null)attackModifier = new AttackModifier(skills[0].skillType);
			else attackModifier = new AttackModifier(3);
			if(skills[1]!=null)chargedAttackModifier = new ChargedAttackModifier(skills[1].skillType);
			else chargedAttackModifier = new ChargedAttackModifier(103);
			if(skills[2]!=null)dodgeModifier = new DodgeModifier(skills[2].skillType);
			else dodgeModifier = new DodgeModifier(150);
			if(skills[3]!=null)blockModifier = new BlockModifier(skills[3].skillType);
			else blockModifier = new BlockModifier(200);
			activeSkills = new ArrayList();
			if(skills[4]!=null)activeSkills.Add(new ActiveModifier(skills[4].skillType));
			if(skills[6]!=null)activeSkills.Add(new ActiveModifier(skills[6].skillType));

			return true;
		}
		return false;
	}

	//Currently only division.
	public override void attack() {
		if(actionCdRemain[0] > 0) return;
		actionCdRemain[0] = actionCds[0];
		if(state == CharacterState.Block && blockModifier.type == 203) {
			actionCdRemain[0] = actionCds[0]*0.5f;
		}
		Projectile.ShootProjectile(this, status.facingDirection, CharacterSkillType.Attack);
	}

	public override void startCharging() {
		applyTemporaryEffect(555);//Apply speed reduce

	}

	public override void chargedAttack(float chargingTime) {
		if(actionCdRemain[1] > 0) return;
		actionCdRemain[1] = actionCds[1];
		if(state == CharacterState.Block && blockModifier.type == 203) {
			actionCdRemain[1] = actionCds[1]*0.5f;
			removeTemporaryEffect(555);//remove speed reduce
			if(chargingTime >= chargedAttackModifier.chargeTime*0.5f)
				Projectile.ShootProjectile(this, status.facingDirection, CharacterSkillType.ChargedAttack);
			else 
				Projectile.ShootProjectile(this, status.facingDirection, CharacterSkillType.Attack);
			return;
		}
		removeTemporaryEffect(555);//remove speed reduce
		if(chargingTime >= chargedAttackModifier.chargeTime)
			Projectile.ShootProjectile(this, status.facingDirection, CharacterSkillType.ChargedAttack);
		else 
			Projectile.ShootProjectile(this, status.facingDirection, CharacterSkillType.Attack);
	}

	public override void block() {
		if(actionCdRemain[3] > 0) return;
		actionCdRemain[3] = actionCds[3];
		if(blockModifier.type == 200) {
			Projectile.ShootProjectile(this, status.facingDirection, CharacterSkillType.Block);
		}
		else {
			GetComponent<Rigidbody>().isKinematic = true;
			Projectile.ShootProjectile(this, status.facingDirection, this.transform.position, CharacterSkillType.Block);
		}
	}

	public override void attackToward(Vector2 dir) {
		if(actionCdRemain[0] > 0) return;
		actionCdRemain[0] = actionCds[0];
		Projectile.ShootProjectile(this, dir, CharacterSkillType.Attack);
	}

	public override void dodgeToward(Vector2 dir) {
		if(actionCdRemain[2] > 0) return;
		actionCdRemain[2] = actionCds[2];
		faceTowardWithoutLerp(dir);
		StartCoroutine(dodgeLerp(dir));
	}

	public override void useSkill (int skillIndex)
	{
		if(activeSkills.Count < skillIndex)return;
		if(actionCdRemain[skillIndex+4] > 0)return;
		actionCdRemain[skillIndex+4] = actionCds[skillIndex+4];
		ActiveModifier modifier = (ActiveModifier) activeSkills[skillIndex];
		if(modifier.type == 251) {
			Projectile.ShootProjectile(this, status.facingDirection, CharacterSkillType.Active1, 0);
		}
		if(modifier.type == 258) {
			Projectile.ShootProjectile(this, status.facingDirection, CharacterSkillType.Active2, 0);
			applyTemporaryEffect(555);
		}
	}

	public override void hit(Character other, CharacterSkillType skillType, int subType = 0) {
		if(other == null) return;
		if(other.tag == "TempEnemy") {
			//TempEnemy temp = (TempEnemy) other;
			Character temp = (Character) other;
			switch(skillType) {
			case CharacterSkillType.Attack:
				temp.status.hp -= attackModifier.damage[subType];
				if(attackModifier.negativeEffectCount > 0) {
					for(int a=0;a!=attackModifier.negativeEffectCount;++a) {
						other.applyTemporaryEffect(attackModifier.negativeEffects[a]);
					}
				}
				break;
			case CharacterSkillType.ChargedAttack: 
				temp.status.hp -= chargedAttackModifier.damage[subType];
				break;
			case CharacterSkillType.Dodge:
				if(dodgeModifier.negativeEffectCount > 0) {
					for(int a=0;a!=dodgeModifier.negativeEffectCount;++a) {
						other.applyTemporaryEffect(dodgeModifier.negativeEffects[a]);
					}
				}
				break;
			case CharacterSkillType.Active1: {
				ActiveModifier mod = (ActiveModifier)activeSkills[0];
				temp.status.hp -= mod.damage[0];
				if(mod.negativeEffectCount > 0) {
					for(int a=0;a!=mod.negativeEffectCount;++a) {
						other.applyTemporaryEffect(mod.negativeEffects[a]);
					}
				}
				break;
				}
			case CharacterSkillType.Active2: {
					ActiveModifier mod = (ActiveModifier)activeSkills[1];
					temp.status.hp -= mod.damage[0];
					if(mod.negativeEffectCount > 0) {
						for(int a=0;a!=mod.negativeEffectCount;++a) {
							other.applyTemporaryEffect(mod.negativeEffects[a]);
						}
					}
					break;
				}
			}
		}
	}


	IEnumerator dodgeLerp(Vector2 dir) {
		float time = 0;
		Vector3 pos = this.transform.position;
		Vector3 scale = this.transform.localScale;

		this.gameObject.layer = 10;

		float scalez = scale.z;
		ParticleSystem.Particle []particleList = new ParticleSystem.Particle[cloud.particleCount];
		cloud.GetParticles(particleList);
		byte cloudAlpha = particleList[0].color.a;
		ParticleSystem.Particle []lightningList = new ParticleSystem.Particle[lightning.particleCount];
		lightning.GetParticles(lightningList);
		byte lightningAlpha = lightningList[0].color.a;

		float dodgeSlowTimeout = 0.05f;  //Only for slow path
		while(time < dodgeModifier.dodgeTime) {
			pos = new Vector3(dir.x, 0, dir.y).normalized * 1.5f;
			//this.transform.position = pos;
			Rigidbody body = GetComponent<Rigidbody>();
			body.velocity = pos * 65f;
			if(dodgeModifier.type == 152) {  //Slow path 
				dodgeSlowTimeout -= Time.deltaTime;
				if(dodgeSlowTimeout <= 0f) {
					dodgeSlowTimeout = 0.05f;
					Projectile.ShootProjectile(this, status.facingDirection, CharacterSkillType.Dodge, 1);
				}
			}
			if(time < dodgeModifier.dodgeTime/2f) {
				scale.z = Mathf.Lerp(scalez, scalez/5f, time/(dodgeModifier.dodgeTime/2f));
			} else {
				scale.z = Mathf.Lerp(scalez/5f, scalez, (time-dodgeModifier.dodgeTime/2f)/(dodgeModifier.dodgeTime/2f));
			}

			cloud.GetParticles(particleList);
			for(int i = 0; i < particleList.Length; ++i)
			{
				Color32 color = particleList[i].color;
				if(time < dodgeModifier.dodgeTime/2f && color.a > 10) {
					color.a -= 10;
				} else if(time > dodgeModifier.dodgeTime/2f && color.a < cloudAlpha){
					color.a += 10;
				}
				particleList[i].color = color;
			}        
			cloud.SetParticles(particleList, cloud.particleCount);

			lightning.GetParticles(lightningList);
			for(int i = 0; i < lightningList.Length; ++i)
			{
				Color32 color = lightningList[i].color;
				if(time < dodgeModifier.dodgeTime/2f && color.a > 10) {
					color.a -= 10;
				} else if(time > dodgeModifier.dodgeTime/2f && color.a < lightningAlpha){
					color.a += 10;
				}
				particleList[i].color = color;
			}        
			lightning.SetParticles(lightningList, lightning.particleCount);

			this.transform.localScale = scale;
			yield return new WaitForEndOfFrame();
			time += Time.deltaTime;
		}

		cloud.GetParticles(particleList);
		for(int i = 0; i < particleList.Length; ++i)
		{
			Color32 color = particleList[i].color;
			color.a = cloudAlpha;
			particleList[i].color = color;
		}        
		cloud.SetParticles(particleList, cloud.particleCount);

		lightning.GetParticles(lightningList);
		for(int i = 0; i < lightningList.Length; ++i)
		{
			Color32 color = lightningList[i].color;
			color.a = lightningAlpha;
			particleList[i].color = color;
		}        
		lightning.SetParticles(lightningList, lightning.particleCount);

		scale.z = scalez;
		this.transform.localScale = scale;

		this.gameObject.layer = 8;
	}

	// Use this for initialization
	void Start () {
		//Temporary collider setup
		SphereCollider collider = gameObject.AddComponent<SphereCollider>();
		collider.radius = 2.4f;

		GameObject hb = ((GameObject)Instantiate(Resources.Load("Prefabs/Healthbar")));
		HealthAndChargeBar healthBar = hb.GetComponent<HealthAndChargeBar>();
		healthBar.character = this;

		if(GameSave.current == null) {
			SaveLoad.Load();
		}
	}

	// Update is called once per frame
	void Update () {
		base.Update();
		if(GetComponent<Rigidbody>().isKinematic == true && state != CharacterState.Block) {
			GetComponent<Rigidbody>().isKinematic = false;
		}
	}

	void OnDestroy() {
		Instantiate(Resources.Load("Prefabs/MenuDead"));
	}
}
