using UnityEngine;
using System.Collections;

public class ButtonPlate : EventTrigger {
	public GameObject mark;
	public GameObject button;

	public float pressTime = 0.3f;
	public bool pressed = false;
	// Use this for initialization
	void Start () {
		eventType = 0;
	}
	
	void OnTriggerEnter(Collider coll) {
		if(coll.gameObject.tag != "Player" ||  pressed)return;
		pressed = true;
		print("press");
		StartCoroutine(pushButton());
	}

	void OnTriggerExit(Collider coll) {
		if(coll.gameObject.tag != "Player" ||  !pressed)return;
		pressed = false;
		StartCoroutine(releaseButton());
	}
		
	IEnumerator pushButton() {
		float time = 0;
		mark.GetComponent<SpriteRenderer>().color = Color.green;
		Vector3 oriPos = button.transform.position;
		while(time < pressTime) {
			time += Time.deltaTime;
			Vector3 pos = Vector3.Lerp(oriPos, oriPos - new Vector3(0,0.4f,0), time/pressTime);
			button.transform.position = pos;
			yield return new WaitForEndOfFrame();
		}
		//return true;
	}

	IEnumerator releaseButton() {
		float time = 0;
		mark.GetComponent<SpriteRenderer>().color = Color.white;
		Vector3 oriPos = button.transform.position;
		while(time < pressTime) {
			time += Time.deltaTime;
			Vector3 pos = Vector3.Lerp(oriPos, oriPos + new Vector3(0,0.4f,0), time/pressTime);
			button.transform.position = pos;
			yield return new WaitForEndOfFrame();
		}
		//return true;
	}
}
