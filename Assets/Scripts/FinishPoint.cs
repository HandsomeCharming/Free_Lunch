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
		//text.text = "You win!";
		if(col.tag == "Player") {
			InputHandler.current.stopInput();
			LevelCompleteMenu menu = ((GameObject)Instantiate(Resources.Load("Prefabs/LevelComplete"))).GetComponent<LevelCompleteMenu>();
		}

	}
}
