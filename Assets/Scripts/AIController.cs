using UnityEngine;
using System.Collections;

public class AIController : MonoBehaviour {

	//In case we add coorperation mode, change this to an array.
	public Character player;

	public static AIController current;

	public bool autoSpawnEnemy = true;

	public ArrayList characters {get; set;}
	public int characterCount {get; set;}

	public AIController() {
		current = this;
	}

	// Use this for initialization
	void Start () {
		characters = new ArrayList();
		current = this;
		StartCoroutine(makeTempEnemy());
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.timeScale == 0)return;
		if(player == null) return;
		handleAllAI();
	}

	void handleAllAI() {
		if(characters.Count == 0)return;
		foreach (Character character in characters) {
			handleAI(character);
		}
	}

	void handleAI(Character character) {
		Vector3 dir = player.transform.position - character.transform.position;
		switch(character.type) {
		case 5:
			Enemy enemy = (Enemy) character;
			//character.moveToward(new Vector2(dir.x, dir.z).normalized);
			if(!enemy.isMoving) {
				if(Vector3.Distance(player.transform.position, enemy.transform.position)>50f) {
					enemy.moveToward(new Vector2(dir.x, dir.z).normalized);
				} else {
					Random.Range(50,100);
					enemy.moveTo(new Vector2(character.transform.position.x + Random.onUnitSphere.x*10f, character.transform.position.z + Random.onUnitSphere.y * 10f));
				}
			}

			enemy.faceToward(new Vector2(dir.x, dir.z).normalized);
			enemy.attack();
			break;
		case 6:
			character.faceToward(new Vector2(dir.x, dir.z).normalized);
			if(character.actionCdRemain[1] == 0) {
				character.startCharging(); 
			} else {
				
			}
			break;
		case 7:
			character.moveToward(new Vector2(dir.x, dir.z).normalized);
			character.faceToward(new Vector2(dir.x, dir.z).normalized);
			if(character.actionCdRemain[1] == 0 && Vector3.Distance(character.transform.position, player.transform.position) <= 5f ) {
				character.startCharging(); 
			} else {

			}
			break;
		}
	}

	public GameObject makeEnemy(int type, Vector3 pos) {
		GameObject obj = null;
		switch(type) {
		case 5: 
			obj= (GameObject)Instantiate(Resources.Load("Prefabs/TempCubeEnemy"), pos, Quaternion.identity);
			break;
		case 6:
			obj= (GameObject)Instantiate(Resources.Load("Prefabs/RotateEnemy"), pos, Quaternion.identity);
			break;
		case 7:
			obj = (GameObject)Instantiate (Resources.Load ("Prefabs/TempBomb"), pos, Quaternion.identity);
			break;
		}
		return obj;
	}

	IEnumerator makeTempEnemy() {
		while(true) {
			yield return new WaitForSeconds(5f);
			if(!autoSpawnEnemy) continue;
			Vector3 pos = player.transform.position + (Random.Range(0,1)==0?-1:1) * new Vector3(Random.Range(20f,30f), 0 ,Random.Range(20f,30f));
			//GameObject obj = (GameObject)Instantiate(Resources.Load("Prefabs/RotateEnemy"), pos, Quaternion.Euler(new Vector3(270f,90f,0f)));

			//GameObject obj = (GameObject)Instantiate(Resources.Load("Prefabs/TempCubeEnemy"), pos, Quaternion.identity);
			//GameObject obj = (GameObject)Instantiate(Resources.Load("Prefabs/TempBomb"), pos, Quaternion.identity);
		}
	}
}

