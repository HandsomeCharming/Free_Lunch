using UnityEngine;
using System.Collections;

public class Boss1Divided : Enemy {
	float lowerRadius = 0.85f;
	float lowerAnglePlus =0.39f;
	float upperRadius = 0.69f;
	float upperAnglePlus = 0.395f;
	// Use this for initialization
	ArrayList lowerShields;
	ArrayList upperShields;

	Character player = null;

	int phase = 1;

	bool dividing = true;

	void Start () {
		type = 10;
		status.maxhp = 100;
		status.hp = 100;
		status.regularMoveSpeed = 200f;


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

	public void startDivide() {
		StartCoroutine(divide());
	}

	public IEnumerator divide() {
		yield return new WaitForEndOfFrame();
		Vector2 dir = status.facingDirection;
		state = CharacterState.UseSkill;
		float time = 0f;
		float rotSpeed = 16f;
		while(time <= 0.1f) {
			time += Time.deltaTime;
			Vector3 rot = transform.rotation.eulerAngles;
			rot.y += rotSpeed;
			transform.rotation = Quaternion.Euler(rot);
			moveToward(dir.normalized);
			yield return new WaitForEndOfFrame();
		}
		GetComponent<Collider>().isTrigger = false;
		while(time <= 0.5f) {
			time += Time.deltaTime;
			Vector3 rot = transform.rotation.eulerAngles;
			rot.y += rotSpeed;
			transform.rotation = Quaternion.Euler(rot);
			moveToward(dir.normalized);
			yield return new WaitForEndOfFrame();
		}
		stop();
		while(time <= 2f) {
			if(rotSpeed >= 0f)
				rotSpeed -= 0.2f;
			Vector3 rot = transform.rotation.eulerAngles;
			rot.y += rotSpeed;
			transform.rotation = Quaternion.Euler(rot);
			time += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		dividing = false;
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
		state = CharacterState.Stand;
		yield break;
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
		if(coll.rigidbody.tag == "Player") {
			Character player = coll.gameObject.GetComponent<Character>();
			player.gotHit(this, 8f);
		}
	}

	void OnCollisionStay(Collision coll) {
		if(coll.rigidbody.tag == "Player") {
			Character player = coll.gameObject.GetComponent<Character>();
			player.gotHit(this, 2f);
		}
	}

	// Update is called once per frame
	void Update () {
		base.Update();
		if(AIController.current != null && player == null) {
			player = AIController.current.player;
		}
		if(state != CharacterState.UseSkill && !dividing) {
			state = CharacterState.UseSkill;
			StartCoroutine(startRamming());
		}
	}
}
