using UnityEngine;
using System.Collections;

public class AIController : MonoBehaviour {

	//In case we add coorperation mode, change this to an array.
	public Character player;

	public static AIController current;

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
		handleAllAI();
	}

	void handleAllAI() {
		foreach (Character character in characters) {
			handleAI(character);
		}
	}

	void handleAI(Character character) {
		Vector3 dir = player.transform.position - character.transform.position;
		character.moveToward(new Vector2(dir.x, dir.z).normalized);
		character.faceToward(new Vector2(dir.x, dir.z).normalized);
		character.attack();
	}

	IEnumerator makeTempEnemy() {
		while(true) {
			yield return new WaitForSeconds(5f);
			Vector3 pos = player.transform.position + (Random.Range(0,1)==0?-1:1) * new Vector3(Random.Range(4f,8f), 0 ,Random.Range(4f,8f));
			//GameObject obj = (GameObject)Instantiate(Resources.Load("Prefabs/TempCubeEnemy"), pos, Quaternion.identity);
		}
	}
}

