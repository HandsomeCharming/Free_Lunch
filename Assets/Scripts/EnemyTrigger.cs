using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class EnemyTrigger : EventTrigger {
	bool triggered = false;
	bool allDead = false;

	public GameObject[] enemies;
	int enemyCount = 0;

	void Start() {
		enemies = new GameObject[20];
	}

	void OnTriggerEnter(Collider coll) {
		if(triggered == true || coll.tag != "Player")return;
		triggered = true;
		EnemySpawnPoint[] spawnPoints = (EnemySpawnPoint[])this.GetComponentsInChildren<EnemySpawnPoint>();
		for(int a=0;a!=spawnPoints.Length;++a) {
			enemies[enemyCount] = (AIController.current.makeEnemy(spawnPoints[a].enemyType, spawnPoints[a].gameObject.transform.position));
			enemyCount++;
		}
	}

	void Update() {
		if(!triggered || allDead)return;
		int deadCount = enemyCount;
		for(int a=0;a<enemyCount;++a) {
			if(enemies[a] == null) deadCount--;
		}
		if(deadCount == 0) {
			allDead = true;
			mEvent.Invoke();
		}
	}
}
