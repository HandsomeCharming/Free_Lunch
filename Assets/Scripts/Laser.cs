using UnityEngine;
using System.Collections;

public class Laser : MonoBehaviour {

	float hitTime = 0f; //Record the time that the trap can't hit player

	void Update() {
		if(hitTime > 0f) {
			hitTime -= Time.deltaTime;
			if(hitTime <= 0f)
				hitTime = 0f;
		}
	}

	void OnCollisionEnter(Collision col) {
		if(hitTime > 0f) return;
		hitTime = 0.5f;
		if(col.rigidbody.tag == "Player") {
			Character ch = col.gameObject.GetComponent<Character>();
			ch.gotHit(null, 30f);
		}
	}
}
