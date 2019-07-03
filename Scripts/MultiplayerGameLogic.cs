using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MultiplayerGameLogic : MonoBehaviour{
	[SerializeField] Player[] players = new Player[6];
	TextMeshProUGUI[] playerNames = new TextMeshProUGUI[6];
	Color[] playerColors = { new Color32(25, 40, 214, 255), new Color32(214, 49, 47, 255), new Color32(231, 228, 82, 255), new Color32(33, 152, 24, 255), new Color32(217, 118, 211, 255), new Color32(232, 145, 46, 255) };

	[SerializeField] TextMeshProUGUI winPlayer;
	[SerializeField] TextMeshProUGUI roundWinPlayer;
	PlayerSingleton singleton;
	GameController controller;
	int turn;
	int currentPlayer;
	int lastCurrentPlayer;
	bool roundOver = false;
	[SerializeField] GameObject changePlayersOnWin;
	[SerializeField] GameObject changePlayersOnRoundEnd;

	[SerializeField] TMP_InputField newNumPlayersWin;
	[SerializeField] TMP_InputField newNumPlayersRoundEnd;

	[SerializeField] GameObject roundOverScreen;

	[SerializeField] GameObject mpGameCanvas;

	int numPlayers;
	Stats stats;

	// Start is called before the first frame update
	void Start() {
		singleton = FindObjectOfType<PlayerSingleton>();
		controller = FindObjectOfType<GameController>();
		stats = FindObjectOfType<Stats>();
		numPlayers = singleton.GetNumPlayers();
		controller.SetRollText(players[currentPlayer].GetName() + "'s turn!");

		SetUpMultiplayerMenu(numPlayers);

		for (int i = 0; i < players.Length; i++) {
			playerNames[i] = players[i].GetComponent<TextMeshProUGUI>();
			playerNames[i].text = players[i].GetName();
		}

		turn = 0;
		currentPlayer = 0;

		changePlayersOnWin.gameObject.SetActive(false);
		changePlayersOnRoundEnd.gameObject.SetActive(false);
		roundOverScreen.gameObject.SetActive(false);

		newNumPlayersWin.characterLimit = 1;
		newNumPlayersWin.keyboardType = TouchScreenKeyboardType.NumberPad;
		newNumPlayersWin.contentType = TMP_InputField.ContentType.IntegerNumber;

		newNumPlayersRoundEnd.characterLimit = 1;
		newNumPlayersRoundEnd.keyboardType = TouchScreenKeyboardType.NumberPad;
		newNumPlayersRoundEnd.contentType = TMP_InputField.ContentType.IntegerNumber;
	}

	void Update() {
		if (turn < numPlayers) {
			if (!controller.GetMovesAvailable()) {
				players[currentPlayer].SetScore(controller.GetScore());
				turn = turn + 1;
				lastCurrentPlayer = currentPlayer;
				if (currentPlayer < numPlayers - 1) {
					currentPlayer = currentPlayer + 1;
				}
				else {
					currentPlayer = 0;
				}
				controller.SayMovesAvailable();
			}
		}
		else {
			roundOver = true;
		}
	}

	public void mpRoundReset() {
		if (turn == numPlayers) { //not stb round
			turn = 0;
			currentPlayer = 0;
		}
		else {
			turn = 0;
		}
		if (currentPlayer >= numPlayers) {
			currentPlayer = 0;
		}

		roundOver = false;

		SetUpMultiplayerMenu(numPlayers);
		roundOverScreen.gameObject.SetActive(false);
		for (int i = 0; i < players.Length; i++) {
			players[i].ResetScore();
		}
		controller.Reset();
		controller.SetRollText(players[currentPlayer].GetName() + "'s turn!");
	}

	public void SetUpMultiplayerMenu(int numPlayers) {
		for (int i = 0; i < players.Length; i++) {
			if (i < numPlayers) {
				players[i].gameObject.SetActive(true);
			}
			else {
				players[i].gameObject.SetActive(false);
			}
		}
		players[0].SetName(stats.getName());
	}

	public void UpdateName(Player p) {
		int index = p.GetIndex();
		playerNames[index].text = p.GetName();
	}

	public void SetNumPlayers(int num) {
		numPlayers = num;
	}

	public string GetCurrentPlayerName() {
		return players[currentPlayer].GetName();
	}

	public int GetCurrentPlayerNum() {
		return currentPlayer;
	}

	public void WinDisplayChangePlayer() {
		changePlayersOnWin.gameObject.SetActive(true);
	}

	public void RoundEndDisplayChangePlayer() {
		changePlayersOnRoundEnd.gameObject.SetActive(true);
	}

	public bool RoundOver() {
		return roundOver;
	}

	public void SetUpRoundOverScreen() {
		roundOverScreen.gameObject.SetActive(true);
		int winner = DetermineWinner();
		if (winner == 6) {
			roundWinPlayer.color = new Color32(20, 20, 20, 255);
			roundWinPlayer.text = "Tied Players!";
		}
		else {
			roundWinPlayer.color = playerColors[winner];
			roundWinPlayer.text = players[winner].GetName() + "!";
		}
	}

	public void MPStB() {
		changePlayersOnWin.gameObject.SetActive(false);
		winPlayer.color = playerColors[lastCurrentPlayer];
		winPlayer.text = players[lastCurrentPlayer].GetName() + "!";
	}

	public void ConfirmButtonWin() {
		int uInput;
		bool success = int.TryParse(newNumPlayersWin.text, out uInput);
		if (success && uInput >= 2 && uInput <= 6) {
			if (uInput > numPlayers) {
				currentPlayer = lastCurrentPlayer + 1;
			}
			changePlayersOnWin.gameObject.SetActive(false);
			SetNumPlayers(uInput);
			newNumPlayersWin.text = "";
			mpRoundReset();
		}
	}

	public void ConfirmButtonRoundEnd() {
		int uInput;
		bool success = int.TryParse(newNumPlayersRoundEnd.text, out uInput);
		if (success && uInput >= 2 && uInput <= 6) {
			if (uInput > numPlayers) {
				currentPlayer = lastCurrentPlayer + 1;
			}
			changePlayersOnRoundEnd.gameObject.SetActive(false);
			SetNumPlayers(uInput);
			newNumPlayersRoundEnd.text = "";
			mpRoundReset();
		}
	}

	public void mpReset() {
		if (!RoundOver()) {
			controller.Reset();
			controller.SetRollText(players[currentPlayer].GetName() + "'s turn!");
		}
		else {
			controller.TurnOffLoseScreen();
			changePlayersOnRoundEnd.gameObject.SetActive(false);
			SetUpRoundOverScreen();
		}
	}

	public int DetermineWinner() {
		int min = 123456789;
		int pIndex = 0;
		int winners = 1;
		for (int i = 0; i < players.Length; i++) {
			if(players[i].GetScore() <= min) {
				if (players[i].GetScore() < min) {
					min = players[i].GetScore();
					pIndex = i;
					winners = 1;
				}
				else {
					winners++;
				}
			}
			
		}

		if (winners > 1) {
			pIndex = 6;
		}

		if(pIndex == 0) {
			stats.IncrementMpWins();
		}

		return pIndex;
	}

	public void TurnOffGameCanvas() {
		mpGameCanvas.gameObject.SetActive(false);
	}

}
