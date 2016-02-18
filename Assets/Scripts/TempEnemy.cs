using UnityEngine;
using System.Collections;

public class TempEnemy : Character {
	public int hits;
	// Use this for initialization
	void Start () {
		if(AIController.current != null) {
			AIController.current.characters.Add(this);
		}
		hits = 3;
		status.moveSpeed = 0.2f;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
