using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class OptionsControl : MonoBehaviour{
	[SerializeField] GameObject optionsCanvas;
	[SerializeField] GameObject thanksCanvas;
	[SerializeField] GameObject rulesCanvas;

	[SerializeField] TMP_InputField newName;
	[SerializeField] TextMeshProUGUI placeholder;

	[SerializeField] Slider musicSlider;
	[SerializeField] Slider soundSlider;

	Stats stats;
	MusicSingleton musicPlayer;
	int charLimit = 9;

	void Start(){
		stats = FindObjectOfType<Stats>();
		musicPlayer = FindObjectOfType<MusicSingleton>();
		placeholder.text = stats.getName();
		newName.characterLimit = charLimit;
		TurnOffOptionsCanvas();
		TurnOffRulesCanvas();
		TurnOffThanksCanvas();

		InitializeSliders();
		musicSlider.onValueChanged.AddListener(delegate { MusicChangeCheck(); });
		soundSlider.onValueChanged.AddListener(delegate { SoundChangeCheck(); });
    }

	private void Update() {
		
	}

	public void TurnOnOptionsCanvas() {
		optionsCanvas.gameObject.SetActive(true);
	}

	public void TurnOffOptionsCanvas() {
		optionsCanvas.gameObject.SetActive(false);
	}

	public void TurnOnRulesCanvas() {
		rulesCanvas.gameObject.SetActive(true);
	}

	public void TurnOffRulesCanvas() {
		rulesCanvas.gameObject.SetActive(false);
	}

	public void TurnOnThanksCanvas() {
		thanksCanvas.gameObject.SetActive(true);
	}

	public void TurnOffThanksCanvas() {
		thanksCanvas.gameObject.SetActive(false);
	}

	public void InitializeSliders() {
		soundSlider.value = PlayerPrefs.GetFloat("SoundVolume", soundSlider.maxValue);
		if(soundSlider.value == soundSlider.maxValue) {
			PlayerPrefs.SetFloat("SoundVolume", soundSlider.maxValue);
		}

		musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", musicSlider.maxValue);
		if(musicSlider.value == musicSlider.maxValue) {
			PlayerPrefs.SetFloat("MusicVolume", musicSlider.maxValue);
		}
	}

	public void ConfirmPlayerNameChange() {
		if (newName.text != "") {
			string pName = newName.text;
			stats.updateName(pName);
			placeholder.text = pName;
			newName.text = "";
			newName.MoveTextStart(true);
		}
	}

	public void MusicChangeCheck() {
		musicPlayer = FindObjectOfType<MusicSingleton>();
		float musicVolume = musicSlider.value;
		PlayerPrefs.SetFloat("MusicVolume", musicVolume);
		musicPlayer.UpdateMusicVolume();
	}

	public void SoundChangeCheck() {
		float soundVolume = soundSlider.value;
		PlayerPrefs.SetFloat("SoundVolume", soundVolume);
	}

}
