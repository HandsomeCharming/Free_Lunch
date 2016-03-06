using UnityEngine;
using System.Collections;

public class MainCamera : MonoBehaviour {

	public static MainCamera current;

	public float[] cameraBoundaries;

	public Character character;

	public MainCamera() {
		current = this;
		cameraBoundaries = new float[4] {-450, -400, 450, 400};//minx, miny, maxx,miny

	}
	// Use this for initialization
	void Start () {
		current = this;
		cameraBoundaries = new float[4] {-450, -400, 450, 400};
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 pos = transform.position;
		pos.x = Mathf.Clamp(character.transform.position.x, cameraBoundaries[0], cameraBoundaries[2]);
		pos.z = Mathf.Clamp(character.transform.position.z, cameraBoundaries[1], cameraBoundaries[3]);
		transform.position = pos;
	}
}
