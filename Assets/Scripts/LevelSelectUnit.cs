using UnityEngine;
using System.Collections;

public class LevelSelectUnit : MonoBehaviour {
	public GameObject chooseCircle;
	public float circleRadius = 0.31642f;
	public float circleSpeed = 1f;

	public LevelSelectCircle[] circles;

	float rad = 90f*Mathf.Deg2Rad;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(chooseCircle == null) return;
		Vector3 pos = chooseCircle.transform.localPosition;
		pos.y = Mathf.Sin(rad)*circleRadius;
		pos.x = Mathf.Cos(rad)*circleRadius;
		pos.z = 0.01f;
		chooseCircle.transform.localPosition = pos;
		rad += circleSpeed*Mathf.Deg2Rad;
		if(rad >= 2*Mathf.PI) {
			rad = 0;
		}
		float deg = rad*Mathf.Rad2Deg;
		if(deg>=88f-2f&& deg <=92f && circles[0].playerMatch) {
			goToLevel(circles[0].level);
		} else if(deg>=208f&& deg <=212f && circles[1].playerMatch) {
			goToLevel(circles[1].level);
		} else if(deg>=328f&& deg <=332f && circles[2].playerMatch){
			goToLevel(circles[2].level);
		}

	}

	public void goToLevel(int level) {
		Application.LoadLevel("Level1");
	}
}
