using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lid : MonoBehaviour {
	Animator anim;
	RuntimeAnimatorController animController;

	// Start is called before the first frame update
	void Start(){
		anim = GetComponent<Animator>();
		animController = anim.runtimeAnimatorController;
		anim.enabled = true;
	}

	public void Open() {
		anim.Play("Open");
	}

	public void Close() {
		anim.Play("Close");
	}

	public float OpenAnimPlayTime() {
		return animController.animationClips[0].length;
	}

	public float CloseAnimPlayTime() {
		return animController.animationClips[1].length;

	}
}
