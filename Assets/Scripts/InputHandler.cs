using UnityEngine;
using System.Collections;

public class InputHandler : MonoBehaviour {
	public Character character;

	public float x;
	public float y;
	public float z;
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
			if(Input.GetMouseButtonDown(0)) {
				Vector3 mousePosition = Input.mousePosition;
				mousePosition.z = 50;
				mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
				mousePosition = mousePosition - character.transform.position;
				x = mousePosition.x;
				y = mousePosition.y;
				z = mousePosition.z;
				character.attackToward(new Vector2(mousePosition.x, mousePosition.z).normalized);
			}
			break;
		}
		case CharacterState.Move: {
			float vertical = Input.GetAxis("Vertical");
			float horizontal = Input.GetAxis("Horizontal");
			if(vertical != 0 || horizontal != 0) {
				character.moveToward(new Vector2(horizontal, vertical));
				character.state = CharacterState.Move;
			} else {
				character.state = CharacterState.Stand;
			}
			if(Input.GetMouseButtonDown(0)) {
				Vector3 mousePosition = Input.mousePosition;
				mousePosition.z = 50;
				mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
				mousePosition = mousePosition - character.transform.position;
				x = mousePosition.x;
				y = mousePosition.y;
				z = mousePosition.z;
				character.attackToward(new Vector2(mousePosition.x, mousePosition.z).normalized);
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
