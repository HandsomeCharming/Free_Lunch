using UnityEngine;
using System.Collections;

public class EnemyTrigger : EventTrigger {
	bool triggered = false;

	void OnTriggerEnter(Collider coll) {
		if(triggered == true)return;
		triggered = true;
		EnemySpawnPoint[] spawnPoints = (EnemySpawnPoint[])this.GetComponentsInChildren<EnemySpawnPoint>();
		for(int a=0;a!=spawnPoints.Length;++a) {
			AIController.current.makeEnemy(spawnPoints[a].enemyType, spawnPoints[a].gameObject.transform.position);
		}
	}
}
