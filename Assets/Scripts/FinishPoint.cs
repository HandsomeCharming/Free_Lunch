using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FinishPoint : MonoBehaviour {

	public Text text;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider col) {
		text.text = "You win!";
	}
}
