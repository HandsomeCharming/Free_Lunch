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
		character.moveToward(new Vector2(dir.x, dir.z).normalized);
		character.faceToward(new Vector2(dir.x, dir.z).normalized);
		character.startCharging();
	}

	public GameObject makeEnemy(int type, Vector3 pos) {
		GameObject obj = (GameObject)Instantiate(Resources.Load("Prefabs/TempCubeEnemy"), pos, Quaternion.identity);
		return obj;
	}

	IEnumerator makeTempEnemy() {
		while(true) {
			yield return new WaitForSeconds(5f);
			if(!autoSpawnEnemy) continue;
			Vector3 pos = player.transform.position + (Random.Range(0,1)==0?-1:1) * new Vector3(Random.Range(20f,30f), 0 ,Random.Range(20f,30f));
			GameObject obj = (GameObject)Instantiate(Resources.Load("Prefabs/RotateEnemy"), pos, Quaternion.Euler(new Vector3(270f,90f,0f)));
		}
	}
}

