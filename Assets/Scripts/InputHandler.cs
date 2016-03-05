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
	float blockTime = 0;
	float activeTime = 0;

	int usingSkillType = 0;

	public InputHandler() {
		current = this;
	}

	void Start () {
		current = this;
	}

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
				if(vertical == 0 && horizontal == 0) 
					stand();
				if(vertical != 0 || horizontal != 0) {
					move ();
					character.state = CharacterState.Move;
				}
				if(Input.GetKeyDown(KeyCode.Space) && character.actionCdRemain[2] == 0) {
					dodge();
					character.state = CharacterState.Dodge;
				} else if(Input.GetMouseButton(0)) {
					attack();
				} else if(Input.GetMouseButtonDown(1)) {
					startCharging();
					character.state = CharacterState.ChargedAttack;
				} else if (Input.GetKeyDown(KeyCode.LeftShift) && character.actionCdRemain[3] == 0) {
					block();
					character.state = CharacterState.Block;
				} else if(Input.GetKeyDown(KeyCode.C) && character.actionCdRemain[4] == 0) {
					useActive(0);
					if(((ActiveModifier)character.activeSkills[0]).existTime > 0)
						character.state = CharacterState.UseSkill;
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
				if(Input.GetKeyDown(KeyCode.Space) && character.actionCdRemain[2] == 0) {
					dodge();
					character.state = CharacterState.Dodge;
				} else if(Input.GetMouseButton(0)) {
					attack();
				} else if(Input.GetMouseButtonDown(1)) {
					startCharging();
				} else if (Input.GetKeyDown(KeyCode.LeftShift) && character.actionCdRemain[3] == 0) {
					block();
					character.state = CharacterState.Block;
				} else if(Input.GetKeyDown(KeyCode.C) && character.actionCdRemain[4] == 0) {
					useActive(0);
					if(((ActiveModifier)character.activeSkills[0]).existTime > 0)
						character.state = CharacterState.UseSkill;
				}

				break;
			}
			case CharacterState.Attack: {
				break;
			}
			case CharacterState.ChargedAttack: {
				face ();
				if(Input.GetKeyDown(KeyCode.Space) && character.actionCdRemain[2] == 0) {
					dodge();
					character.state = CharacterState.Dodge;
					chargedAttack();
				} else if(Input.GetMouseButtonUp(1)) {
					character.state = CharacterState.Move;
					chargedAttack ();
				} else if (Input.GetKeyDown(KeyCode.LeftShift) && character.actionCdRemain[3] == 0) {
					block();
					character.state = CharacterState.Block;
					chargedAttack();
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
						dodgeTime = 0;
					character.state = CharacterState.Move;
				}
				break;
			}
			case CharacterState.Block: {
				face();
				move();
				blockTime += Time.deltaTime;
				if(blockTime >= character.blockModifier.blockTime) {
					blockTime = 0;
					character.state = CharacterState.Move;
				}
				break;
			}
			case CharacterState.UseSkill: {
				face();
				activeTime += Time.deltaTime;
				if(activeTime >= ((ActiveModifier)character.activeSkills[usingSkillType]).existTime) {
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

	void stand() {
		character.stand();
	}

	void dodge() {
		dodgeTime = 0;
		float vertical = Input.GetAxis("Vertical");
		float horizontal = Input.GetAxis("Horizontal");
		if(vertical != 0 || horizontal != 0) {
			character.dodgeToward(new Vector2(horizontal, vertical).normalized);
		} else {
			character.dodgeToward(new Vector2(1f, 0).normalized);
		}
	}

	void block() {
		character.block();
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

	void useActive(int activeType) {

	}

	void chargedAttack() {
		character.chargedAttack(chargingTime);
	}
}
