using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLogic : MonoBehaviour{
	[SerializeField] Lid lid;
	[SerializeField] BoxScene box;
	MainMenuControl startMenu;
	StatsDisplay statsDisplay;
	OptionsControl optionsControl;
	CornerMenuController cornerController;
	GameController controller;

	private void Start() {
		controller = FindObjectOfType<GameController>();
		cornerController = FindObjectOfType<CornerMenuController>();
	}

	public void LoadStartMenu() {
		SceneManager.LoadScene(0);
	}

	public void Play() {
		startMenu = FindObjectOfType<MainMenuControl>();
		startMenu.TurnOffMMCanvas();
		startMenu.TurnOnPMCanvas();
	}

	public void PlayMenuBack() {
		startMenu.TurnOffMPPlayOptions();
		startMenu.TurnOffAIPlayOptions();
		startMenu.TurnOffPMCanvas();
		startMenu.TurnOnMMCanvas();
	}

	public void Rules() {
		optionsControl = FindObjectOfType<OptionsControl>();
		startMenu = FindObjectOfType<MainMenuControl>();
		startMenu.TurnOffMMCanvas();
		optionsControl.TurnOnRulesCanvas();
	}

	public void RulesMenuBack() {
		optionsControl.TurnOffRulesCanvas();
		startMenu.TurnOnMMCanvas();
	}

	public void Stats() {
		statsDisplay = FindObjectOfType<StatsDisplay>();
		startMenu = FindObjectOfType<MainMenuControl>();
		startMenu.TurnOffMMCanvas();
		statsDisplay.TurnOnStatCanvas();
	}

	public void StatsMenuBack() {
		statsDisplay.TurnOffStatCanvases();
		startMenu.TurnOnMMCanvas();
	}

	public void Options() {
		optionsControl = FindObjectOfType<OptionsControl>();
		startMenu = FindObjectOfType<MainMenuControl>();
		startMenu.TurnOffMMCanvas();
		optionsControl.TurnOnOptionsCanvas();
	}

	public void OptionsMenuBack() {
		optionsControl.TurnOffOptionsCanvas();
		startMenu.TurnOnMMCanvas();
	}

	public void Thanks() {
		optionsControl = FindObjectOfType<OptionsControl>();
		startMenu = FindObjectOfType<MainMenuControl>();
		startMenu.TurnOffMMCanvas();
		optionsControl.TurnOnThanksCanvas();
	}

	public void ThanksMenuBack() {
		optionsControl.TurnOffThanksCanvas();
		startMenu.TurnOnMMCanvas();
	}

	public void SinglePlay() {
		StartCoroutine(WaitThenLoad(1));
	}

	public void MPButton() {
		startMenu.TurnOffAIPlayOptions();
		startMenu.TurnOnMPPlayOptions();
	}

	public void MultiPlay() {
		StartCoroutine(WaitShiftThenLoad(2));
	}

	public void AIButton() {
		startMenu.TurnOffMPPlayOptions();
		startMenu.TurnOnAIPlayOptions();

	}

	public void AIPlay() {
		StartCoroutine(WaitShiftThenLoad(3));
	}

	public void ShutBox() {
		controller.MakeDiceInvisibleNow();
		StartCoroutine(CloseBox());
	}

	public void MainMenuCloseBox() {
		controller.MakeDiceInvisibleNow();
		StartCoroutine(ShiftBackCloseBox());
	}

	public void MainMenuKeepBoxClosed() {
		StartCoroutine(ShiftBack());
	}

	IEnumerator WaitThenLoad(int buildIndex) {
		startMenu.TurnOffPMCanvas();
		lid.Open();
		yield return new WaitForSeconds(lid.OpenAnimPlayTime());
		SceneManager.LoadScene(buildIndex);
	}

	IEnumerator WaitShiftThenLoad(int buildIndex) {
		startMenu.TurnOffPMCanvas();
		box.ShiftRight();
		lid.Open();
		yield return new WaitForSeconds(lid.OpenAnimPlayTime());
		SceneManager.LoadScene(buildIndex);
	}

	IEnumerator ShiftBackCloseBox() {
		controller.Resume();
		TurnOffCanvases();
		box.ShiftLeft();
		lid.Close();
		yield return new WaitForSeconds(lid.CloseAnimPlayTime());
		SceneManager.LoadScene(0);
	}

	IEnumerator ShiftBack() {
		controller.Resume();
		TurnOffCanvases();
		box.ShiftLeft();
		yield return new WaitForSeconds(lid.CloseAnimPlayTime());
		SceneManager.LoadScene(0);
	}

	IEnumerator CloseBox() {
		cornerController.MenuDisappear();
		controller.Resume();
		controller.TurnOffGameCanvas();
		lid.Close();
		yield return new WaitForSeconds(lid.CloseAnimPlayTime());
		SceneManager.LoadScene(0);
	}

	public void TurnOffCanvases() {
		cornerController.MenuDisappear();

		var scene = SceneManager.GetActiveScene();
		int index = scene.buildIndex;

		if(index == 2) {
			MultiplayerGameLogic mp = FindObjectOfType<MultiplayerGameLogic>();
			mp.TurnOffGameCanvas();
		}

		else if(index == 3){
			AIGameLogic ap = FindObjectOfType<AIGameLogic>();
			ap.TurnOffGameCanvas();
		}
	}

	public void LoadGame() {
		SceneManager.LoadScene(1);
	}

	public void PlayAgain() {
		controller.Reset();
	}
}
