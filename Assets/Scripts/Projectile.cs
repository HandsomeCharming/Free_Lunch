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
		Projectile ans = new Projectile(shooter, dir, type);
		return ans;
	}

	// Use this for initialization
	void Start () {
		destroyOnDelay(2.0f);
	}

	IEnumerator destroyOnDelay(float delay) {
		yield return new WaitForSeconds(2f);
		Destroy(this);
	}
	// Update is called once per frame
	void Update () {
	
	}
}
