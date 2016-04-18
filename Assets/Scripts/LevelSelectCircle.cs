using UnityEngine;
using System.Collections;

public class LevelSelectCircle : MonoBehaviour {
	public int level;
	public LevelSelectUnit parent;

	public bool circleMatch = false;
	public bool playerMatch = false;
	// Use this for initialization
	void Start () {
		parent = transform.parent.gameObject.GetComponent<LevelSelectUnit>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider coll) {
		if(coll.tag == "Player") {
			playerMatch = true;
		}
		if(coll.tag ==  "Wall") {
			circleMatch = true;
		}
		if(playerMatch && circleMatch) {
			parent.goToLevel(level);
		}
	}

	void OnTriggerExit(Collider coll) {
		if(coll.tag == "Player") {
			playerMatch = false;
		} else if(coll.tag ==  "Wall") {
			circleMatch = false;
		}
	}
}
