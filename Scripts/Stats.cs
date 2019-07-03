using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Stats : MonoBehaviour{
	SaveManager saveManager;
	public Profile profile;
	[SerializeField] TextMeshProUGUI bestOrWorst;
	[SerializeField] TextMeshProUGUI newStbStreak;

	string playerName;
	int bestScore;
	int worstScore;
	int numTimesStb;
	int mostConsecutiveTimesStb;
	int currentStbStreak;
	int mpWins;
	int aiWins;

	// Start is called before the first frame update
	void Awake(){
		saveManager = FindObjectOfType<SaveManager>();
		saveManager.LoadStats();
		
		playerName = profile.playerName;
		bestScore = profile.bestScore;
		worstScore = profile.worstScore;
		numTimesStb = profile.numTimesStb;
		mostConsecutiveTimesStb = profile.mostConsecutiveTimesStb;
		currentStbStreak = profile.currentStbStreak;
		mpWins = profile.mpWins;
		aiWins = profile.aiWins;
    }

	private void Start() {
		TurnOffUpdatedStatsText();
	}

	public void updateName(string newName) {
		playerName = newName;
		profile.setPlayerName(playerName);
		saveManager.SaveStats();
	}

	public string getName() {
		return playerName;
	}

	public void scoreUpdateCheck(int currentScore) {
		bool changed = false;
		if (currentScore > worstScore) {
			worstScore = currentScore;
			profile.setWorstScore(worstScore);
			bestOrWorst.gameObject.SetActive(true);
			bestOrWorst.color = new Color32(215, 20, 25, 255);
			bestOrWorst.text = "New Worst Score!";
			changed = true;
		}
		if (currentScore < bestScore) {
			bestScore = currentScore;
			profile.setBestScore(bestScore);
			bestOrWorst.gameObject.SetActive(true);
			bestOrWorst.color = new Color32(21, 221, 233, 255);
			bestOrWorst.text = "New Best Score!";
			changed = true;
		}

		if (changed) {
			saveManager.SaveStats();
		}
	}

	public int getBestScore() {
		return bestScore;
	}

	public int getWorstScore() {
		return worstScore;
	}

	public void updateNumStB() {
		bestScore = 0;
		profile.setBestScore(bestScore);
		numTimesStb++;
		profile.setStbWins(numTimesStb);
		saveManager.SaveStats();
	}

	public int getNumStBWins() {
		return numTimesStb;
	}

	public void updateConsecutiveStreak(int currentStreak) {
		currentStbStreak = currentStreak;
		profile.setCurrentStreak(currentStreak);
		if (currentStreak > mostConsecutiveTimesStb) {
			mostConsecutiveTimesStb = currentStreak;
			profile.setLongestStbStreak(mostConsecutiveTimesStb);
			if(mostConsecutiveTimesStb > 1) {
				newStbStreak.gameObject.SetActive(true);
				newStbStreak.text = "New Stb Streak: " + mostConsecutiveTimesStb.ToString() + "!";
			}
		}
		saveManager.SaveStats();
	}

	public int getConsecutiveStreak() {
		return mostConsecutiveTimesStb;
	}

	public int getCurrentStreak() {
		return currentStbStreak;
	}

	public void resetCurrentStreak() {
		currentStbStreak = 0;
		profile.setCurrentStreak(0);
		saveManager.SaveStats();
	}

	public void TurnOffUpdatedStatsText() {
		bestOrWorst.gameObject.SetActive(false);
		newStbStreak.gameObject.SetActive(false);
	}

	public void IncrementMpWins() {
		mpWins++;
		profile.incrementMpWins();
		saveManager.SaveStats();
	}

	public int getMpWins() {
		return mpWins;
	}

	public void IncrementAiWins() {
		aiWins++;
		profile.incrementAiWins();
		saveManager.SaveStats();
	}

	public int getAiWins() {
		return aiWins;
	}

}

