using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour{
	[SerializeField] TextMeshProUGUI scoreText;
	AIPlayLogic playLogic;
	string playerName;
	int score;
	int index;
	bool playing = true;
	bool playingRound = false;

	private void Start() {
		playLogic = FindObjectOfType<AIPlayLogic>();
		playerName = name;
		score = 123456789;
		scoreText.text = "";
		index = GetIndex();
	}

	public void SetName(string newName) {
		playerName = newName;
	}

	public void SetScore(int newScore) {
		score = newScore;
		if (score == 0) {
			scoreText.text = "Shut the box!";
		}
		else {
			scoreText.text = score.ToString();
		}
	}

	public void InitilizeScore() {
		score = 123456789;
		scoreText.text = score.ToString();
	}

	public void ResetScore() {
		score = 123456789;
		scoreText.text = "";
	}

	public void ResetAIScore() {
		playLogic = FindObjectOfType<AIPlayLogic>();
		score = 123456789;
		scoreText.text = "";
		playing = true;
		playingRound = false;
	}

	public string GetName() {
		return playerName;
	}

	public int GetScore() {
		return score;
	}

	public List<int> GetBoard() {
		string strScore = score.ToString();
		List<int> b = new List<int>();
		for (int i = 0; i < strScore.Length; i++) {
			b.Add((int)char.GetNumericValue(strScore[i]));
		}
		return b;
	}

	public int GetIndex() {
		string pName = this.gameObject.name;
		char ch = pName[7];
		double num = char.GetNumericValue(ch);
		return (int)num - 1; //0-5 instead of 1-6
	}

	public void DoTurn() {
		if (playing && !playingRound) {
			InitilizeScore();
			playingRound = true;
			playLogic.StartCoroutine(playLogic.PlayRound(GetBoard(), index));
		}

	}

	public void DonePlaying() {
		playing = false;
	}

	public bool GetPlaying() {
		return playing;
	}

}
