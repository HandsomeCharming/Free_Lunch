using UnityEngine;
using System.Collections;

public class Boss1 : Enemy {
	float lowerRadius = 0.85f;
	float lowerAnglePlus =0.39f;
	float upperRadius = 0.69f;
	float upperAnglePlus = 0.395f;
	float cannonRadius = 0.7f;
	float cannonAnglePlus = 0.5f;
	// Use this for initialization
	ArrayList lowerShields;
	ArrayList upperShields;
	ArrayList cannons;

	Character player = null;

	public int phase = 1;

	int ramCount = 0;
	bool isRamming = false;
	bool[] cannonCanShoot = {true, true, true, true};
	bool died = false;

	public FinishPoint finishPoint;
	public Canvas hpBar;

	void Start () {
		type = 9;
		status.maxhp = 3000;
		status.hp = 3000;
		status.canBeCCed = false;
		actionCds[0] = 6f;		//Single Ram
		actionCds[1] = 20f;		//Divide
		actionCdRemain[1] = 5f;
		actionCds[2] = 20f;
		actionCds[3] = 3f;

		attackModifier = new AttackModifier(31);
		chargedAttackModifier = new ChargedAttackModifier(117);

		lowerShields = new ArrayList();
		for(int a=0;a!=8;++a) {
			float angle = a * 360f/8f*Mathf.Deg2Rad + lowerAnglePlus;
			Vector3 pos = new Vector3(Mathf.Sin(angle)*lowerRadius, -0.03f, Mathf.Cos(angle)*lowerRadius);
			Quaternion rot = Quaternion.Euler(new Vector3(270f, 45f*a, 0f));
			GameObject obj = (GameObject)Instantiate(Resources.Load("Prefabs/Boss1/lowerShield"), pos, rot);
			obj.transform.parent = this.gameObject.transform;
			obj.transform.localScale = new Vector3(90f, 90f, 90f);
			obj.transform.localPosition = pos;
			lowerShields.Add(obj);
		}
		upperShields = new ArrayList();
		for(int a=0;a!=8;++a) {
			float angle = a * 360f/8f*Mathf.Deg2Rad + upperAnglePlus;
			Vector3 pos = new Vector3(Mathf.Sin(angle)*upperRadius, 0.514f, Mathf.Cos(angle)*upperRadius);
			Quaternion rot = Quaternion.Euler(new Vector3(270f, 45f*a, 0f));
			GameObject obj = (GameObject)Instantiate(Resources.Load("Prefabs/Boss1/upperShield"), pos, rot);
			obj.transform.parent = this.gameObject.transform;
			obj.transform.localScale = new Vector3(90f, 90f, 90f);
			obj.transform.localPosition = pos;
			upperShields.Add(obj);
		}


	}

	void OnTriggerEnter(Collider coll) {
		if(coll.tag == "Player") {
			Character player = coll.gameObject.GetComponent<Character>();
			player.gotHit(this, 10f);
		}
	}

	void OnTriggerStay(Collider coll) {
		if(coll.tag == "Player") {
			Character player = coll.gameObject.GetComponent<Character>();
			player.gotHit(this, 2f);
		}
	}

	void OnCollisionEnter(Collision coll) {
		if(died)return;
		if(coll.rigidbody.tag == "Player") {
			Character player = coll.gameObject.GetComponent<Character>();
			player.gotHit(this, 8f);
		}
	}

	void OnCollisionStay(Collision coll) {
		if(died)return;
		if(coll.rigidbody.tag == "Player") {
			Character player = coll.gameObject.GetComponent<Character>();
			player.gotHit(this, 2f);
		}
	}

	public override void die() {
		if(died)return;
		died = true;
		//GetComponent<Collider>().enabled = false;
		stop();
		status.hp = 0;
		MainCamera.current.startBloomEffect();
		StartCoroutine(generateFinishPoint());
		Destroy(this.gameObject, 2f);
	}

	IEnumerator generateFinishPoint() {
		yield return new WaitForSeconds(1.4f);
		finishPoint.gameObject.SetActive(true);
		finishPoint.transform.position = transform.position;
		MeshRenderer[] rends = GetComponentsInChildren<MeshRenderer>();
		foreach(MeshRenderer rend in rends) {
			rend.enabled = false;
		}
		GetComponent<Collider>().enabled = false;
		hpBar.enabled = false;
		//Destroy(this);
		yield break;
	}

	IEnumerator startDivision() {
		float prepareTime = 2f;
		float time = 0;
		float rotSpeed = 0;
		while(time <= prepareTime) {
			rotSpeed += 0.2f;
			Vector3 rot = transform.rotation.eulerAngles;
			rot.y += rotSpeed;
			transform.rotation = Quaternion.Euler(rot);
			time += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		float degree = Random.value*60f;
		for(int a=0;a!=3;++a) {
			float rad = degree*Mathf.Deg2Rad;
			Vector3 pos = transform.position;
			Quaternion rota = Quaternion.Euler(new Vector3(0, 0, 0f));
			GameObject obj = (GameObject)Instantiate(Resources.Load("Prefabs/Boss1/boss1Divided"), pos, rota);
			Boss1Divided div = obj.GetComponent<Boss1Divided>();
			div.status.regularMoveSpeed = 200f;
			div.status.facingDirection = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
			div.moveToward(new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)));
			div.startDivide();
			degree += 120f;
			time = 0;
			while(time <= 0.3f) {
				Vector3 rot = transform.rotation.eulerAngles;
				rot.y += rotSpeed;
				transform.rotation = Quaternion.Euler(rot);
				time += Time.deltaTime;
				yield return new WaitForEndOfFrame();
			}
		}
		state = CharacterState.Stand;
		yield break;
	}

	IEnumerator startRamming() {
		float prepareTime = 2f;
		float time = 0;
		float rotSpeed = 0;
		while(time <= prepareTime) {
			rotSpeed += 0.2f;
			Vector3 rot = transform.rotation.eulerAngles;
			rot.y += rotSpeed;
			transform.rotation = Quaternion.Euler(rot);
			time += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		float rammingTime = 0.9f;
		Vector3 dir = player.transform.position - transform.position;
		time = 0;
		float currentSpeed = 150f;
		status.regularMoveSpeed = currentSpeed;
		while(time <= rammingTime) {
			if(currentSpeed <= 180f)
				currentSpeed += 2f;
			status.regularMoveSpeed = currentSpeed;
			Vector3 rot = transform.rotation.eulerAngles;
			rot.y += rotSpeed;
			transform.rotation = Quaternion.Euler(rot);
			time += Time.deltaTime;
			moveToward(new Vector2(dir.x, dir.z).normalized);
			yield return new WaitForEndOfFrame();
		}
		stop();
		time = 0;
		while(time <= prepareTime) {
			if(rotSpeed >= 0f)
				rotSpeed -= 0.2f;
			Vector3 rot = transform.rotation.eulerAngles;
			rot.y += rotSpeed;
			transform.rotation = Quaternion.Euler(rot);
			time += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		ramCount--;
		if(ramCount > 0) {
			StartCoroutine(startRamming());
		} else {
			state = CharacterState.Stand;
		}
		yield break;
	}

	IEnumerator enterPhase2() {
		float time = 0f;
		for(int a=0;a!=8;++a) {
			GameObject obj = (GameObject) lowerShields[a];
			Rigidbody body = obj.GetComponent<Rigidbody>();
			body.isKinematic = false;
			body.useGravity = true;
			body.AddExplosionForce(5000f, transform.position + Random.insideUnitSphere, 1000f);
			Destroy(obj, 4f);
		}

		cannons = new ArrayList();
		for(int a=0;a!=4;++a) {
			float angle = a * 360f/4f*Mathf.Deg2Rad + lowerAnglePlus;
			Vector3 pos = new Vector3(Mathf.Sin(angle)*lowerRadius, -0.03f, Mathf.Cos(angle)*lowerRadius);
			Quaternion rot = Quaternion.Euler(new Vector3(0, 205+90f*a, 0f));
			GameObject obj = (GameObject)Instantiate(Resources.Load("Prefabs/Boss1/Cannon2"), pos, rot);
			obj.transform.parent = this.gameObject.transform;
			obj.transform.localScale = new Vector3(135f, 135f, 70f);
			obj.transform.localRotation = rot;
			obj.transform.localPosition = pos;
			cannons.Add(obj);
		}

		actionCdRemain[2] = 10f;

		while(time < 0.5f) {
			float lowerR = Mathf.Lerp(lowerRadius, lowerRadius*3f, time/3f);
			float rotx = Mathf.Lerp(270f, 180f, time/2f);
			float cannonScale = Mathf.Lerp(20f, 60f, time/1f);
			float[] angles = new float[8];
			for(int a=0;a!=8;++a) {
				angles[a] = a * 360f/8f*Mathf.Deg2Rad + lowerAnglePlus + Random.value*0.3f;
			}
			for(int a=0;a!=8;++a) {
				GameObject obj = (GameObject) lowerShields[a];
				Rigidbody body = obj.GetComponent<Rigidbody>();
				body.AddForce(0,-10f,0f);
				Vector3 rota = new Vector3(rotx, 45f*a, 0f);
				Quaternion rot = Quaternion.Euler(rota);
				obj.transform.localRotation = rot;
			}
			for(int a=0;a!=4;++a) {
				GameObject obj = (GameObject) cannons[a];
				obj.transform.localScale = new Vector3(135f, 135f, cannonScale);
			}
			time += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		for(int a=0;a!=8;++a) {
			GameObject obj = (GameObject) lowerShields[a];
			obj.transform.parent = null;
		}
		state = CharacterState.Stand;
		while(time < 2f) {
			float lowerR = Mathf.Lerp(lowerRadius, lowerRadius*3f, time/3f);
			float rotx = Mathf.Lerp(270f, 180f, time/2f);
			float[] angles = new float[8];
			for(int a=0;a!=8;++a) {
				angles[a] = a * 360f/8f*Mathf.Deg2Rad + lowerAnglePlus + Random.value*0.3f;
			}
			for(int a=0;a!=8;++a) {
				GameObject obj = (GameObject) lowerShields[a];
				Rigidbody body = obj.GetComponent<Rigidbody>();
				body.AddForce(0,-10f,0f);
				Vector3 rota = new Vector3(rotx, 45f*a, 0f);
				Quaternion rot = Quaternion.Euler(rota);
				obj.transform.localRotation = rot;
			}
			time += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		lowerShields.Clear();
		yield break;
	}

	IEnumerator cannonScale(int a) {
		float time = 0;
		float scaleTime = 0.2f;
		GameObject obj = (GameObject)cannons[a];
		while(time < scaleTime) {
			float scale = Mathf.Lerp(70f, 40f, time/scaleTime);
			Vector3 sca = obj.transform.localScale;
			sca.z = scale;
			obj.transform.localScale = sca;
			time += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		time = 0;
		while(time < scaleTime) {
			float scale = Mathf.Lerp(40f,70f, time/scaleTime);
			Vector3 sca = obj.transform.localScale;
			sca.z = scale;
			obj.transform.localScale = sca;
			time += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		cannonCanShoot[a] = true;
		yield break;
	}

	IEnumerator rotateShoot() {
		float prepareTime = 0.5f;
		float time = 0;
		float rotSpeed = 0;
		while(time <= prepareTime) {
			rotSpeed = Mathf.Lerp(0f, 5f,time/prepareTime);
			Vector3 rot = transform.rotation.eulerAngles;
			rot.y += rotSpeed;
			transform.rotation = Quaternion.Euler(rot);
			time += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		float shootTime = 2f;
		Vector3 dir = player.transform.position - transform.position;
		float degree = Vector3.Angle(Vector3.right, dir);
		time = 0;
		while(time <= shootTime) {
			if(died)yield break;
			Vector3 rot = transform.rotation.eulerAngles;
			rot.y += rotSpeed;
			transform.rotation = Quaternion.Euler(rot);
			dir = (player.transform.position - transform.position).normalized;
			degree = Vector3.Angle(Vector3.right, dir);
			if(dir.z < 0) degree = 360f - degree;
			//print(degree);
			for(int a=0;a!=4;++a) {
				rot.y += 90*a;
				if(rot.y - degree < 3 && rot.y - degree > -3 && cannonCanShoot[a]) {
					//print(dir);
					Projectile.ShootProjectile(this, new Vector2(dir.x, dir.z), CharacterSkillType.Attack);
					cannonCanShoot[a] = false;
					StartCoroutine(cannonScale(a));
				}
			}
			time += Time.deltaTime;
			status.regularMoveSpeed = 30f;
			status.moveSpeed = 30f;
			moveToward(new Vector2(dir.x, dir.z).normalized);
			yield return new WaitForEndOfFrame();
		}
		time = 0;
		while(time <= prepareTime) {
			rotSpeed = Mathf.Lerp(5f,0.0f,time/prepareTime);
			Vector3 rot = transform.rotation.eulerAngles;
			rot.y += rotSpeed;
			transform.rotation = Quaternion.Euler(rot);
			time += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		state = CharacterState.Stand;
		yield break;
	}
		
	IEnumerator rotateUlt() {
		float prepareTime = 0.5f;
		float time = 0;
		float rotSpeed = 0;
		stop();
		transform.position = Vector3.zero;
		while(time <= prepareTime) {
			rotSpeed = Mathf.Lerp(0f, 11f,time/prepareTime);
			Vector3 rot = transform.rotation.eulerAngles;
			rot.y += rotSpeed;
			transform.rotation = Quaternion.Euler(rot);
			time += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		float shootTime = 5f;
		Vector3 dir = player.transform.position - transform.position;
		float degree = Vector3.Angle(Vector3.right, dir);
		time = 0;
		stop();
		while(time <= shootTime) {
			if(died)yield break;
			Vector3 rot = transform.rotation.eulerAngles;
			rot.y += rotSpeed;
			transform.rotation = Quaternion.Euler(rot);
			if(dir.z < 0) degree = 360f - degree;
			//print(degree);
			for(int a=0;a!=4;++a) {
				rot.y += 90*a;
					//print(dir);
				Projectile.ShootProjectile(this, new Vector2(Mathf.Cos(rot.y*Mathf.Deg2Rad), Mathf.Sin(rot.y*Mathf.Deg2Rad)), CharacterSkillType.ChargedAttack);
			}
			time += Time.deltaTime;
			//moveToward(new Vector2(dir.x, dir.z).normalized);
			yield return new WaitForEndOfFrame();
			time += Time.deltaTime;
			yield return new WaitForEndOfFrame();

		}
		time = 0;
		prepareTime = 2f;
		while(time <= prepareTime) {
			rotSpeed = Mathf.Lerp(11f,0.0f,time/prepareTime);
			Vector3 rot = transform.rotation.eulerAngles;
			rot.y += rotSpeed;
			transform.rotation = Quaternion.Euler(rot);
			time += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		state = CharacterState.Stand;
		yield break;
	}

	// Update is called once per frame
	void Update () {
		base.Update();
		if(AIController.current != null && player == null) {
			player = AIController.current.player;
		}
		if(state != CharacterState.UseSkill && status.hp < 0.4f * status.maxhp && phase == 1) {
			phase = 2;
			stop();
			StartCoroutine(enterPhase2());
			state = CharacterState.UseSkill;
		} else if (state != CharacterState.UseSkill && status.hp < 0.33f * status.maxhp && phase == 2) {
			//phase = 3;
		}
		if(phase == 1) {
			if(state != CharacterState.UseSkill) {
				if(actionCdRemain[1] == 0) {
					actionCdRemain[1] = actionCds[1];
					state = CharacterState.UseSkill;
					StartCoroutine(startDivision());
				} else if(actionCdRemain[0] == 0) {
					state = CharacterState.UseSkill;
					ramCount = Random.Range(1,3);
					actionCdRemain[0] = actionCds[0] + 2f*ramCount;
					StartCoroutine(startRamming());
				} else {
					Vector3 dir = player.transform.position - transform.position;
					status.regularMoveSpeed = 30f;
					moveToward(new Vector2(dir.x, dir.z).normalized);
				}
			}
		} else if(phase == 2) {
			if(state != CharacterState.UseSkill) {
				if(actionCdRemain[2] == 0) {
					actionCdRemain[2] = actionCds[2];
					state = CharacterState.UseSkill;
					StartCoroutine(rotateUlt());
				} else if(actionCdRemain[3] == 0) {
					actionCdRemain[3] = actionCds[3];
					state = CharacterState.UseSkill;
					StartCoroutine(rotateShoot());
				}
			}
		}

	}

	public override void hit(Character other, CharacterSkillType skillType, int subType = 0) {
		if(other == null) return;
		if(other.tag == "Player") {
			
		}
	}
}
