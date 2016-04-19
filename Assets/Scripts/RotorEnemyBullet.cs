using UnityEngine;
using System.Collections;

public class RotorEnemyBullet : MonoBehaviour {
	public RotorEnemy parent;
	MeshRenderer mrenderer;
	bool hit = false;
	public float alphaTime = 0.1f;
	public float shootTime = 0.5f;
	float damageTime = 0.1f;
	// Use this for initialization
	void Start () {
		mrenderer = GetComponent<MeshRenderer>();
		hit = false;
	}

	public void shoot() {
		StartCoroutine(startCharging());
	}

	IEnumerator startCharging() {
		float time = 0;
		mrenderer.enabled = true;
		float scale = 0.001f;
		while(time <= alphaTime) {
			time += Time.deltaTime;
			scale = Mathf.Lerp(0.001f, 0.006f, time/alphaTime);
			Vector3 sc = transform.localScale;
			sc.z = scale;
			transform.localScale = sc;
			mrenderer.material.color = Color.Lerp(new Color(1f,1f,1f,0.0f), Color.white, time/alphaTime);
			yield return new WaitForEndOfFrame();
		}
		GetComponent<CapsuleCollider>().enabled = true;
		yield return new WaitForSeconds(shootTime);
		GetComponent<CapsuleCollider>().enabled = false;
		time = 0;
		while(time <= alphaTime) {
			time += Time.deltaTime;
			mrenderer.material.color = Color.Lerp(Color.white, new Color(1f,1f,1f,0.0f), time/alphaTime);
			scale = Mathf.Lerp(0.006f, 0.001f, time/alphaTime);
			Vector3 sc = transform.localScale;
			sc.z = scale;
			transform.localScale = sc;
			yield return new WaitForEndOfFrame();
		}
		mrenderer.enabled = false;
		parent.status.turningSpeed = 3f;
		parent.state = CharacterState.Move;
		yield break;
	}

	void OnTriggerEnter(Collider coll) {
		if(coll.gameObject == null)return;
		if(coll.gameObject.tag == "TempEnemy" || coll.gameObject.tag == "Bullet" || coll.gameObject.tag == "Trap" || coll.gameObject.tag == "Wall")return;
		parent.hit(coll.gameObject.GetComponent<Character>(), CharacterSkillType.ChargedAttack, 0);
		damageTime = 0.1f;
	}

	void OnTriggerStay(Collider coll) {
		if(damageTime >= 0) {
			damageTime -= Time.deltaTime;
			return;
		}
		parent.hit(coll.gameObject.GetComponent<Character>(), CharacterSkillType.ChargedAttack, 0);
		damageTime = 0.1f;
	}

	// Update is called once per frame
	void Update () {
		transform.localPosition = new Vector3(0.2f, 0f, 0f);
	}
}
