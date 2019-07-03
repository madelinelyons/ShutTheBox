using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MainMenuControl : MonoBehaviour{

	[SerializeField] Canvas mainMenu;
	[SerializeField] Canvas playMenu;
	[SerializeField] GameObject mpPlayOptions;
	[SerializeField] GameObject aiPlayOptions;

	[SerializeField] TMP_InputField numPlayers;
	[SerializeField] TMP_InputField numAIPlayers;

	[SerializeField] PlayerSingleton singleton;

	SceneLogic manager;

	private void Start() {
		singleton = FindObjectOfType<PlayerSingleton>();
		manager = FindObjectOfType<SceneLogic>();
		mainMenu.gameObject.SetActive(true);
		playMenu.gameObject.SetActive(false);
		mpPlayOptions.gameObject.SetActive(false);
		aiPlayOptions.gameObject.SetActive(false);

		numPlayers.characterLimit = 1;
		numPlayers.keyboardType = TouchScreenKeyboardType.NumberPad;
		numPlayers.contentType = TMP_InputField.ContentType.IntegerNumber;

		numAIPlayers.characterLimit = 1;
		numAIPlayers.keyboardType = TouchScreenKeyboardType.NumberPad;
		numAIPlayers.contentType = TMP_InputField.ContentType.IntegerNumber;
	}

	public void TurnOnMMCanvas() {
		mainMenu.gameObject.SetActive(true);
	}

	public void TurnOffMMCanvas() {
		mainMenu.gameObject.SetActive(false);
	}

	public void TurnOnPMCanvas() {
		playMenu.gameObject.SetActive(true);
	}

	public void TurnOffPMCanvas() {
		playMenu.gameObject.SetActive(false);
	}

	public void TurnOnMPPlayOptions() {
		mpPlayOptions.gameObject.SetActive(true);
	}

	public void TurnOffMPPlayOptions() {
		mpPlayOptions.gameObject.SetActive(false);
	}

	public void TurnOnAIPlayOptions() {
		aiPlayOptions.gameObject.SetActive(true);
	}

	public void TurnOffAIPlayOptions() {
		aiPlayOptions.gameObject.SetActive(false);
	}

	public void MPConfirmButton() {
		int uInput;
		bool success = int.TryParse(numPlayers.text, out uInput);
		if(success && uInput >= 2 && uInput <= 6) {
			singleton.StoreNumPlayers(uInput);
			manager.MultiPlay();
		}
	}

	public void AIConfirmButton() {
		int uInput;
		bool success = int.TryParse(numAIPlayers.text, out uInput);
		if (success && uInput >= 1 && uInput <= 5) {
			singleton.StoreNumPlayers(uInput);
			manager.AIPlay();
		}
	}
}
