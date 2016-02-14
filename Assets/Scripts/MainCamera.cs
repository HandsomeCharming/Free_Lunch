using UnityEngine;
using System.Collections;

public class MainCamera : MonoBehaviour {
	public Character character;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 pos = transform.position;
		pos.x = character.transform.position.x;
		pos.z = character.transform.position.z;
		transform.position = pos;
	}
}
