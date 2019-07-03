using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Profile")]
[Serializable]
public class Profile : ScriptableObject{
	public string playerName;
	public int bestScore;
	public int worstScore;
	public int numTimesStb;
	public int mostConsecutiveTimesStb;
	public int currentStbStreak;

	public int mpWins;
	public int aiWins;

	public void setPlayerName(string newName) {
		playerName = newName;
	}

	public void setBestScore(int score) {
		bestScore = score;
	}

	public void setWorstScore(int score) {
		worstScore = score;
	}

	public void setStbWins(int numWins) {
		numTimesStb = numWins;
	}

	public void setLongestStbStreak(int longestStreak) {
		mostConsecutiveTimesStb = longestStreak;
	}

	public void setCurrentStreak(int currentStreak) {
		currentStbStreak = currentStreak;
	}

	public void incrementMpWins() {
		mpWins++;
	}

	public int getMpWins() {
		return mpWins;
	}

	public void incrementAiWins() {
		aiWins++;
	}
	public int getAiWins() {
		return aiWins;
	}

}
