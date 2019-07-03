using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class AIGameLogic : MonoBehaviour{
	[SerializeField] Player[] players = new Player[6];
	TextMeshProUGUI[] playerNames = new TextMeshProUGUI[6];
	Color[] playerColors = new Color[6];
	List<string> AINameList;
	List<Color> colorList;
	PlayerSingleton singleton;
	GameController controller;
	int numPlayers;
	int currentPlayer = 0; //player
	bool displayInProgress = false;

	[SerializeField] TextMeshProUGUI congratsOrTooBad;
	[SerializeField] TextMeshProUGUI humanOrRobot;

	[SerializeField] GameObject changePlayersOnWin;
	[SerializeField] GameObject changePlayersOnRoundEnd;

	[SerializeField] TMP_InputField newNumPlayersWin;
	[SerializeField] TMP_InputField newNumPlayersRoundEnd;

	[SerializeField] GameObject AIGameCanvas;
	Stats stats;

	void Start(){
		singleton = FindObjectOfType<PlayerSingleton>();
		controller = FindObjectOfType<GameController>();
		stats = FindObjectOfType<Stats>();
		numPlayers = singleton.GetNumPlayers() + 1;
		AINameList = new List<string>(){"Carlton", "Drake", "Benedict", "Martin", "Garrison", "Kyle", "Diesel", "James",  "Kim",  "Kerry", "Ace", "Gustavo", "Tex",
										"Allison", "Susie", "Abby", "Betty Jo", "Mafalda", "Caroline", "Blaire", "Sylvia", "Lady Luck", "Bailey", "Maeve", "Penelope", 
										"Jimothy", "Bobert", "DanTheMan", "Bae", "Willaby", "Daisy Duke", "Robot Joe", "Pachinko", "Aleks", "Broccolo", "Jambalaya",
										"Astrid", "Barold", "Sebastian", "Esmeralda", "Garrett", "Jasper", "Emmett", "Rosaline", "Peggy", "Rod", "Jacques", "Maple"};

		colorList = new List<Color>() { new Color32(145, 22, 22, 255), new Color32(255, 0, 0, 255), new Color32(33, 152, 24, 255), new Color32(255, 225, 25, 255),
			new Color32(245, 130, 48, 255), new Color32(137, 48, 154, 255),  new Color32(51, 255, 255, 255),  new Color32(217, 118, 211, 255),  new Color32(128, 255, 0, 255),
			new Color32(0, 128, 128, 255),  new Color32(255, 98, 133, 255), new Color32(144, 255, 191, 255),  new Color32(106, 128, 0, 255),  new Color32(0, 0, 128, 255)};

		SetUpAIplayerMenu(numPlayers);

		changePlayersOnWin.gameObject.SetActive(false);
		changePlayersOnRoundEnd.gameObject.SetActive(false);

		newNumPlayersWin.characterLimit = 1;
		newNumPlayersWin.keyboardType = TouchScreenKeyboardType.NumberPad;
		newNumPlayersWin.contentType = TMP_InputField.ContentType.IntegerNumber;

		newNumPlayersRoundEnd.characterLimit = 1;
		newNumPlayersRoundEnd.keyboardType = TouchScreenKeyboardType.NumberPad;
		newNumPlayersRoundEnd.contentType = TMP_InputField.ContentType.IntegerNumber;

	}

	private void Update() {
		if (currentPlayer != 0) {
			RunAIsConcurrently();
		}

		if (AllAIsDone() && !displayInProgress) {
			displayInProgress = true;
			if (PlayerWon() == 2) {
				congratsOrTooBad.text = "Congrats!";
				humanOrRobot.text = "you beat all the ai!";
				stats.IncrementAiWins();
			}
			else if(PlayerWon() == 1) {
				congratsOrTooBad.text = "So Close!";
				humanOrRobot.text = "you tied with a robot";
			}
			else {
				congratsOrTooBad.text = "Too Bad!";
				humanOrRobot.text = "a robot won this round";
			}

			controller.StartCoroutine(controller.DisplayLoseScreen());
			controller.StartCoroutine(controller.MakeDiceInvisible());
		}
	}

	public void SetUpAIplayerMenu(int numP) {
		ShuffleList(AINameList);
		ShuffleList(colorList);
		MakeNameList(numP);
	}

	private void MakeNameList(int numP) {
		string playerName = stats.getName();
		for (int i = 0; i < players.Length; i++) {
			if (AINameList[i] != playerName) {
				players[i].SetName(AINameList[i]);
			}
			else {
				players[i].SetName(AINameList[players.Length + 1]);
			}
			playerColors[i] = colorList[i];
			if (i < numP) {
				this.players[i].gameObject.SetActive(true);
			}
			else {
				this.players[i].gameObject.SetActive(false);
			}
		}

		players[0].SetName(playerName);
		playerColors[0] = new Color32(25, 40, 214, 255);

		for (int i = 0; i < players.Length; i++) {
			playerNames[i] = players[i].GetComponent<TextMeshProUGUI>();
			playerNames[i].text = players[i].GetName();
			playerNames[i].color = playerColors[i];
		}
	}

	void ShuffleList<T>(List<T> list) {
		int n = list.Count;
		while (n > 1) {
			n--;
			int k = Random.Range(0, n + 1);
			var value = list[k];
			list[k] = list[n];
			list[n] = value;
		}
	}

	public IEnumerator PlayerDone() {
		players[currentPlayer].SetScore(controller.GetScore());
		yield return new WaitForSeconds(.25f);
		currentPlayer += 1;
	}

	public void WinDisplayChangePlayer() {
		changePlayersOnWin.gameObject.SetActive(true);
	}

	public void RoundEndDisplayChangePlayer() {
		changePlayersOnRoundEnd.gameObject.SetActive(true);
	}

	public void RunAIsConcurrently() {
		for (int i = 1; i < numPlayers; i++) {
			players[i].DoTurn();
		}
	}

	public void UpdateScore(List<int> board, int playerID) {
		string boardScore = string.Join("", board);
		int score;
		if (boardScore == "") {
			score = 0;
		}
		else {
			score = int.Parse(boardScore);
		}
		players[playerID].SetScore(score);
	}

	public void PlayerStB() {
		players[0].SetScore(0);
	}

	public bool AllAIsDone() {
		bool done = true;
		for (int i = 1; i < numPlayers; i++) {
			if (players[i].GetPlaying()) {
				done = false;
			}
		}
		return done;
	}
	public bool StillPlaying(int index) {
		return players[index].GetPlaying();
	}

	public void AIDone(int index) {
		players[index].DonePlaying();
	}

	public int PlayerWon() {
		int playerScore = players[0].GetScore();
		int playerWon = 2; //0- loss, 1- tie, 2- win
		int lowestScore = 123456789;

		for (int i = 1; i < numPlayers; i++) {
			if (players[i].GetScore() <= playerScore) {
				if (players[i].GetScore() < lowestScore) {
					lowestScore = players[i].GetScore();
				}
				playerWon = 0;
			}
		}

		if(lowestScore == playerScore) {
			playerWon = 1;
		}

		return playerWon;
	}

	public void ConfirmButtonWin() {
		int uInput;
		bool success = int.TryParse(newNumPlayersWin.text, out uInput);
		if (success && uInput >= 1 && uInput <= 5) {
			changePlayersOnWin.gameObject.SetActive(false);
			SetNumPlayers(uInput + 1);
			newNumPlayersWin.text = "";
			aiReset();
		}
	}

	public void ConfirmButtonRoundEnd() {
		int uInput;
		bool success = int.TryParse(newNumPlayersRoundEnd.text, out uInput);
		if (success && uInput >= 1 && uInput <= 5) {
			changePlayersOnRoundEnd.gameObject.SetActive(false);
			SetNumPlayers(uInput + 1);
			newNumPlayersRoundEnd.text = "";
			aiReset();
		}
	}

	public void aiReset() {
		for (int i = 0; i < players.Length; i++) {
			players[i].ResetAIScore();
		}
		SetUpAIplayerMenu(numPlayers);
		currentPlayer = 0;
		displayInProgress = false;
		controller.Reset();
	}

	public void SetNumPlayers(int num) {
		numPlayers = num;
	}

	public void TurnOffGameCanvas() {
		AIGameCanvas.gameObject.SetActive(false);
	}
}
