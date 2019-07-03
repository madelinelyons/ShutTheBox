using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class AIPlayLogic : MonoBehaviour {
	GameController controller;
	Dictionary<int, List<List<int>>> lut;
	AIGameLogic aiFlow;

	// Use this for initialization
	void Start () {
		controller = FindObjectOfType<GameController>();
		aiFlow = FindObjectOfType<AIGameLogic>();
		lut = controller.MakeDict();
	}

	public IEnumerator PlayRound(List<int> board, int playerIndex) {
		while (aiFlow.StillPlaying(playerIndex)) {
			yield return new WaitForSeconds(.4f);
			int roll = Roll(board);
			bool check = controller.MovesAvailable(roll, board);

			if (!check) {
				aiFlow.AIDone(playerIndex);
			}
			else {
				MakeYourChoice(roll, board);
			}
			aiFlow.UpdateScore(board, playerIndex);
		}
	}

	private int Roll(List<int> board) {
		int roll;
		if (board.Sum() > 6) {
			roll = Random.Range(1, 6) + Random.Range(1, 6);
		}
		else {
			roll = Random.Range(1, 6);
		}
		return roll;
	}

	void MakeYourChoice(int roll, List<int> board) {
		List<List<int>> combos = lut[roll];
		List<int> set1 = null;

		List<int> preserve = FindPrecariousNums(board);

		for (int i = 0; i < combos.Count; i++){
			if(combos[i].Intersect(preserve).Any() == false) {
				if (!combos[i].Except(board).Any()){
					set1 = combos[i];
					break;
				}
			}
		}

		if(set1 == null) {
			for(int i = 0; i < combos.Count; i++) {
				if (!combos[i].Except(board).Any()) {
					set1 = combos[i];
					break;
				}
			}
		}

		for (int j = 0; j < set1.Count; j++) {
			board.Remove(set1[j]);
		}

	}

	List<int> FindPrecariousNums(List<int> board) {
		List<int> preserve = new List<int>();
		List<int> combo = new List<int>();
		for (int k=2; k<=12; k++) {
			int numCombos = 0;
			for (int v = 0; v < lut[k].Count; v++) {
				if (!lut[k][v].Except(board).Any()) {
					numCombos += 1;
					combo = lut[k][v];
				}
				if (numCombos > 1) {
					break;
				}
			}
			if (numCombos == 1) {
				for (int nums = 0; nums < combo.Count; nums++) {
					preserve.Add(combo[nums]);
				}
			}
		}

		preserve = preserve.Distinct().ToList();
		return preserve;
	}

}
