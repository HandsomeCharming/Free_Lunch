using UnityEngine;
using System.Collections;

public class LevelCompleteMenu : MonoBehaviour {
	public GameObject completeText;
	public GameObject next;
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
		yield return new WaitForSeconds(1f);
		completeText.SetActive(true);
		next.SetActive(true);
		yield break;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
