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

	int type;
	int subType;
	float speed = 1;
	float existTime = 2;
	Vector2 direction;

	Projectile(Character shooter, Vector2 dir, CharacterSkillType type) {
		this.shooter = shooter;
		this.direction = dir;
		this.skillType = type;
	}

	public static void InitProjectileData() {
		projectileTypes = new Dictionary<int, ProjectileType>();

		projectileTypes.Add(3, new ProjectileType(3,1));
		projectileTypes[3].projectileDatas[0] = new ProjectileData(3, 0, CharacterSkillType.Attack, 1f, 2f, "Prefabs/CubeBullet");

		projectileTypes.Add(4, new ProjectileType(4,1));
		projectileTypes[4].projectileDatas[0] = new ProjectileData(4, 0, CharacterSkillType.Attack, 1f, 2f, "Prefabs/CubeBullet");

		projectileTypes.Add(5, new ProjectileType(5,1));
		projectileTypes[5].projectileDatas[0] = new ProjectileData(5, 0, CharacterSkillType.Attack, 1f, 2f, "Prefabs/CubeBullet");

		projectileTypes.Add(30, new ProjectileType(30,1));
		projectileTypes[30].projectileDatas[0] = new ProjectileData(30, 0, CharacterSkillType.Attack, 1f, 2f, "Prefabs/EnemyBullet");

		projectileTypes.Add(103, new ProjectileType(103, 2));
		projectileTypes[103].projectileDatas[0] = new ProjectileData(103, 0, CharacterSkillType.ChargedAttack, 1f, 2f, "Prefabs/CubeBullet");
		projectileTypes[103].projectileDatas[1] = new ProjectileData(103, 1, CharacterSkillType.ChargedAttack, 0f, 2f, "Prefabs/BurstExplosion");

		projectileTypes.Add(152, new ProjectileType(152, 2));
		projectileTypes[152].projectileDatas[0] = new ProjectileData(152, 0, CharacterSkillType.Dodge, 1f, 2f, "");
		projectileTypes[152].projectileDatas[1] = new ProjectileData(152, 1, CharacterSkillType.Dodge, 0f, 5f, "Prefabs/DodgeSlowCloud");

		projectileTypes.Add(200, new ProjectileType(200, 1));
		projectileTypes[200].projectileDatas[0] = new ProjectileData(200, 0, CharacterSkillType.Block, 0f, 1f, "Prefabs/Shield");

		projectileTypes.Add(251, new ProjectileType(251, 1));
		projectileTypes[251].projectileDatas[0] = new ProjectileData(251, 0, CharacterSkillType.Active1, 0f, 5f, "Prefabs/Ability251");
		//projectileData[0].setData
	}

	public static Projectile ShootProjectile(Character shooter, Vector2 dir, Vector3 pos, CharacterSkillType skillType, int subType = 0) {
		GameObject obj = objectByType(shooter, skillType, subType); //(GameObject)Instantiate(Resources.Load("Prefabs/CubeBullet"), pos, Quaternion.identity);
		obj.transform.position = pos;
		Projectile ans = obj.GetComponent<Projectile>();
		if(ans == null) ans = obj.GetComponentInChildren<Projectile>();
		InitByType(ref ans, shooter, skillType, subType);
		ans.shooter = shooter;
		ans.direction = dir;
		ans.skillType = skillType;

		obj.transform.rotation = rotationByType(shooter, dir, skillType, subType);

		Destroy(obj, ans.existTime);
		return ans;
	}

	public static Projectile ShootProjectile(Character shooter, Vector2 dir, CharacterSkillType skillType, int subType = 0) {
		Vector3 pos = shooter.transform.position;
		pos.x += dir.x * 3f;
		pos.z += dir.y * 3f;
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
		case 251:
			return Quaternion.Euler(90, 0, 0);
		default:
			return Quaternion.Euler(0, -angle, 0);
		}
			
	}

	static void InitByType(ref Projectile projectile, Character shooter, CharacterSkillType skillType, int subType = 0) {
		int type = shooter.getSkillType(skillType);
		ProjectileData data = projectileTypes[type].projectileDatas[subType];

		projectile.type = type;
		projectile.speed = data.speed;
		projectile.existTime = data.existTime;
		projectile.subType = subType;
	}

	void Start () {
		//destroyOnDelay(2.0f);
	}

	void Update () {
		Vector3 pos;
		if(type == 200) {
			pos = shooter.transform.position;
			pos.x += shooter.status.facingDirection.x * 3f;
			pos.z += shooter.status.facingDirection.y * 3f;
			this.transform.rotation = rotationByType(shooter, shooter.status.facingDirection, CharacterSkillType.Block, 0);
		} else {
			pos = this.transform.position;
			pos.x += direction.x*speed;
			pos.z += direction.y*speed;
		}
		this.transform.position = pos;

		if(shooter == null) Destroy(this.gameObject);
	}

	void OnTriggerEnter(Collider coll) {
		//Temp Enemy only
		if(coll.gameObject == null || shooter == null)return;
		if(coll.gameObject.tag == shooter.tag || coll.gameObject.tag == "Bullet")return;
		hit(coll.gameObject.GetComponent<Character>());
		
		if(skillType == CharacterSkillType.ChargedAttack) {
			if(subType == 0) {
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

	bool destroyOnContact() {
		if(type == 152 || type == 200 || type == 251)return false;
		return true;
	}

	void hit(Character character) {
		shooter.hit(character, skillType, subType);
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