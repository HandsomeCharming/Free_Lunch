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
			Vector2 pos = new Vector2(transform.position.x, transform.position.z);
			if(Vector2.Distance(pos, movePos) <= 0.2f) {
				isMoving = false;
			}
			moveToward((movePos - pos).normalized);
		}
	}
}
