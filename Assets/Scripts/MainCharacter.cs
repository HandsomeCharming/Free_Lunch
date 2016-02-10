using UnityEngine;
using System.Collections;

public class MainCharacter : Character {

	public MainCharacter() {
		actionCds = new float[10];
		actionCdRemain = new float[10];
		
		for(int a=0;a!=10;++a)
			actionCds[a] = 0.3f;
		for(int a=0;a!=10;++a)
			actionCdRemain[a] = 0;
	}

	// Use this for initialization
	void Start () {
		//Temporary collider setup
		SphereCollider collider = gameObject.AddComponent<SphereCollider>();
		collider.radius = 2.4f;
	}
	
	// Update is called once per frame
	void Update () {
		if(actionCdRemain[0]>0) {
			actionCdRemain[0] -= Time.deltaTime;
		} 
		if(actionCdRemain[0] <= 0) actionCdRemain[0] = 0;
	}
}
