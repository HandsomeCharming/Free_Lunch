using UnityEngine;
using System.Collections;

public class HealthAndChargeBar : MonoBehaviour {

	public Character character;

	public Texture2D chargeBar;

	public int cursorSize = 63;
	int sizeX;
	int sizeY;

	void Awake() {
		sizeX = cursorSize;
		sizeY = cursorSize;
	}

	void Start () {
	}

	void Update () {
		if(character == null) {
			Destroy(this.gameObject);
			return;
		}
		transform.position = character.transform.position - new Vector3(3f,0,0);
		transform.localScale = new Vector3(3,4*(character.status.hp/character.status.maxhp),3);
		if(InputHandler.current.chargingTime > 0) {
			
		}
	}

	void OnGUI()
	{
		if(InputHandler.current.chargingTime > 0) {
			
			float lerp = (InputHandler.current.chargingTime)/character.chargedAttackModifier.chargeTime;
			float scale = Mathf.Lerp(cursorSize, 20, lerp);

			GUI.DrawTexture(new Rect(Event.current.mousePosition.x - (scale/2), Event.current.mousePosition.y - (scale/2), scale, scale), chargeBar);
		}
	}
}
