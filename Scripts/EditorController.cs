using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class EditorController : MonoBehaviour {
	[SerializeField] Button pencilButton;
	[SerializeField] GameObject editMenu;
	[SerializeField] TMP_InputField newName;
	[SerializeField] TextMeshProUGUI placeholder;
	[SerializeField] GameController controller;

	[SerializeField] Player[] players = new Player[6];
	[SerializeField] MultiplayerGameLogic mpController;
	Player currentlySelected;
	int charLimit = 9;
	bool editModeActive = false;
	Stats stats;

	void Start(){
		currentlySelected = players[0];
		editMenu.gameObject.SetActive(false);
		newName.characterLimit = charLimit;
		stats = FindObjectOfType<Stats>(); 
    }

	public void TurnOnEditMode() {
		if (!controller.GetPaused()){
			placeholder.text = currentlySelected.GetName();
			editModeActive = true;
			controller.Pause();
			editMenu.gameObject.SetActive(true);
		}
	}

	public void TurnOffEditMode() {
		editModeActive = false;
		editMenu.gameObject.SetActive(false);
		controller.Resume();
	}

	public bool EditModeActive() {
		return editModeActive;
	}

	public void ConfirmNewName() {
		if (newName.text != "") {
			currentlySelected.SetName(newName.text);
			mpController.UpdateName(currentlySelected);
			if(currentlySelected == players[0]) {
				stats.updateName(currentlySelected.GetName());
			}
			newName.text = "";
			placeholder.text = currentlySelected.GetName();
			newName.MoveTextStart(true);
		}
	}

	public void SelectPlayer() {
		string name = EventSystem.current.currentSelectedGameObject.name;
		currentlySelected = GetPlayer(name);
		placeholder.text = currentlySelected.GetName();
	}

	Player GetPlayer(string pName) {
		int index = GetNumber(pName) - 1;
		return players[index];
	}

	int GetNumber(string name) {
		char ch = name[7];
		double num = char.GetNumericValue(ch);
		return (int)num;
	}

}
