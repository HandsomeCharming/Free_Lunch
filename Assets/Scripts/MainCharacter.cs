﻿using UnityEngine;
using System.Collections;

public class MainCharacter : Character {

	public ParticleSystem cloud;
	public ParticleSystem lightning;

	public MainCharacter() {
		actionCds = new float[10];
		actionCdRemain = new float[10];
		
		for(int a=0;a!=10;++a)
			actionCds[a] = 0.3f;
		for(int a=0;a!=10;++a)
			actionCdRemain[a] = 0;

		dodgeModifier = new DodgeModifier();
	}


	//Currently only division.
	public override void attack() {
		if(actionCdRemain[0] > 0) return;
		actionCdRemain[0] = actionCds[0];
		Projectile.ShootProjectile(this, status.facingDirection, CharacterSkillType.Attack);
	}

	public override void chargedAttack() {
		if(actionCdRemain[1] > 0) return;
		actionCdRemain[1] = actionCds[1];
		status.moveSpeed = status.regularMoveSpeed;
		Projectile.ShootProjectile(this, status.facingDirection, CharacterSkillType.ChargedAttack);
	}

	public override void attackToward(Vector2 dir) {
		if(actionCdRemain[0] > 0) return;
		actionCdRemain[0] = actionCds[0];
		Projectile.ShootProjectile(this, dir, CharacterSkillType.Attack);
	}

	public override void dodgeToward(Vector2 dir) {
		faceToward(dir);
		StartCoroutine(dodgeLerp(dir));
	}

	IEnumerator dodgeLerp(Vector2 dir) {
		float time = 0;
		Vector3 pos = this.transform.position;
		Vector3 scale = this.transform.localScale;
		float scalez = scale.z;
		ParticleSystem.Particle []particleList = new ParticleSystem.Particle[cloud.particleCount];
		cloud.GetParticles(particleList);
		byte cloudAlpha = particleList[0].color.a;
		ParticleSystem.Particle []lightningList = new ParticleSystem.Particle[lightning.particleCount];
		lightning.GetParticles(lightningList);
		byte lightningAlpha = lightningList[0].color.a;
		while(time < dodgeModifier.dodgeTime) {
			pos += new Vector3(dir.x, 0, dir.y) * 2f;
			this.transform.position = pos;
			if(time < dodgeModifier.dodgeTime/2f) {
				scale.z = Mathf.Lerp(scalez, scalez/5f, time/(dodgeModifier.dodgeTime/2f));
			} else {
				scale.z = Mathf.Lerp(scalez/5f, scalez, (time-dodgeModifier.dodgeTime/2f)/(dodgeModifier.dodgeTime/2f));
			}

			cloud.GetParticles(particleList);
			for(int i = 0; i < particleList.Length; ++i)
			{
				Color32 color = particleList[i].color;
				if(time < dodgeModifier.dodgeTime/2f && color.a > 10) {
					color.a -= 10;
				} else if(time > dodgeModifier.dodgeTime/2f && color.a < cloudAlpha){
					color.a += 10;
				}
				particleList[i].color = color;
			}        
			cloud.SetParticles(particleList, cloud.particleCount);

			lightning.GetParticles(lightningList);
			for(int i = 0; i < lightningList.Length; ++i)
			{
				Color32 color = lightningList[i].color;
				if(time < dodgeModifier.dodgeTime/2f && color.a > 10) {
					color.a -= 10;
				} else if(time > dodgeModifier.dodgeTime/2f && color.a < lightningAlpha){
					color.a += 10;
				}
				particleList[i].color = color;
			}        
			lightning.SetParticles(lightningList, lightning.particleCount);

			this.transform.localScale = scale;
			yield return new WaitForEndOfFrame();
			time += Time.deltaTime;
		}

		cloud.GetParticles(particleList);
		for(int i = 0; i < particleList.Length; ++i)
		{
			Color32 color = particleList[i].color;
			color.a = cloudAlpha;
			particleList[i].color = color;
		}        
		cloud.SetParticles(particleList, cloud.particleCount);

		lightning.GetParticles(lightningList);
		for(int i = 0; i < lightningList.Length; ++i)
		{
			Color32 color = lightningList[i].color;
			color.a = lightningAlpha;
			particleList[i].color = color;
		}        
		lightning.SetParticles(lightningList, lightning.particleCount);

		scale.z = scalez;
		this.transform.localScale = scale;
	}

	// Use this for initialization
	void Start () {
		//Temporary collider setup
		SphereCollider collider = gameObject.AddComponent<SphereCollider>();
		collider.radius = 2.4f;

		if(GameSave.current == null) {
			SaveLoad.Load();
		}
	}
	
	// Update is called once per frame
	void Update () {
		for(int a=0;a!=10;++a) {
			if(actionCdRemain[a]>0) {
				actionCdRemain[a] -= Time.deltaTime;
			} 
			if(actionCdRemain[a] <= 0) actionCdRemain[a] = 0;
		}
	}
}