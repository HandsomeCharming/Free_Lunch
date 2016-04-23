using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Feb 27 To-do: Add ProjectileType static array.


//Projectile is for animation and collider stuff
//Damage and negative effects are in hit() and modifier stuff.

public class ProjectileType {
	public ProjectileType(int type, int subTypeCount) {
		modifierType = type;
		modifierSubTypeCount = subTypeCount;

		projectileDatas = new ProjectileData[subTypeCount];
	}

	public ProjectileData[] projectileDatas;

	public int modifierType;
	public int modifierSubTypeCount; 
	public CharacterSkillType skillType;
}

public class ProjectileData {
	public ProjectileData(int modifierType, int modifierSubType, CharacterSkillType skillType, float speed, float existTime,
	                      string prefabName) {
		this.modifierType = modifierType;
		this.modifierSubType = modifierSubType;
		this.skillType = skillType;
		this.speed = speed;
		this.existTime = existTime;
		this.prefabName = prefabName;
	}

	public void setData(int modifierType, int modifierSubType, CharacterSkillType skillType, float speed, float existTime, string prefabName) {
		this.modifierType = modifierType;
		this.modifierSubType = modifierSubType;
		this.skillType = skillType;
		this.speed = speed;
		this.existTime = existTime;
		this.prefabName = prefabName;
	}
	public int modifierType;
	public int modifierSubType;
	public CharacterSkillType skillType;
	public float speed;
	public float existTime = 2f;
	public string prefabName;
}

public class Projectile : MonoBehaviour {
	public Character shooter;
	public CharacterSkillType skillType;

	public static Dictionary<int, ProjectileType> projectileTypes;

	public Modifier modifier;

	int type;
	int subType;
	float speed = 1;
	float existTime = 2;
	float timeToClearHitMax = 0.2f; //For dot, if 0, dont clear
	float timeToClearHit = 0.2f;
	Vector2 direction;

	ArrayList hitByThis;
	bool clearHit = false;

	Projectile(Character shooter, Vector2 dir, CharacterSkillType type) {
		this.shooter = shooter;
		this.direction = dir;
		this.skillType = type;
	}

	public static void InitProjectileData() {
		projectileTypes = new Dictionary<int, ProjectileType>();

		projectileTypes.Add(3, new ProjectileType(3,1));
		projectileTypes[3].projectileDatas[0] = new ProjectileData(3, 0, CharacterSkillType.Attack, 1.8f, 2f, "Prefabs/CubeBullet");

		projectileTypes.Add(4, new ProjectileType(4,1));
		projectileTypes[4].projectileDatas[0] = new ProjectileData(4, 0, CharacterSkillType.Attack, 1.8f, 2f, "Prefabs/CubeBullet");

		projectileTypes.Add(5, new ProjectileType(5,1));
		projectileTypes[5].projectileDatas[0] = new ProjectileData(5, 0, CharacterSkillType.Attack, 1.8f, 2f, "Prefabs/CubeBullet");

		projectileTypes.Add(30, new ProjectileType(30,1));
		projectileTypes[30].projectileDatas[0] = new ProjectileData(30, 0, CharacterSkillType.Attack, 1f, 2f, "Prefabs/EnemyBullet");

		projectileTypes.Add(31, new ProjectileType(31,1));
		projectileTypes[31].projectileDatas[0] = new ProjectileData(31, 0, CharacterSkillType.Attack, 2f, 4f, "Prefabs/Boss1/Boss1Bullet");

		projectileTypes.Add(103, new ProjectileType(103, 2));
		projectileTypes[103].projectileDatas[0] = new ProjectileData(103, 0, CharacterSkillType.ChargedAttack, 2.5f, 2f, "Prefabs/CubeBullet");
		projectileTypes[103].projectileDatas[1] = new ProjectileData(103, 1, CharacterSkillType.ChargedAttack, 0f, 2f, "Prefabs/BurstExplosion");

		projectileTypes.Add(116, new ProjectileType(116,1));
		projectileTypes[116].projectileDatas[0] = new ProjectileData(116, 0, CharacterSkillType.ChargedAttack, 0f, 1f, "Prefabs/TempBombExplosion");

		projectileTypes.Add(117, new ProjectileType(117,1));
		projectileTypes[117].projectileDatas[0] = new ProjectileData(117, 0, CharacterSkillType.ChargedAttack, 1f, 4f, "Prefabs/Boss1/Boss1UltBullet");

		projectileTypes.Add(152, new ProjectileType(152, 2));
		projectileTypes[152].projectileDatas[0] = new ProjectileData(152, 0, CharacterSkillType.Dodge, 1f, 2f, "");
		projectileTypes[152].projectileDatas[1] = new ProjectileData(152, 1, CharacterSkillType.Dodge, 0f, 5f, "Prefabs/DodgeSlowCloud");

		projectileTypes.Add(200, new ProjectileType(200, 1));
		projectileTypes[200].projectileDatas[0] = new ProjectileData(200, 0, CharacterSkillType.Block, 0f, 1f, "Prefabs/Shield");

		projectileTypes.Add(203, new ProjectileType(203, 1));
		projectileTypes[203].projectileDatas[0] = new ProjectileData(203, 0, CharacterSkillType.Block, 0f, 100f, "Prefabs/Deploy");

		projectileTypes.Add(204, new ProjectileType(204, 1));
		projectileTypes[204].projectileDatas[0] = new ProjectileData(204, 0, CharacterSkillType.Block, 0f, 100f, "Prefabs/ReadOnly");

		projectileTypes.Add(251, new ProjectileType(251, 1));
		projectileTypes[251].projectileDatas[0] = new ProjectileData(251, 0, CharacterSkillType.Active1, 0f, 5f, "Prefabs/Ability251");

		projectileTypes.Add(258, new ProjectileType(258, 1));
		projectileTypes[258].projectileDatas[0] = new ProjectileData(258, 0, CharacterSkillType.Active2, 0f, 2f, "Prefabs/Overflow");
		//projectileData[0].setData
	}

	public void init(Character shooter, Vector2 dir, Vector3 pos, CharacterSkillType skillType, int subType = 0) {
		transform.position = pos;
		initByType(shooter, skillType, subType);

		hitByThis = new ArrayList();
		shooter = shooter;
		direction = dir;
		skillType = skillType;

		transform.rotation = rotationByType(shooter, dir, skillType, subType);

		if(type == 204) {
			Physics.IgnoreCollision(GetComponent<Collider>(), shooter.GetComponent<Collider>());
		}

		Destroy(this.gameObject, existTime);
	}

	public static Projectile ShootProjectile(Character shooter, Vector2 dir, Vector3 pos, CharacterSkillType skillType, int subType = 0) {
		GameObject obj = objectByType(shooter, skillType, subType); //(GameObject)Instantiate(Resources.Load("Prefabs/CubeBullet"), pos, Quaternion.identity);
		obj.transform.position = pos;
		Projectile ans = obj.GetComponent<Projectile>();
		if(ans == null) ans = obj.GetComponentInChildren<Projectile>();
		InitByType(ref ans, shooter, skillType, subType);

		ans.hitByThis = new ArrayList();
		ans.shooter = shooter;
		ans.direction = dir;
		ans.skillType = skillType;

		switch(skillType) {
		case CharacterSkillType.Attack:
			ans.modifier = shooter.attackModifier;
			break;
		case CharacterSkillType.ChargedAttack:
			ans.modifier = shooter.chargedAttackModifier;
			break;
		case CharacterSkillType.Block:
			ans.modifier = shooter.blockModifier;
			break;
		case CharacterSkillType.Dodge:
			ans.modifier = shooter.dodgeModifier;
			break;
		case CharacterSkillType.Active1:
			ans.modifier = (ActiveModifier)shooter.activeSkills[0];
			break;
		case CharacterSkillType.Active2:
			ans.modifier = (ActiveModifier)shooter.activeSkills[1];
			break;
		}

		obj.transform.rotation = rotationByType(shooter, dir, skillType, subType);

		if(ans.type == 204) {
			Physics.IgnoreCollision(ans.GetComponent<Collider>(), shooter.GetComponent<Collider>());
		}

		Destroy(obj, ans.existTime);
		return ans;
	}

	public static Projectile ShootProjectile(Character shooter, Vector2 dir, CharacterSkillType skillType, int subType = 0) {
		Vector3 pos = shooter.transform.position;
		if(shooter.getSkillType(skillType) != 152) {
			pos.x += dir.x * 3f;
			pos.z += dir.y * 3f;
		} else {
			pos.x -= dir.x * 3f;
			pos.z -= dir.y * 3f;
		}
		return ShootProjectile(shooter, dir, pos, skillType, subType);
	}



	static GameObject objectByType(Character shooter, CharacterSkillType skillType, int subType = 0) {
		Modifier modifier;

		int type = shooter.getSkillType(skillType);
		GameObject obj = null;
		Projectile pro = null;

		obj = (GameObject)Instantiate(Resources.Load(projectileTypes[type].projectileDatas[subType].prefabName));
		return obj;
	}

	static Quaternion rotationByType(Character shooter, Vector2 dir, CharacterSkillType skillType, int subType = 0) {
		int type = shooter.getSkillType(skillType);

		float angle = Vector2.Angle(new Vector2(1f, 0), dir); 
		angle = dir.y>0?angle:-angle;

		switch(type) {
		case 152:
			return Quaternion.Euler(270, 0, 0);
		case 200:
			return Quaternion.Euler(320, 90-angle, 90);
		case 204:
			return Quaternion.Euler(0,0,0);
		case 251:
			return Quaternion.Euler(90, 0, 0);
		case 258:
			return Quaternion.Euler(0, 90-angle, 0);
		default:
			return Quaternion.Euler(0, -angle, 0);
		}
			
	}

	void initByType(Character shooter, CharacterSkillType skillType, int subType = 0) {
		int type = shooter.getSkillType(skillType);
		ProjectileData data = projectileTypes[type].projectileDatas[subType];

		type = type;
		speed = data.speed;
		existTime = data.existTime;
		subType = subType;
	}

	static void InitByType(ref Projectile projectile, Character shooter, CharacterSkillType skillType, int subType = 0) {
		int type = shooter.getSkillType(skillType);
		ProjectileData data = projectileTypes[type].projectileDatas[subType];

		projectile.type = type;
		projectile.speed = data.speed;
		projectile.existTime = data.existTime;
		projectile.subType = subType;

		if(type == 251 || type == 258) {
			projectile.clearHit = true;
		}
	}

	void Start () {
		//destroyOnDelay(2.0f);
	}



	void Update () {
		if(Time.timeScale == 0)return;
		Vector3 pos;

		if(destroyIfShooterDestroyed() && shooter == null) {
			Destroy(this.gameObject);
			return;
		}

		if(type == 200) {
			pos = shooter.transform.position;
			pos.x += shooter.status.facingDirection.x * 3f;
			pos.z += shooter.status.facingDirection.y * 3f;
			this.transform.rotation = rotationByType(shooter, shooter.status.facingDirection, CharacterSkillType.Block, 0);
			if(InputHandler.current.blockTime == 0) {
				Destroy(this.gameObject);
			}
			
		} else if(type == 204 || type == 203) {
			pos = this.transform.position;
			if(InputHandler.current.blockTime == 0) {
				if(this.transform.parent != null) {
					Destroy(this.transform.parent.gameObject);
					Destroy(this.gameObject);
				}
				else 
					Destroy(this.gameObject);
			}
		} else if(type == 258) {
			pos = shooter.transform.position;
			this.transform.rotation = rotationByType(shooter, shooter.status.facingDirection, CharacterSkillType.Active2, 0);
		}
		else {
			pos = this.transform.position;
			pos.x += direction.x*speed;
			pos.z += direction.y*speed;
		}
		this.transform.position = pos;

		if(clearHit && hitByThis != null && hitByThis.Count > 0) {
			if(timeToClearHit <= 0) {
				timeToClearHit = timeToClearHitMax;
				while(hitByThis.Count > 0) {
					hitByThis.RemoveAt(0);
				}
			} else {
				timeToClearHit -= Time.deltaTime;
			}
		}
	}

	void OnTriggerEnter(Collider coll) {
		//Temp Enemy only
		if(coll.gameObject == null || shooter == null)return;
		if(coll.gameObject.tag == shooter.tag || coll.gameObject.tag == "Bullet" || coll.gameObject.tag == "Trap")return;
		if(coll.gameObject.tag == "Wall" && destroyOnContact()) {
			Destroy(this.gameObject);
			return;
		}
		hit(coll.gameObject.GetComponent<Character>());
		
		if(skillType == CharacterSkillType.ChargedAttack) {
			if(type == 103 && subType == 0) {
				float angle = Vector2.Angle(new Vector2(1f,0),direction);
				angle = direction.y<0?angle:-angle;
				Projectile pro = ShootProjectile(shooter, direction, transform.position, skillType, 1);
			}
			if(subType == 1) {

			}
		}
		if(this.gameObject != null && destroyOnContact())
			Destroy(this.gameObject);
	}

	void OnTriggerStay(Collider coll) {
		if(coll.gameObject == null || shooter == null)return;
		if(coll.gameObject.tag == shooter.tag || coll.gameObject.tag == "Bullet" || coll.gameObject.tag == "Trap")return;
		if(coll.gameObject.tag == "Wall" && destroyOnContact()) {
			Destroy(this.gameObject);
			return;
		}
		hit(coll.gameObject.GetComponent<Character>());
	}

	bool destroyIfShooterDestroyed() {
		if(type == 200 || type == 258 || type == 117) return true;
		return false;
	}

	bool destroyOnContact() {
		if(type == 116 || type == 152 || type == 200 || type == 204 || type == 251 || type == 258)return false;
		return true;
	}

	void hit(Character character) {
		if(hitByThis.Contains(character))return;
		hitByThis.Add(character);
		//shooter.hit(character, skillType, subType);
		if(shooter == null || shooter.type != 0) {
			character.gotHit(null, modifier, subType);
		}
		else {
			shooter.hit(character, skillType, subType);
		}
	}
}


/*void OnCollisionEnter(Collision coll) {
		//Temp Enemy only
		if(coll.gameObject.tag == shooter.tag)return;
		hit(coll.gameObject.GetComponent<Character>());

		if(type == CharacterSkillType.ChargedAttack) {
			GameObject obj = (GameObject)Instantiate(Resources.Load("Prefabs/CubeExplosion"), this.transform.position, Quaternion.identity);
			obj.GetComponent<CubeExplosion>().dir = direction;
		}
		Destroy(this.gameObject);
	}*/