using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {
	protected GameController controller;
	[SerializeField] AudioClip resetSound;
	[SerializeField] Tab[] tabs = new Tab[9];
	AudioSource aud;
	float soundVolume;

	private void Start() {
		controller = FindObjectOfType<GameController>();
		aud = GetComponent<AudioSource>();
		aud.clip = resetSound;
		aud.volume = PlayerPrefs.GetFloat("SoundVolume");
	}

	public void ResetBoard() {
		foreach (Tab t in tabs) {
			t.Unflip();
		}
		aud.Play();
	}
	
	public void UnselectAll() {
		foreach (Tab t in tabs) {
			t.Unselect();
		}
	}

	public void updateSoundVolume(float newVolume) {
		aud.volume = PlayerPrefs.GetFloat("SoundVolume");
		foreach (Tab t in tabs) {
			t.updateSoundVolume(newVolume);
		}
	}
}
