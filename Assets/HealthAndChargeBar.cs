using UnityEngine;
using System.Collections;

public class HealthAndChargeBar : MonoBehaviour {

	public Character character;

	void Start () {
	
	}

	void Update () {
		transform.position = character.transform.position;
	}
}
