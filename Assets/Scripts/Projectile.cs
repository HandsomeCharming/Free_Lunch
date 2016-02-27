using UnityEngine;
using System.Collections;


//Feb 27 To-do: Add ProjectileType static array.

public class ProjectileData {
	public ProjectileData(int modifierType, int modifierSubType, CharacterSkillType skillType, float speed,
	                      Vector3 direction, string prefabName) {
		this.modifierType = modifierType;
		this.modifierSubType = modifierSubType;
		this.skillType = skillType;
		this.speed = speed;
		this.existTime = existTime;
		this.direction = direction;
		this.prefabName = prefabName;
	}

	public void setData(int modifierType, int modifierSubType, CharacterSkillType skillType, float speed,
	               Vector3 direction, string prefabName) {
		this.modifierType = modifierType;
		this.modifierSubType = modifierSubType;
		this.skillType = skillType;
		this.speed = speed;
		this.existTime = existTime;
		this.direction = direction;
		this.prefabName = prefabName;
	}

	public int modifierType;
	public int modifierSubType;
	public CharacterSkillType skillType;
	public float speed;
	public float existTime;
	public Vector3 direction;
	public string prefabName;
}

public class Projectile : MonoBehaviour {
	public Character shooter;
	public CharacterSkillType type;

	public static ProjectileData[] projectileData;
	
	float speed = 1;
	float existTime = 2;
	Vector2 direction;

	Projectile(Character shooter, Vector2 dir, CharacterSkillType type) {
		this.shooter = shooter;
		this.direction = dir;
		this.type = type;
	}

	public static void InitProjectileData() {
		projectileData = new ProjectileData[100];

		//projectileData[0].setData
	}


	public static Projectile ShootProjectile(Character shooter, Vector2 dir, CharacterSkillType skillType, int subType = 0) {
		Vector3 pos = shooter.transform.position;
		pos.x += dir.x * 3f;
		pos.z += dir.y * 3f;
		GameObject obj = objectByType(shooter, skillType); //(GameObject)Instantiate(Resources.Load("Prefabs/CubeBullet"), pos, Quaternion.identity);
		obj.transform.position = pos;
		Projectile ans = obj.GetComponent<Projectile>();
		InitByType(ref ans, shooter, skillType, subType);
		Destroy(obj, 2f);
		ans.shooter = shooter;
		ans.direction = dir;
		ans.type = skillType;
		return ans;
	}

	static GameObject objectByType(Character shooter, CharacterSkillType skillType, int subType = 0) {
		int type = shooter.type;
		GameObject obj = null;
		Projectile pro = null;
		switch (type) {
		case 0: {  //Main Character 
			obj = (GameObject)Instantiate(Resources.Load("Prefabs/CubeBullet"));
			pro = obj.GetComponent<Projectile>();

			break;
		}
		case 5: {
			return (GameObject)Instantiate(Resources.Load("Prefabs/EnemyBullet"));
		}
		default:
			break;
		}
		return obj;
	}

	static void InitByType(ref Projectile projectile, Character shooter, CharacterSkillType skillType, int subType = 0) {
		switch(skillType) {
		case CharacterSkillType.Attack:
			//speed = 1f;
			break;
		case CharacterSkillType.ChargedAttack:
			//speed = 1f;
			break;
		default:
			break;
		}
	}

	void Start () {
		//destroyOnDelay(2.0f);
	}

	IEnumerator destroyOnDelay(float delay) {
		yield return new WaitForSeconds(delay);

		Destroy(this.gameObject);
	}
	// Update is called once per frame
	void Update () {
		Vector3 pos = this.transform.position;
		pos.x += direction.x;
		pos.z += direction.y;
		this.transform.position = pos;

		if(shooter == null) Destroy(this.gameObject);
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

	void OnTriggerEnter(Collider coll) {
		//Temp Enemy only
		if(coll.gameObject == null || shooter == null)return;
		if(coll.gameObject.tag == shooter.tag || coll.gameObject.tag == "Bullet")return;
		hit(coll.gameObject.GetComponent<Character>());
		
		if(type == CharacterSkillType.ChargedAttack) {
			float angle = Vector2.Angle(new Vector2(1f,0),direction);
			angle = direction.y<0?angle:-angle;
			GameObject obj = (GameObject)Instantiate(Resources.Load("Prefabs/Explosion1"), this.transform.position, 
			                                         Quaternion.Euler(90f, angle+30f, 0 ) );
			Destroy(obj, 0.8f);
			//obj.GetComponent<CubeExplosion>().dir = direction;
		}
		if(this.gameObject != null)
			Destroy(this.gameObject);
	}

	void hit(Character character) {
		shooter.hit(character, type);
	}
}
