using UnityEngine;
using System.Collections;

public class LevelController : MonoBehaviour {
	public float playerSpawnPosX;
	public float playerSpawnPosY;

	public Vector2 cursurHotspot = Vector2.zero;
	public Texture2D cursorTexture;
	public CursorMode cursorMode;

	public int cursorSize = 63;
	int sizeX;
	int sizeY;

	void Awake() {
		sizeX = cursorSize;
		sizeY = cursorSize;
	}

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
		Vector3 pos = new Vector3 (playerSpawnPosX, 0, playerSpawnPosY);		
		player.gameObject.transform.position = pos;

		//Cursor.SetCursor(cursorTexture, cursurHotspot, cursorMode);
		//Cursor.visible = false;
	}

	Character loadPlayer() {
		return ((GameObject)Instantiate(Resources.Load("Prefabs/DivisionCharacterB"))).GetComponent<Character>();
	}

	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI()
	{
		GUI.DrawTexture(new Rect(Event.current.mousePosition.x - (cursorSize/2), Event.current.mousePosition.y - (cursorSize/2), sizeX, sizeY), cursorTexture);
	}
}
