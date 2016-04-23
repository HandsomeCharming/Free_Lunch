using UnityEngine;
using System.Collections;

public class LevelLoad : MonoBehaviour {
	public static int lastScene;

	// Use this for initialization
	void Start () {
		lastScene = Application.loadedLevel;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
