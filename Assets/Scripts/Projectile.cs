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
		projectileTypes[103].projectileDatas[0] = new ProjectileData(103, 0, CharacterSkillType.Attack, 1f, 2f, "Prefabs/CubeBullet");
		projectileTypes[103].projectileDatas[1] = new ProjectileData(103, 1, CharacterSkillType.Attack, 0f, 2f, "Prefabs/BurstExplosion");
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

		float angle = Vector2.Angle(new Vector2(1f, 0), dir); 
		angle = dir.y>0?angle:-angle;
		obj.transform.rotation = Quaternion.Euler(0, -angle, 0);

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

	IEnumerator destroyOnDelay(float delay) {
		yield return new WaitForSeconds(delay);

		Destroy(this.gameObject);
	}

	void Update () {
		Vector3 pos = this.transform.position;
		pos.x += direction.x*speed;
		pos.z += direction.y*speed;
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
		if(this.gameObject != null)
			Destroy(this.gameObject);
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