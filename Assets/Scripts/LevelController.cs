using UnityEngine;
using System.Collections;

public class LevelController : MonoBehaviour {
	public float playerSpawnPosX;
	public float playerSpawnPosY;
	// Use this for initialization
	void Start () {
		Projectile.InitProjectileData();

		Instantiate(Resources.Load("Prefabs/InputHandler"));
		Instantiate(Resources.Load("Prefabs/AIController"));

		//GameObject pobj = (GameObject)Instantiate(Resources.Load("Prefabs/DivisionCharacterB"));
		Character player = loadPlayer();
		MainCamera.current.character = player;
		InputHandler.current.character = player;
		AIController.current.player = player;
<<<<<<< HEAD
		Vector3 pos = new Vector3 (-320, 0, -370);		
=======
		Vector3 pos = new Vector3 (playerSpawnPosX, 0, playerSpawnPosY);		
>>>>>>> 16394d8a2d5f72a10ef4a36406bfe0180112790b
		player.gameObject.transform.position = pos;
	}

	Character loadPlayer() {
		return ((GameObject)Instantiate(Resources.Load("Prefabs/DivisionCharacterB"))).GetComponent<Character>();
	}

	// Update is called once per frame
	void Update () {
	
	}
}
