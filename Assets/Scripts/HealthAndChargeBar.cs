using UnityEngine;
using System.Collections;

public class HealthAndChargeBar : MonoBehaviour {

	public Character character;

	public GameObject chargeBar;

	void Start () {
		chargeBar = ((GameObject)Instantiate(Resources.Load("Prefabs/Chargebar")));
	}

	void Update () {
		if(character == null) {
			Destroy(this.gameObject);
			return;
		}
		transform.position = character.transform.position - new Vector3(3f,0,0);
		transform.localScale = new Vector3(3,4*(character.status.hp/character.status.maxhp),3);
		if(InputHandler.current.chargingTime > 0) {
			chargeBar.transform.position = character.transform.position + new Vector3(3f,0,0);
			chargeBar.transform.localScale =  new Vector3(3,4*(InputHandler.current.chargingTime/character.chargedAttackModifier.chargeTime),3);
		} else {
			chargeBar.transform.localScale =  new Vector3(3,0,3);
		}
	}
}
