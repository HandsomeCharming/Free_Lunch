using UnityEngine;
using System.Collections;

public class MainCamera : MonoBehaviour {

	public static MainCamera current;

	public Character character;

	public MainCamera() {
		current = this;
	}
	// Use this for initialization
	void Start () {
		current = this;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 pos = transform.position;
		pos.x = character.transform.position.x;
		pos.z = character.transform.position.z;
		transform.position = pos;
	}
}
