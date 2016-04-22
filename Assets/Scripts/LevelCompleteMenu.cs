using UnityEngine;
using System.Collections;

public class LevelCompleteMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine(delayAnimation());
	}

	IEnumerator delayAnimation() {
		Animator[] ani = GetComponentsInChildren<Animator>();
		foreach(Animator a in ani) {
			a.enabled = false;
		}
		yield return new WaitForSeconds(1f);
		foreach(Animator a in ani) {
			a.enabled = true;
		}
		yield break;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
