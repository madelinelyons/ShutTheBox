using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tab : MonoBehaviour{
	private bool faceUp = true;
	private bool selected = false;
	Animator anim;
	RuntimeAnimatorController animController;
	Renderer rend;
	[SerializeField] Material notGlowing;
	[SerializeField] Material glowing;
	[SerializeField] AudioClip flipSound;
	AudioSource audioSource;
	float soundVolume;
	GameController controller;

	private void Start() {
		controller = FindObjectOfType<GameController>();
		anim = GetComponent<Animator>();
		animController = anim.runtimeAnimatorController;
		rend = GetComponent<Renderer>();
		audioSource = GetComponent<AudioSource>();
		audioSource.volume = PlayerPrefs.GetFloat("SoundVolume");
		anim.enabled = true;
		audioSource.clip = flipSound;
	}

	private void Update() {
		if (Input.GetMouseButtonDown(0) && !controller.GetRollTime()) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(ray, out hit)) {
				if (hit.transform.name == this.gameObject.name) {
					if (selected == false) {
						selected = true;
						controller.AddSelected(this);
						Glow();
					}
					else {
						selected = false;
						controller.RemoveSelected(this);
						Unglow();
					}
				}
			}
		}
	}

	public int GetNumber(){
		string name = this.gameObject.name;
		char ch = name[3];
		double num = char.GetNumericValue(ch);
		return (int) num;
	}

	public void Flip() {
		if (faceUp) {
			audioSource.Play();
			anim.Play("Flip");
			faceUp = false;
		}
		Unglow();
	}
	
	public float FlipAnimationPlayTime() {
		return animController.animationClips[0].length;
	}

	public void Unflip() {
		if (!faceUp) {
			anim.Play("Unflip");
			faceUp = true;
		}
		Unglow();
	}

	void Glow() {
		if (faceUp) {
			rend.material = glowing;
		}
	}

	void Unglow() {
		rend.material = notGlowing;
	}
	
	public void Unselect() {
		selected = false;
	}

	public void updateSoundVolume(float newVolume) {
		audioSource.volume = PlayerPrefs.GetFloat("SoundVolume");
	}
}
