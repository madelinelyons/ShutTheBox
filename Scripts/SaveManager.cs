using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveManager : MonoBehaviour{
	[SerializeField] Profile profile;

	public void SaveStats() {
		if (!Directory.Exists(Application.persistentDataPath + "/saves")) {
			Directory.CreateDirectory(Application.persistentDataPath + "/saves");
		}

		BinaryFormatter bf = new BinaryFormatter();
		FileStream saveFile = File.Create(Application.persistentDataPath + "/saves/stats.dat");

		var json = JsonUtility.ToJson(profile);
		bf.Serialize(saveFile, json);

		saveFile.Close();
	}

	public void LoadStats() {
		if (!Directory.Exists(Application.persistentDataPath + "/saves")) {
			Directory.CreateDirectory(Application.persistentDataPath + "/saves");
		}

		else {
			if (File.Exists(Application.persistentDataPath + "/saves/stats.dat")) {
				BinaryFormatter bf = new BinaryFormatter();
				FileStream saveFile = File.Open(Application.persistentDataPath + "/saves/stats.dat", FileMode.Open);

				JsonUtility.FromJsonOverwrite((string)bf.Deserialize(saveFile), profile);
				saveFile.Close();
			}
		}
	}
}
