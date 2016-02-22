using UnityEngine;
using System.Collections;

public class ButtonJump : MonoBehaviour {

	private GameObject Player;

	// Use this for initialization
	void Start () {
		Player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void OnClick () {
        //Player.GetComponent<DynamicPlayerController>().Jump();
	}
}
