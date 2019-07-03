using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSingleton : MonoBehaviour{
	int numPlayers;

	public void Awake() {
		int singleCount = FindObjectsOfType(GetType()).Length;
		if (singleCount > 1) {
			gameObject.SetActive(false);
			Destroy(gameObject);
		}
		else {
			DontDestroyOnLoad(gameObject);
		}
	}

	public void StoreNumPlayers(int num) {
		numPlayers = num;
	}

	public int GetNumPlayers() {
		return numPlayers;
	}

}
