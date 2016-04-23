using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveLoad : MonoBehaviour {
	public static void Save() {
		if(GameSave.current == null)GameSave.current = new GameSave();
		Debug.Log(Application.persistentDataPath);
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + "/savedGames.gd");
		bf.Serialize(file, GameSave.current);
		file.Close();
	}

	public static bool Load() {
		if(File.Exists(Application.persistentDataPath + "/savedGames.gd")) {
			BinaryFormatter bf = new BinaryFormatter();
			Debug.Log(Application.persistentDataPath);
			FileStream file = File.Open(Application.persistentDataPath + "/savedGames.gd", FileMode.Open);
			GameSave.current = (GameSave)bf.Deserialize(file);
			if(GameSave.current == null) return false;
			file.Close();
			return true;
		} else {
			Debug.Log("Load failed");
			GameSave.current = new GameSave();
			return false;
		}
	}

}
