using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxScene : MonoBehaviour{
	Animator anim;

	// Start is called before the first frame update
	void Start(){
		anim = GetComponent<Animator>();
		anim.enabled = true;
	}

	public void ShiftRight() {
		anim.Play("ShiftOver");
	}

	public void ShiftLeft() {
		anim.Play("ShiftBack");
	}
}
