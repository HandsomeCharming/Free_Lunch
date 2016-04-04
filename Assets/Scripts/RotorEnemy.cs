using UnityEngine;
using System.Collections;

public class RotorEnemy : Character {

	public GameObject rotor;
	public GameObject chargeBall;
	public RotorEnemyBullet bullet;

	public RotorEnemy() : base() {
		type = 6;
		chargedAttackModifier = new ChargedAttackModifier(115);

		actionCds[1] = 5f;
	}
	// Use this for initialization
	void Start () {
		if(AIController.current != null) {
			AIController.current.characters.Add(this);
		}
		//hits = 3;
		status.moveSpeed = 0.2f;
		status.turningSpeed = 3f;
		actionCds[1] = 5f;
		type = 6;

	}

	public override void startCharging() {
		if(actionCdRemain[1] > 0) return;
		actionCdRemain[1] = actionCds[1];

		StartCoroutine(startRotor());
	}

	IEnumerator startRotor() {
		float acc = 0f;
		float time = 0f;

		status.turningSpeed = 0f;
		chargeBall.SetActive(true);
		chargeBall.transform.localScale = new Vector3(0.001f, 0.001f, 0.001f);
		Vector3 rotate = rotor.transform.localRotation.eulerAngles;
		float ballScale = 0.01f;
		while(time <= chargedAttackModifier.chargeTime) {
			if(acc <= 20f)acc+=1f;
			time += Time.deltaTime;
			rotate = rotor.transform.localRotation.eulerAngles;
			rotate.y += acc;
			rotor.transform.localRotation = Quaternion.Euler(rotate);
			ballScale = Mathf.Lerp(0.001f, 0.01f, time/chargedAttackModifier.chargeTime);
			chargeBall.transform.localScale = new Vector3(ballScale, ballScale, ballScale);
			yield return new WaitForEndOfFrame();
		}
		chargedAttack(chargedAttackModifier.chargeTime);
		while(acc >= 10f) {
			acc -= 1f;
			rotate = rotor.transform.localRotation.eulerAngles;
			rotate.y += acc;
			rotor.transform.localRotation  = Quaternion.Euler(rotate);
			ballScale -= 0.001f;
			chargeBall.transform.localScale = new Vector3(ballScale, ballScale, ballScale);
			yield return new WaitForEndOfFrame();
		}
		chargeBall.SetActive(false);
		while(rotate.y > 100f || rotate.y < 80f) {
			rotate = rotor.transform.localRotation .eulerAngles;
			rotate.y += acc;
			rotor.transform.localRotation  = Quaternion.Euler(rotate);
			yield return new WaitForEndOfFrame();
		}
		rotate.x = 0f;
		rotate.y = 90f;
		rotate.z = 90f;
		rotor.transform.localRotation = Quaternion.Euler(rotate);
		yield return new WaitForEndOfFrame();
		yield break;
	}

	public override void chargedAttack(float chargingTime) {
		//Projectile.ShootProjectile(this, status.facingDirection, CharacterSkillType.ChargedAttack);
		bullet.shoot();
	}

	void Update() {
		base.Update();
	}

	public override void hit (Character other, CharacterSkillType skillType, int subType = 0)
	{
		if(this == null || other == null) return;
		base.hit (other, skillType, subType);
		other.gotHit(this, 5f);
	}
}
