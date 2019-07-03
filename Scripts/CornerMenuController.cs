using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CornerMenuController : MonoBehaviour {
	[SerializeField] GameObject cornerMenu;
	[SerializeField] GameObject optionsMenu;

	[SerializeField] Slider musicSlider;
	[SerializeField] Slider soundSlider;
	[SerializeField] Board board;
	[SerializeField] Dice dice;

	GameController controller;
	EditorController editorController;
	MusicSingleton musicPlayer;



	// Start is called before the first frame update
	void Start() {
		cornerMenu.gameObject.SetActive(false);
		optionsMenu.gameObject.SetActive(false);
		controller = FindObjectOfType<GameController>();
		editorController = FindObjectOfType<EditorController>();
		musicPlayer = FindObjectOfType<MusicSingleton>();

		InitializeSliders();
		musicSlider.onValueChanged.AddListener(delegate { MusicChangeCheck(); });
		soundSlider.onValueChanged.AddListener(delegate { SoundChangeCheck(); });
	}

	public void TurnOnMenu() {
		if (editorController != null && editorController.EditModeActive()) {
			editorController.TurnOffEditMode();
		}
		controller.Pause();
		cornerMenu.gameObject.SetActive(true);
	}

	public void TurnOffMenu() {
		controller.Resume();
		cornerMenu.gameObject.SetActive(false);
	}

	public void TurnOnOptions() {
		optionsMenu.gameObject.SetActive(true);
	}

	public void TurnOffOptions() {
		optionsMenu.gameObject.SetActive(false);
	}

	public void MenuDisappear() {
		cornerMenu.gameObject.SetActive(false);
	}

	public void InitializeSliders() {
		if (PlayerPrefs.HasKey("MusicVolume")) {
			musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
		}
		if (PlayerPrefs.HasKey("SoundVolume")) {
			soundSlider.value = PlayerPrefs.GetFloat("SoundVolume");
		}
	}

	public void MusicChangeCheck() {
		float musicVolume = musicSlider.value;
		PlayerPrefs.SetFloat("MusicVolume", musicVolume);
		musicPlayer.UpdateMusicVolume();
	}

	public void SoundChangeCheck() {
		float soundVolume = soundSlider.value;
		PlayerPrefs.SetFloat("SoundVolume", soundVolume);
		board.updateSoundVolume(soundVolume);
		dice.updateSoundVolume(soundVolume);
	}

}
