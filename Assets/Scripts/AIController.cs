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
			character.moveToward(new Vector2(dir.x, dir.z).normalized);
			character.faceToward(new Vector2(dir.x, dir.z).normalized);
			character.attack();
			break;
		case 6:
			character.faceToward(new Vector2(dir.x, dir.z).normalized);
			if(character.actionCdRemain[1] == 0) {
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
		}
	}
}

