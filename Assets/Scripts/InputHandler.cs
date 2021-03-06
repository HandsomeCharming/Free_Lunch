﻿using UnityEngine;
using System.Collections;

public class InputHandler : MonoBehaviour {

	public static InputHandler current;

	public Character character;

	public float x;
	public float y;
	public float z;

	public float chargingTime = 0;
	float dodgeTime = 0;
	public float blockTime = 0;
	float activeTime = 0;

	int usingSkillType = 0;

	public bool paused = false;
	public bool canInput = true;

	Canvas pauseMenu;

	public InputHandler() {
		current = this;
	}

	void Start () {
		current = this;
		GameObject obj;
		if(Application.loadedLevelName == "LevelSelectMenu") {
			obj = (GameObject)Instantiate(Resources.Load("Prefabs/PauseMenuLevelSelect"));
		} else {
			obj = (GameObject)Instantiate(Resources.Load("Prefabs/PauseMenuInGame"));
		}
		//pauseMenu = (Canvas)obj;
		//obj.SetActive(false);
		pauseMenu = obj.GetComponent<Canvas>();
		pauseMenu.enabled = false;
	}

	void Update () {
		handleInput();
	}

	public void stopInput() {
		canInput = false;
	}

	public void enableInput() {
		canInput = true;
	}

	public void pause() {
		Time.timeScale = 0;
		pauseMenu.enabled = true;
		paused = true;
	}

	public void resume() {
		Time.timeScale = 1f;
		pauseMenu.enabled = false;
		paused = false;
	}

	void handleInput () {
		if(character == null) return;
		if(!canInput) {
			character.stop();
			return;
		}

		if(Input.GetKeyDown(KeyCode.Escape)) {
			if(!paused) {
				pause();
			}
			else {
				resume();
			}
		} 
		if(paused )return;
		if (Input.GetKeyDown (KeyCode.Q)) {
			character.status.hp = 100;
		}
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
					if(((ActiveModifier)character.activeSkills[0]).existTime > 0) {
						character.state = CharacterState.UseSkill;
						usingSkillType = 0;
					}
				} else if(Input.GetKeyDown(KeyCode.V) && character.actionCdRemain[5] == 0) {
					useActive(1);
					if(((ActiveModifier)character.activeSkills[1]).existTime > 0) {
						character.state = CharacterState.UseSkill;
						usingSkillType = 1;
					}
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
					if(((ActiveModifier)character.activeSkills[0]).existTime > 0) {
						character.state = CharacterState.UseSkill;
						usingSkillType = 0;
					}
				} else if(Input.GetKeyDown(KeyCode.V) && character.actionCdRemain[5] == 0) {
					useActive(1);
					if(((ActiveModifier)character.activeSkills[1]).existTime > 0) {
						character.state = CharacterState.UseSkill;
						usingSkillType = 1;
					}
				}

				break;
			}
			case CharacterState.Attack: {
				break;
			}
			case CharacterState.ChargedAttack: {
				face ();

				chargingTime+=Time.deltaTime;
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
				if(character.blockModifier.type == 203) {
					face();
					character.stop();
					if(Input.GetMouseButton(0)) {
						attack();
					} else if(Input.GetMouseButton(1) && character.actionCdRemain[1] == 0) {
						chargingTime+=Time.deltaTime;
						if(chargingTime >= character.chargedAttackModifier.chargeTime*0.5f) {
							chargedAttack ();
						}
					} 
					if(Input.GetMouseButtonUp(1)) {
						chargedAttack();
					}
				}
				else if(character.blockModifier.type != 204) {
					face();
					move();
				}
				blockTime += Time.deltaTime;
				if(Input.GetKeyDown(KeyCode.LeftShift) ||  blockTime >= character.blockModifier.blockTime) {
					blockTime = 0;
					character.state = CharacterState.Move;
				}
				if(Input.GetKeyDown(KeyCode.Space) && character.actionCdRemain[2] == 0) {
					blockTime = 0;
					dodge();
					character.state = CharacterState.Dodge;
				}
				break;
			}
			case CharacterState.UseSkill: {
				face();
				move();
				activeTime += Time.deltaTime;

				if(activeTime >= ((ActiveModifier)character.activeSkills[usingSkillType]).existTime) {
					character.state = CharacterState.Move;
					activeTime = 0f;
				}
				break;
			}
			default:
				break;
		}
	}

	void face() {
		Vector3 mousePosition = Input.mousePosition;
		mousePosition.z = MainCamera.current.transform.position.y;
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
		character.stop();
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
		character.useSkill(activeType);
	}

	void chargedAttack() {
		character.chargedAttack(chargingTime);
		chargingTime = 0;
	}
}
