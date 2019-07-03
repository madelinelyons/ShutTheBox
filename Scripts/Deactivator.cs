using System.Collections;
using UnityEngine;

public class Deactivator : MonoBehaviour{
	void OnApplicationQuit() {
		MonoBehaviour[] scripts = FindObjectsOfType<MonoBehaviour>();
		foreach (MonoBehaviour script in scripts) {
			script.enabled = false;
		}
	}
}
