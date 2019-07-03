using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatsDisplay : MonoBehaviour{
	[SerializeField] GameObject NoStbStatsCanvas;
	[SerializeField] GameObject StbStatsCanvas;

	[SerializeField] TextMeshProUGUI noStbMpWins;
	[SerializeField] TextMeshProUGUI noStbAiWins;
	[SerializeField] TextMeshProUGUI noStbBestScore;
	[SerializeField] TextMeshProUGUI noStbWorstScore;

	[SerializeField] TextMeshProUGUI mpWins;
	[SerializeField] TextMeshProUGUI aiWins;
	[SerializeField] TextMeshProUGUI timesStb;
	[SerializeField] TextMeshProUGUI consecutiveStb;
	[SerializeField] TextMeshProUGUI worstScore;

	[SerializeField] Stats stats;
	[SerializeField] GameObject StbStreakItems;

	public void Start() {
		TurnOffStatCanvases();
		DisplayMpWins();
		DisplayAiWins();
		DisplayBestScore();
		DisplayWorstScore();
		DisplayTimesStB();
		DisplayConsecutiveStBWins();
	}

	public void TurnOffStatCanvases() {
		StbStatsCanvas.gameObject.SetActive(false);
		NoStbStatsCanvas.gameObject.SetActive(false);
	}

	public void TurnOnStatCanvas() {
		if (stats.getBestScore() == 0) {
			StbStatsCanvas.gameObject.SetActive(true);
		}
		else {
			NoStbStatsCanvas.gameObject.SetActive(true);
		}
	}

	public void DisplayMpWins() {
		int mp = stats.getMpWins();
		/*
		if (mp == 0) {
			noStbMpWins.text = "";
			mpWins.text = "";
		}
		else {
		*/
			noStbMpWins.text = mp.ToString();
			mpWins.text = mp.ToString();
		
	}

	public void DisplayAiWins() {
		int ai = stats.getAiWins();
		/*
		if (ai == 0) {
			noStbAiWins.text = "";
			aiWins.text = "";
		}
		else {
		*/
			noStbAiWins.text = ai.ToString();
			aiWins.text = ai.ToString();
		
	}

	public void DisplayBestScore() {
		int best = stats.getBestScore();
		if (best == 123456789) {
			noStbBestScore.text = "-----";
		}
		else {
			noStbBestScore.text = best.ToString();
		}
	}

	public void DisplayWorstScore() {
		int worst = stats.getWorstScore();
		if (stats.getBestScore() == 0) {
			if (worst == 0) {
				worstScore.text = "-----";
			}
			else {
				worstScore.text = worst.ToString();
			}
		}
		else {
			if (worst == 0) {
				noStbWorstScore.text = "-----";
			}
			else {
				noStbWorstScore.text = worst.ToString();
			}
		}
	}

	public void DisplayTimesStB() {
		int StBWins = stats.getNumStBWins();
		timesStb.text = StBWins.ToString();
	}

	public void DisplayConsecutiveStBWins() {
		int consecWins = stats.getConsecutiveStreak();
		if (consecWins > 1) {
			StbStreakItems.gameObject.SetActive(true);
			consecutiveStb.text = consecWins.ToString();
		}
		else {
			StbStreakItems.gameObject.SetActive(false);
		}
	}

}
