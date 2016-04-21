using UnityEngine;
using System.Collections;

public class Boss1 : Enemy {
	float lowerRadius = 0.85f;
	float lowerAnglePlus =0.39f;
	float upperRadius = 0.69f;
	float upperAnglePlus = 0.395f;
	// Use this for initialization
	ArrayList lowerShields;
	ArrayList upperShields;

	Character player = null;

	int phase = 1;

	int ramCount = 0;
	bool isRamming = false;

	void Start () {
		type = 9;
		status.maxhp = 2000;
		status.hp = 2000;
		status.canBeCCed = false;
		actionCds[0] = 6f;		//Single Ram
		actionCds[1] = 20f;		//Divide
		actionCdRemain[1] = 5f;

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

	void OnCollisionEnter(Collision coll) {
		if(coll.rigidbody.tag == "Player") {
			Character player = coll.gameObject.GetComponent<Character>();
			player.gotHit(this, 10f);
		}
	}

	void OnCollisionStay(Collision coll) {
		if(coll.rigidbody.tag == "Player") {
			Character player = coll.gameObject.GetComponent<Character>();
			player.gotHit(this, 2f);
		}
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
		float rammingTime = 1f;
		Vector3 dir = player.transform.position - transform.position;
		time = 0;
		float currentSpeed = 150f;
		status.regularMoveSpeed = currentSpeed;
		while(time <= rammingTime) {
			if(currentSpeed <= 200f)
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
		state = CharacterState.Stand;
		ramCount--;
		if(ramCount > 0)
		yield break;
	}

	// Update is called once per frame
	void Update () {
		base.Update();
		if(AIController.current != null && player == null) {
			player = AIController.current.player;
		}
		if(phase == 1) {
			if(state != CharacterState.UseSkill) {
				if(actionCdRemain[1] == 0) {
					actionCdRemain[1] = actionCds[1];
					state = CharacterState.UseSkill;
					//StartCoroutine(startRamming());
					StartCoroutine(startDivision());
				} else if(actionCdRemain[0] == 0) {
					state = CharacterState.UseSkill;
					ramCount = Random.Range(1,3);
					actionCdRemain[0] = actionCds[0] + 2f*ramCount;
					StartCoroutine(startRamming());
				} 
			}

		}

	}
}
