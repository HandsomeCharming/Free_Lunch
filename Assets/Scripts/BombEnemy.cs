using UnityEngine;
using System.Collections;

public class BombEnemy : Character {

	float lerpTime = 2f;
	public bool startAcc = false;
	float chargingTime;

	void Start () {
		type = 7;
		chargedAttackModifier = new ChargedAttackModifier(116);
		chargingTime = chargedAttackModifier.chargeTime;

		if(AIController.current != null) {
			AIController.current.characters.Add(this);
		}

		status.regularMoveSpeed = 35f;
		status.turningSpeed = 3f;

		actionCds[0] = 1f;
		actionCdRemain[1] = actionCds[1];

		StartCoroutine(blink());
	}


	public override void startCharging() {
		if(actionCdRemain[1] > 0) return;
		actionCdRemain[1] = actionCds[1];

		startAcc = true;
	}

	public override void chargedAttack(float chargingTime) {
		Projectile.ShootProjectile(this, status.facingDirection, transform.position,  CharacterSkillType.ChargedAttack, 0);
		AIController.current.characters.Remove(this);
		//this.GetComponent<MeshRenderer>().enabled = false;
		//this.GetComponent<Collider>().enabled = false;
		Destroy(this.gameObject);
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

	IEnumerator blink() {
		MeshRenderer rend = GetComponent<MeshRenderer>();
		Material mat = rend.material;
		float time = 0f;
		Vector3 oriScale = transform.localScale;
		Vector3 tarScale = oriScale * 0.9f;
		Color oriColor = mat.color;
		Color tarColor = new Color(150f/255f,10f/255f,10f/255f);
		while(true) {
			if(this == null)break;
			while(time <= lerpTime/2f) {
				Vector3 sca = Vector3.Lerp(oriScale, tarScale, time/(lerpTime/2f));
				transform.localScale = sca;
				mat.color = Color.Lerp(oriColor, tarColor, time/(lerpTime/2f));
				time += Time.deltaTime;
				if(startAcc && lerpTime >= 0.5f) {
					lerpTime -= 0.01f;
				}
				chargingTime -= Time.deltaTime;
				if(chargingTime <= 0) {
					chargedAttack(0);
					yield break;
				}
				yield return new WaitForEndOfFrame();
			}
			while(time <= lerpTime) {
				Vector3 sca = Vector3.Lerp(tarScale, oriScale, (time-lerpTime/2f)/(lerpTime/2f));
				transform.localScale = sca;
				mat.color = Color.Lerp(tarColor, oriColor, (time-lerpTime/2f)/(lerpTime/2f));
				time += Time.deltaTime;
				if(startAcc && lerpTime >= 0.5f) {
					lerpTime -= 0.01f;
				}
				chargingTime -= Time.deltaTime;
				if(chargingTime <= 0) {
					chargedAttack(0);
					yield break;
				}
				yield return new WaitForEndOfFrame();
			}
			time = 0;
		}
		yield break;
	}
}
