using UnityEngine;
using System.Collections;

public class InputHandler : MonoBehaviour {
	public Character character;

	public float v;
	public float h;
	// Use this for initialization
	void Start () {
			
	}
	
	// Update is called once per frame
	void Update () {
		handleInput();
	}

	void handleInput () {
		CharacterState state = character.state;
		switch(state) {
		case CharacterState.Stand: {
			float vertical = Input.GetAxis("Vertical");
			float horizontal = Input.GetAxis("Horizontal");
			if(vertical != 0 || horizontal != 0) {
				character.moveToward(new Vector2(horizontal, vertical));
				character.state = CharacterState.Move;
			}
			break;
		}
		case CharacterState.Move: {
			float vertical = Input.GetAxis("Vertical");
			float horizontal = Input.GetAxis("Horizontal");
			v = vertical;
			h = horizontal;
			if(vertical != 0 || horizontal != 0) {
				character.moveToward(new Vector2(horizontal, vertical));
				character.state = CharacterState.Move;
			} else {
				character.state = CharacterState.Stand;
			}
			if(Input.GetMouseButtonDown(0)) {
				character.attackToward(new Vector2(1f,1f));
			}
			break;
		}
		case CharacterState.Attack: {
			break;
		}

		default:
			break;
		}
	}
}
