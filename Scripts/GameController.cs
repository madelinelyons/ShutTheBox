using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameController : MonoBehaviour{
	List<int> board = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 }; //1, 2, 3, 4, 5, 6, 7, 8, 9
	List<Tab> selectedTabs = new List<Tab>();
	List<int> selected = new List<int>();
	Dictionary<int, List<List<int>>> lut;
	bool rollTime = true;
	int roll = 0;
	float diceWidth;
	bool playing = true;
	bool paused = false;
	Renderer d1rend, d2rend;
	bool movesAvailable = true;
	MultiplayerGameLogic mp;
	AIGameLogic ap;

	[SerializeField] Dice[] dice = new Dice[2];
	[SerializeField] Board tabBoard;
	[SerializeField] Tab tabAccess;
	[SerializeField] Lid lid;

	[SerializeField] TextMeshProUGUI rollText;
	[SerializeField] TextMeshProUGUI scoreText;

	[SerializeField] GameObject WinScreen;
	[SerializeField] GameObject LoseScreen;

	[SerializeField] GameObject SPGameCanvas;

	Stats stats;

	// Start is called before the first frame update
	void Start(){
		lut = MakeDict();
		Renderer rend = dice[0].gameObject.GetComponent<Renderer>();
		diceWidth = rend.bounds.size.x * dice[0].gameObject.transform.localScale.x;
		rollText.text = "Roll the dice!";
		WinScreen.gameObject.SetActive(false);
		LoseScreen.gameObject.SetActive(false);
		d1rend = dice[0].gameObject.GetComponent<Renderer>();
		d2rend = dice[1].gameObject.GetComponent<Renderer>();
		mp = FindObjectOfType<MultiplayerGameLogic>();
		ap = FindObjectOfType<AIGameLogic>();
		stats = FindObjectOfType<Stats>();
	}

	private void Update() {
		if (!paused) {
			if (playing) {
				playing = GameLoop();
			}
		}
	}

	bool GameLoop() {
		if(board.Sum() == 0) {
			if (mp != null) {
				if (mp.GetCurrentPlayerNum() == 0) {
					stats.IncrementMpWins();
					stats.updateNumStB();
					stats.updateConsecutiveStreak(stats.getCurrentStreak() + 1);
				}
			}
			else {
				if(ap != null) {
					stats.IncrementAiWins();
				}
				stats.updateNumStB();
				stats.updateConsecutiveStreak(stats.getCurrentStreak() + 1);
			}
			
			movesAvailable = false;
			Win();
			return false;
		}

		roll = 0;
		int d1 = 0;
		int d2 = 0;

		if (board.Sum() > 6) {
			if (rollTime) {
				//roll 2 dice
				dice[0].Roll(diceWidth * 4);
				dice[1].Roll(diceWidth * -4);
			}

			d1 = dice[0].DetermineUpSide();
			d2 = dice[1].DetermineUpSide();
			roll = d1 + d2;
		}

		else {
			dice[1].gameObject.SetActive(false);
			if (rollTime) {
				//roll 1 dice
				dice[0].Roll(0);
				}
			d1 = dice[0].DetermineUpSide();
			d2 = 1;
			roll = d1;
		}

		if (d1 != 0 && d2 != 0) {
			TurnOnRollText();
			rollText.text = "You rolled: " + roll.ToString();

			bool check = MovesAvailable(roll, board);

			if (check) {
				if (selected.Sum() == roll) {
					for (int i = 0; i < selectedTabs.Count; i++) {
						selectedTabs[i].Flip();
						board.Remove(selectedTabs[i].GetNumber());
					}
					selected.Clear();
					selectedTabs.Clear();
					foreach (Dice d in dice) {
						d.TabControl();
						d.NotRolled();
					}
					SetRollTime(true);
					rollText.text = "Roll the dice!";
				}
				return true;
			}

			else {
				if(mp != null){
					if (mp.GetCurrentPlayerNum() == 0) {
						stats.scoreUpdateCheck(GetScore());
						stats.resetCurrentStreak();
					}
				}
				else {
					stats.scoreUpdateCheck(GetScore());
					stats.resetCurrentStreak();
				}
				
				movesAvailable = false;
				Lose();
				return false;
			}
		}
		return true;
	}

	private void Win() {
		rollText.gameObject.SetActive(false);
		lid.Close();
		StartCoroutine(DisplayWinScreen());
		if(ap!= null) {
			ap.PlayerStB();
		}
	}

	private void Lose() {
		rollText.gameObject.SetActive(false);
		if (mp != null) {
			scoreText.text = mp.GetCurrentPlayerName() + "'s score is: " + string.Join("", board);
		}
		else {
			scoreText.text = "Your score is: " + string.Join("", board);
		}

		if (ap != null) {
			ap.StartCoroutine(ap.PlayerDone());
		}

		else {
			StartCoroutine(DisplayLoseScreen());
			StartCoroutine(MakeDiceInvisible());
		}
	}

	public int GetScore() {
		string score = string.Join("", board);
		if (score == "") {
			return 0;
		}
		else {
			return int.Parse(score);
		}
	}

	IEnumerator DisplayWinScreen() {
		yield return new WaitForSeconds(lid.CloseAnimPlayTime() + tabAccess.FlipAnimationPlayTime());
		if (mp != null) {
			mp.MPStB();
		}
		WinScreen.gameObject.SetActive(true);
		
	}

	public IEnumerator DisplayLoseScreen() {
		yield return new WaitForSeconds(.75f);
		LoseScreen.gameObject.SetActive(true);
	}

	public void TurnOffLoseScreen() {
		LoseScreen.gameObject.SetActive(false);
	}

	public void TurnOffGameCanvas() {
		SPGameCanvas.gameObject.SetActive(false);
	}

	public IEnumerator MakeDiceInvisible() {
		yield return new WaitForSeconds(.75f);
		d1rend.enabled = false;
		d2rend.enabled = false;
	}

	public void MakeDiceInvisibleNow() {
		d1rend.enabled = false;
		d2rend.enabled = false;
	}

	IEnumerator WinTurnOnRollText() {
		yield return new WaitForSeconds(lid.OpenAnimPlayTime());
		TurnOnRollText();
	}

	public void Pause() {
		paused = true;
		Time.timeScale = 0;
	}

	public void Resume() {
		Time.timeScale = 1;
		paused = false;
	}

	public bool GetPaused() {
		return paused;
	}

	public bool MovesAvailable(int roll, List<int> board) {
		if (board.Count == 0){
			return false;
		}

		List<List<int>> combos = lut[roll];
		bool alive = true;

		int i = 0;
		while (alive) {
			List<int> a = combos[i];
			bool aInBoard = a.All(elem => board.Contains(elem));
			if (aInBoard) {
				return true;
			}
			else {
				i++;
				if (i == combos.Count) {
					break;
				}
			}
		}
		return false;
	}

	public bool GetMovesAvailable() {
		return movesAvailable;
	}

	public void SayMovesAvailable() {
		movesAvailable = true;
	}

	public void SetRollText(string newText) {
		rollText.text = newText;
	}

	public void Reset() {
		if (board.Sum() == 0) {
			StartCoroutine(WinTurnOnRollText());
			WinScreen.gameObject.SetActive(false);
			lid.Open();
		}
		else {
			LoseScreen.gameObject.SetActive(false);
			TurnOnRollText();
		}

		stats.TurnOffUpdatedStatsText();
		rollText.text = "Roll the dice!";

		board = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 }; //1, 2, 3, 4, 5, 6, 7, 8, 9
		tabBoard.ResetBoard();
		tabBoard.UnselectAll();
		selectedTabs.Clear();
		selected.Clear();
		if (!dice[1].gameObject.activeSelf) {
			dice[1].gameObject.SetActive(true);
		}
		d1rend.enabled = true;
		d2rend.enabled = true;
		foreach (Dice d in dice) {
			d.TabControl();
			d.NotRolled();
		}
		SetRollTime(true);
		playing = true;
		
	}

	public bool GetPlaying() {
		return playing;
	}

	public void AddSelected(Tab tab) {
		selected.Add(tab.GetNumber());
		selectedTabs.Add(tab);
	}

	public void RemoveSelected(Tab tab) {
		selected.Remove(tab.GetNumber());
		selectedTabs.Remove(tab);
	}

	public void SetRollTime(bool set) {
		rollTime = set;
	}

	public bool GetRollTime() {
		return rollTime;
	}

	public void TurnOnRollText() {
		rollText.gameObject.SetActive(true);
	}

	public void TurnOffRollText() {
		rollText.gameObject.SetActive(false);
	}

	public List<int> GetBoard() {
		return board;
	}

	public void PrintList(List<int> list) {
		string toRead = string.Join(", ", list);
		Debug.Log(name + ": " + toRead);
	}

	public Dictionary<int, List<List<int>>> MakeDict() {
		var lut = new Dictionary<int, List<List<int>>>();

		var sublist0 = new List<List<int>> {
			new List<int>() {0}
		};

		var sublist1 = new List<List<int>> {
			new List<int>() {1}
		};

		var sublist2 = new List<List<int>> {
			new List<int>() {2}
		};

		var sublist3 = new List<List<int>> {
			new List<int>() {3},
			new List<int>() {1,2}
		};

		var sublist4 = new List<List<int>> {
			new List<int>() {4},
			new List<int>() {1,3}
		};

		var sublist5 = new List<List<int>> {
			new List<int>() {5},
			new List<int>() {1,4},
			new List<int>() {2,3}
		};

		var sublist6 = new List<List<int>> {
			new List<int>() {6},
			new List<int>() {1,5},
			new List<int>() {2,4},
			new List<int>() {1,2,3}
		};

		var sublist7 = new List<List<int>> {
			new List<int>() {7},
			new List<int>() {1,6},
			new List<int>() {2,5},
			new List<int>() {3,4},
			new List<int>() {1,2,4}
		};

		var sublist8 = new List<List<int>> {
			new List<int>() {8},
			new List<int>() {1,7},
			new List<int>() {2,6},
			new List<int>() {3,5},
			new List<int>() {1,2,5},
			new List<int>() {1,3,4}
		};

		var sublist9 = new List<List<int>> {
			new List<int>() {9},
			new List<int>() {1,8},
			new List<int>() {2,7},
			new List<int>() {3,6},
			new List<int>() {4,5},
			new List<int>() {1,2,6},
			new List<int>() {1,3,5},
			new List<int>() {2,3,4}
		};

		var sublist10 = new List<List<int>> {
			new List<int>() {1,9},
			new List<int>() {2,8},
			new List<int>() {3,7},
			new List<int>() {4,6},
			new List<int>() {1,2,7},
			new List<int>() {1,3,6},
			new List<int>() {1,4,5},
			new List<int>() {2,3,5},
			new List<int>() {1,2,3,4}
		};

		var sublist11 = new List<List<int>> {
			new List<int>() {2,9},
			new List<int>() {3,8},
			new List<int>() {4,7},
			new List<int>() {5,6},
			new List<int>() {1,2,8},
			new List<int>() {1,3,7},
			new List<int>() {1,4,6},
			new List<int>() {2,3,6},
			new List<int>() {2,4,5},
			new List<int>() {1,2,3,5}
		};

		var sublist12 = new List<List<int>> {
			new List<int>() {3,9},
			new List<int>() {4,8},
			new List<int>() {5,7},
			new List<int>() {1,2,9},
			new List<int>() {1,3,8},
			new List<int>() {1,4,7},
			new List<int>() {1,5,6},
			new List<int>() {2,3,7},
			new List<int>() {2,4,6},
			new List<int>() {3,4,5},
			new List<int>() {1,2,3,6},
			new List<int>() {1,2,4,5}
		};

		lut.Add(0, sublist0);
		lut.Add(1, sublist1);
		lut.Add(2, sublist2);
		lut.Add(3, sublist3);
		lut.Add(4, sublist4);
		lut.Add(5, sublist5);
		lut.Add(6, sublist6);
		lut.Add(7, sublist7);
		lut.Add(8, sublist8);
		lut.Add(9, sublist9);
		lut.Add(10, sublist10);
		lut.Add(11, sublist11);
		lut.Add(12, sublist12);

		return lut;
	}
}
