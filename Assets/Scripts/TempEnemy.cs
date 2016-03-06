using UnityEngine;
using System.Collections;

public class TempEnemy : Character {
	public int hits;

	public TempEnemy() : base() {
		type = 5;
		attackModifier = new AttackModifier(30);

		actionCds[0] = 1f;
	}
	// Use this for initialization
	void Start () {
		if(AIController.current != null) {
			AIController.current.characters.Add(this);
		}
		//hits = 3;
		status.moveSpeed = 0.2f;
		type = 5;
	}

	public override void attack() {
		if(actionCdRemain[0] > 0) return;
		actionCdRemain[0] = actionCds[0];
		Projectile.ShootProjectile(this, status.facingDirection, CharacterSkillType.Attack);
	}

	void Update() {
		base.Update();
	}

	public override void hit (Character other, CharacterSkillType skillType, int subType = 0)
	{
		base.hit (other, skillType, subType);
		print("hit");
		other.gotHit(this, 10f);
	}
}
