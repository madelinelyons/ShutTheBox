using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSingleton : MonoBehaviour {
	AudioSource audioSource;
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

	private void Start() {
		audioSource = GetComponent<AudioSource>();
		audioSource.volume = PlayerPrefs.GetFloat("MusicVolume", .65f);
		if (!audioSource.isPlaying && audioSource.volume > 0){
			audioSource.Play();
		}
	}

	public void UpdateMusicVolume() {
		if (!Application.isPlaying) return;

		audioSource.volume = PlayerPrefs.GetFloat("MusicVolume");

		if (!audioSource.isPlaying) {
			audioSource.Play();
		}
	}
}
