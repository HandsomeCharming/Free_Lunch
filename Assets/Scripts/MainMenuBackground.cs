using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenuBackground : MonoBehaviour {
	public RectTransform[] back;
	public float[] backSpeed;
	public Vector2[] backMaxMin; //x is min, y is max

	bool start = false;

	bool[] backup = {false, true, false, true}; //true up, false down

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(back.Length != 0 && start == false) {
			//start = true;
			//StartCoroutine(backgroundLerp());
			for(int i=0; i!=4; ++i) {
				Vector2 a = back[i].offsetMax;
				if(backup[i]) {
					a.y -= backSpeed[i];
					if(a.y <= backMaxMin[i].x) {
						backup[i] = false;
					} 
				} else {
					a.y += backSpeed[i];
					if(a.y >= backMaxMin[i].y) {
						backup[i] = true;

					}
				}
				back[i].offsetMax = a;
			}
		}
	}

	IEnumerator backgroundLerp() {
		while(back[0] != null) {
			
			yield return new WaitForEndOfFrame();
		}

	}
}
