using UnityEngine;
using UnityStandardAssets.ImageEffects;
using System.Collections;

public class MainCamera : MonoBehaviour {

	public static MainCamera current;

	public float[] cameraBoundaries;

	public Character character;
	public Character otherFocus = null;

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
		Vector3 minus;
		if(otherFocus == null || !otherFocus.isActiveAndEnabled) {
			minus = character.transform.position - pos;
			if(minus.magnitude <=1.5f) {
				pos.x = Mathf.Clamp(character.transform.position.x, cameraBoundaries[0], cameraBoundaries[2]);
				pos.z = Mathf.Clamp(character.transform.position.z, cameraBoundaries[1], cameraBoundaries[3]);
			} else {
				pos.x = Mathf.Clamp(transform.position.x + minus.normalized.x*2f, cameraBoundaries[0], cameraBoundaries[2]);
				pos.z = Mathf.Clamp(transform.position.z + minus.normalized.z*2f, cameraBoundaries[1], cameraBoundaries[3]);
			}
		} else {
			minus = (otherFocus.transform.position+character.transform.position)/2f -pos;
			Vector3 midp = (otherFocus.transform.position+character.transform.position)/2f;
			if(minus.magnitude <=1.5f) {
				pos.x = Mathf.Clamp(midp.x, cameraBoundaries[0], cameraBoundaries[2]);
				pos.z = Mathf.Clamp(midp.z, cameraBoundaries[1], cameraBoundaries[3]);
			} else {
				pos.x = Mathf.Clamp(transform.position.x + minus.normalized.x*2f, cameraBoundaries[0], cameraBoundaries[2]);
				pos.z = Mathf.Clamp(transform.position.z + minus.normalized.z*2f, cameraBoundaries[1], cameraBoundaries[3]);
			}
		}

		pos.y = transform.position.y;
		transform.position = pos;
	}

	public void startBloomEffect() {
		StartCoroutine(bloomEffect());
	}

	IEnumerator bloomEffect() {
		Bloom bloom = this.GetComponent<Bloom>();
		bloom.enabled = true;
		float time = 0f, bloomTime = 1f;
		while(time <= bloomTime) {
			bloom.bloomIntensity += 0.4f;
			time += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		time = 0;
		while(time <= bloomTime/2f) {
			bloom.bloomIntensity -= 0.8f;
			time += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		bloom.enabled = false;
		yield break;
	}
}
