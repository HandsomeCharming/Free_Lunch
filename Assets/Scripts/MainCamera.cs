using UnityEngine;
using UnityStandardAssets.ImageEffects;
using System.Collections;

public class MainCamera : MonoBehaviour {

	public static MainCamera current;

	public float[] cameraBoundaries;

	public Character character;

	bool paused = false;

	public MainCamera() {
		current = this;

	}
	// Use this for initialization
	void Start () {
		current = this;
		cameraBoundaries = new float[4] {-319, -348, 310, 319}; //minx, miny, maxx, maxy
	}
	
	// Update is called once per frame
	void Update () {
		if(character == null)return;
		if(!paused && InputHandler.current.paused) {
			paused = true;
			GetComponent<Blur>().enabled = true;
		} else if(paused && !InputHandler.current.paused) { 
			paused = false;
			GetComponent<Blur>().enabled = false;
		}

		Vector3 pos = transform.position;
		pos.y = 0;
		Vector3 minus = character.transform.position - pos;
		if(minus.magnitude <=1.5f) {
			pos.x = Mathf.Clamp(character.transform.position.x, cameraBoundaries[0], cameraBoundaries[2]);
			pos.z = Mathf.Clamp(character.transform.position.z, cameraBoundaries[1], cameraBoundaries[3]);
		} else {
			pos.x = Mathf.Clamp(transform.position.x + minus.normalized.x*2f, cameraBoundaries[0], cameraBoundaries[2]);
			pos.z = Mathf.Clamp(transform.position.z + minus.normalized.z*2f, cameraBoundaries[1], cameraBoundaries[3]);
		}
		pos.y = transform.position.y;
		transform.position = pos;
	}
}
