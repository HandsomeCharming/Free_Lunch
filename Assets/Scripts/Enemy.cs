using UnityEngine;
using System.Collections;

public class Enemy : Character {
	public bool isMoving;
	public Vector2 movePos;

	public override void moveTo (Vector2 pos)
	{
		isMoving = true;
		movePos = pos;
	}

	public void Update() {
		base.Update();
		if(isMoving) {

		}
	}
}
