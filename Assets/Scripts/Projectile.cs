using UnityEngine;
using System.Collections;

/*public enum ProjectileType {
	Attack = 0,
	ChargedAttack
}*/

public class Projectile : MonoBehaviour {
	public Character shooter;
	public CharacterSkillType type;

	Vector2 direction;

	Projectile(Character shooter, Vector2 dir, CharacterSkillType type) {
		this.shooter = shooter;
		this.direction = dir;
		this.type = type;
	}

	public static Projectile ShootProjectile(Character shooter, Vector2 dir, CharacterSkillType skillType) {
		Vector3 pos = shooter.transform.position;
		pos.x += dir.x * 3f;
		pos.z += dir.y * 3f;
		GameObject obj = objectByType(shooter.type, skillType); //(GameObject)Instantiate(Resources.Load("Prefabs/CubeBullet"), pos, Quaternion.identity);
		obj.transform.position = pos;
		Projectile ans = obj.GetComponent<Projectile>();
		Destroy(obj, 2f);
		ans.shooter = shooter;
		ans.direction = dir;
		ans.type = skillType;
		return ans;
	}

	static GameObject objectByType(int type, CharacterSkillType skillType) {
		switch (type) {
		case 0: {  //Main Character 
			return (GameObject)Instantiate(Resources.Load("Prefabs/CubeBullet"));
		}
		case 5: {
			return (GameObject)Instantiate(Resources.Load("Prefabs/EnemyBullet"));
		}
		default:
			break;
		}
		return null;
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
			                                         Quaternion.Euler(0, angle+90f, 0 ) );
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
