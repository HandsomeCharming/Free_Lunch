using UnityEngine;
using System.Collections;

public enum ProjectileType {
	Attack = 0,
	ChargedAttack
}

public class Projectile : MonoBehaviour {
	public Character shooter;
	public ProjectileType type;

	Vector2 direction;

	Projectile(Character shooter, Vector2 dir, ProjectileType type) {
		this.shooter = shooter;
		this.direction = dir;
		this.type = type;
	}

	public static Projectile ShootProjectile(Character shooter, Vector2 dir, ProjectileType type) {
		GameObject obj = (GameObject)Instantiate(Resources.Load("Prefabs/CubeBullet"), shooter.transform.position, Quaternion.identity);
		Projectile ans = obj.GetComponent<Projectile>();
		Destroy(obj, 2f);
		ans.shooter = shooter;
		ans.direction = dir;
		ans.type = type;
		return ans;
	}

	// Use this for initialization
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
	}
}
