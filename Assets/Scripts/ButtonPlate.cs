using UnityEngine;
using System.Collections;

public class ButtonPlate : EventTrigger {
	public GameObject mark;
	public GameObject button;

	public float pressTime = 0.3f;
	public bool pressed = false;

	public bool canPressAgain;

	Vector3 oriPos;
	bool lerping = false;

	public GameObject destroy;
	// Use this for initialization
	void Start () {
		eventType = 0;
		oriPos = this.button.transform.position;
	}
	
	void OnTriggerEnter(Collider coll) {
		if(coll.gameObject.tag != "Player" ||  pressed || lerping)return;
		pressed = true;
		print("press");
		StartCoroutine(pushButton());
		if( destroy != null) {
			Destroy(destroy);
			destroy = null;
		}
	}

	void OnTriggerExit(Collider coll) {
		if(coll.gameObject.tag != "Player" ||  !pressed || !canPressAgain)return;

		StartCoroutine(releaseButton());
	}
		
	IEnumerator pushButton() {
		float time = 0;
		lerping = true;
		mark.GetComponent<SpriteRenderer>().color = Color.green;
		while(time < pressTime) {
			time += Time.deltaTime;
			Vector3 pos = Vector3.Lerp(oriPos, oriPos - new Vector3(0,0.4f,0), time/pressTime);
			button.transform.position = pos;
			yield return new WaitForEndOfFrame();
		}
		button.transform.position = oriPos - new Vector3(0,0.4f,0);
		lerping = false;
		//return true;
	}

	IEnumerator releaseButton() {
		float time = 0;
		while(lerping) {
			yield return new WaitForEndOfFrame();
		}
		lerping = true;
		mark.GetComponent<SpriteRenderer>().color = Color.white;
		while(time < pressTime) {
			time += Time.deltaTime;
			Vector3 pos = Vector3.Lerp(oriPos - new Vector3(0,0.4f,0), oriPos, time/pressTime);
			button.transform.position = pos;
			yield return new WaitForEndOfFrame();
		}
		button.transform.position = oriPos;
		if(canPressAgain)
			pressed = false;
		lerping = false;
		//return true;
	}
}
