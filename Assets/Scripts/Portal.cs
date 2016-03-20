using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour {

	public Portal other;
	public Vector2 popoutDirection;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider coll) {
		if(other == null)return;
		coll.gameObject.transform.position = other.transform.position;
	}
}
