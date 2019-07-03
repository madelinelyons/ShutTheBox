using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour {
	private Rigidbody rb;
	private bool tabRelease = true;
	[SerializeField] GameController controller;
	private readonly float padding = .5f;
	private bool rolled = false;
	[SerializeField] AudioClip diceRolling;
	AudioSource audioSource;
	float soundVolume;
	Vector3 MaxCornPos;
	Vector3 MinCornPos;
	float diceXLeftBound;
	float diceXRightBound;
	[SerializeField] float rollSpeed = 20f;

	private void Start() {
		rb = GetComponent<Rigidbody>();
		audioSource = GetComponent<AudioSource>();
		soundVolume = PlayerPrefs.GetFloat("SoundVolume");
		audioSource.clip = diceRolling;
		audioSource.volume = soundVolume;

		MaxCornPos = GameObject.Find("MaxCorn").transform.position;
		MinCornPos = GameObject.Find("MinCorn").transform.position;
		diceXLeftBound = Camera.main.WorldToScreenPoint(MaxCornPos).x;
		diceXRightBound = Camera.main.WorldToScreenPoint(MinCornPos).x;
	}

	public void Roll(float offset) {
		if (tabRelease) {
			if (Input.GetMouseButton(0) && InRange()) {
				Spin();
				controller.TurnOffRollText();
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				if (Physics.Raycast(ray)) {
					if (this.name == "Dice" && !audioSource.isPlaying) {
						audioSource.Play();
					}
					var mousePos = new Vector3(Mathf.Clamp(Input.mousePosition.x, Upperbounds('x') - padding, Lowerbounds('x') + padding) + offset, Mathf.Clamp(Input.mousePosition.y, Upperbounds('y'), Lowerbounds('y')), 15);
					rb.velocity = new Vector3(Mathf.Clamp(rb.velocity.x, -10f, 10f), Mathf.Clamp(rb.velocity.y, -10f, 10f), Mathf.Clamp(rb.velocity.z, -10f, 10f));
					transform.position = Camera.main.ScreenToWorldPoint(mousePos);
					rolled = true;
				}
			}
			if ((Input.GetMouseButtonUp(0) && rolled) || (!InRange() && rolled)) {
				audioSource.Stop();
				controller.SetRollTime(false);
			}
		}
		else {
			if (Input.GetMouseButtonUp(0)) {
				tabRelease = true;
			}
		}
	}

	private bool InRange() {
		if (Input.mousePosition.x >= diceXLeftBound && Input.mousePosition.x <= diceXRightBound) {
			return true;
		}
		else {
			return false;
		}
	}

	private float Lowerbounds(char ch) {
		Vector3 minPos = MinCornPos;
		Vector3 screenPoint = Camera.main.WorldToScreenPoint(minPos);

		if(ch == 'x') {
			return screenPoint.x;
		}
		else if(ch == 'y') {
			return screenPoint.y;
		}
		else {
			return 0f;
		}
	}

	private float Upperbounds(char ch) {
		Vector3 maxPos = MaxCornPos;
		Vector3 screenPoint = Camera.main.WorldToScreenPoint(maxPos);

		if (ch == 'x') {
			return screenPoint.x;
		}
		else if (ch == 'y') {
			return screenPoint.y;
		}
		else {
			return 0f;
		}
	}
	
	void Spin() {
		var pos = transform.rotation;
		var newSpin = Random.rotation;
		transform.rotation = Quaternion.Slerp(pos, newSpin, Time.deltaTime * rollSpeed);
	}

	 public int DetermineUpSide() {
		string upSide = "";
		if (rolled && rb.velocity == new Vector3(0, 0, 0)) {

			if(Mathf.Round(transform.eulerAngles.x) % 90 != 0 || Mathf.Round(transform.eulerAngles.z) % 90 !=0) {
				Kick();
				return 0;
			}

			float highestY = 0;

			//figure out if it's cocked
			foreach (Transform child in transform) {
				if (child.gameObject.transform.position.y > highestY) {
					highestY = child.gameObject.transform.position.y;
					upSide = child.gameObject.name;
				}
			}
			

		}
		if (upSide != "") {
			return GetFaceNum(upSide);
		}
		else {
			return 0;
		}
	}

	void Kick() {
		rb.AddForce(new Vector3(0,1,0) * 200); //try it random and try it up
	}

	int GetFaceNum(string upSide) {
		char ch = upSide[4];
		double num = char.GetNumericValue(ch);
		return (int)num;
	}

	public void NotRolled() {
		rolled = false;
	}

	public void TabControl() {
		tabRelease = false;
	}

	public void updateSoundVolume(float newVolume) {
		audioSource.volume = PlayerPrefs.GetFloat("SoundVolume");
	}
}
