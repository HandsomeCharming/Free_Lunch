using UnityEngine;
using System.Collections;

public class LevelSelectUnit : MonoBehaviour {
	public GameObject chooseCircle;
	public float circleRadius = 0.31642f;
	public float circleSpeed = 1f;

	float rad = 0;
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
	}

	public void goToLevel(int level) {
		Application.LoadLevel("Level1");
	}
}
