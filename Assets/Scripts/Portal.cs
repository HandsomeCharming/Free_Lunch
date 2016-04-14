using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour {

	public Portal other;
	public Vector2 popoutDirection;

	bool canPort = true;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider coll) {
		if(coll.tag != "Player" || other == null || !canPort)return;
		other.canPort = false;
		coll.gameObject.transform.position = other.transform.position;
	}

	void OnTriggerExit(Collider coll) {
		if(coll.tag != "Player" || other == null || canPort)return;
		canPort = true;
	}
}
