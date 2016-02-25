using UnityEngine;
using System.Collections;

public class InputHandler : MonoBehaviour {

	public static InputHandler current;

	public Character character;

	public float x;
	public float y;
	public float z;

	float chargingTime = 0;
	float dodgeTime = 0;
	// Use this for initialization

	public InputHandler() {
		current = this;
	}

	void Start () {
		current = this;
	}
	
	// Update is called once per frame
	void Update () {
		handleInput();
	}

	void handleInput () {
		CharacterState state = character.state;
		float vertical = Input.GetAxis("Vertical");
		float horizontal = Input.GetAxis("Horizontal");
		switch(state) {
		case CharacterState.Stand: {
			face ();
			if(vertical != 0 || horizontal != 0) {
				move ();
				character.state = CharacterState.Move;
			}
			if(Input.GetKeyDown(KeyCode.Space)) {
				dodge();
				character.state = CharacterState.Dodge;
			} else if(Input.GetMouseButton(0)) {
				attack();
			} else if(Input.GetMouseButtonDown(1)) {
				startCharging();
				character.state = CharacterState.ChargedAttack;
			}

			break;
		}
		case CharacterState.Move: {
			face ();
			if(vertical != 0 || horizontal != 0) {
				move ();
			} else {
				character.state = CharacterState.Stand;
			}
			if(Input.GetKeyDown(KeyCode.Space)) {
				dodge();
				character.state = CharacterState.Dodge;
			} else if(Input.GetMouseButton(0)) {
				attack();
			} else if(Input.GetMouseButtonDown(1)) {
				startCharging();
			}

			break;
		}
		case CharacterState.Attack: {
			break;
		}
		case CharacterState.ChargedAttack: {
			face ();
			if(Input.GetKeyDown(KeyCode.Space)) {
				dodge();
				character.state = CharacterState.Dodge;
				chargedAttack();
			} else if(Input.GetMouseButtonUp(1)) {
				character.state = CharacterState.Move;
				chargedAttack ();
			}

			move ();
			chargingTime+=Time.deltaTime;
			if(chargingTime >= character.chargedAttackModifier.chargeTime) {
				character.state = CharacterState.Move;
				chargedAttack ();
			}
			break;
		}
		case CharacterState.Dodge: {
			dodgeTime += Time.deltaTime;
			if(dodgeTime >= character.dodgeModifier.dodgeTime) {
				character.state = CharacterState.Move;
			}
			break;
		}
		default:
			break;
		}
	}

	void face() {
		Vector3 mousePosition = Input.mousePosition;
		mousePosition.z = 50;
		mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
		mousePosition = mousePosition - character.transform.position;
		x = mousePosition.x;
		y = mousePosition.y;
		z = mousePosition.z;
		character.faceToward(new Vector2(mousePosition.x, mousePosition.z).normalized);
	}

	void startCharging() {
		chargingTime = 0;
		character.state = CharacterState.ChargedAttack;
		character.startCharging();
	}

	void move() {
		float vertical = Input.GetAxis("Vertical");
		float horizontal = Input.GetAxis("Horizontal");
		if(vertical != 0 || horizontal != 0) {
			character.moveToward(new Vector2(horizontal, vertical));
		} 
	}

	void dodge() {
		dodgeTime = 0;
		float vertical = Input.GetAxis("Vertical");
		float horizontal = Input.GetAxis("Horizontal");
		if(vertical != 0 || horizontal != 0) {
			character.dodgeToward(new Vector2(horizontal, vertical).normalized);
		} 
	}

	void attack() {
		/*Vector3 mousePosition = Input.mousePosition;
		mousePosition.z = 50;
		mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
		mousePosition = mousePosition - character.transform.position;
		x = mousePosition.x;
		y = mousePosition.y;
		z = mousePosition.z;
		character.attackToward(new Vector2(mousePosition.x, mousePosition.z).normalized);*/
		character.attack ();
	}

	void chargedAttack() {
		character.chargedAttack(chargingTime);
	}
}
