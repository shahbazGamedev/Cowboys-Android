using UnityEngine;
using System.Collections;

public class ButtonSwitchBetweenScreens : MonoBehaviour {

	public GameObject currentScreen;
	public GameObject nextScreen;
	
	void Start () {
		
	}
	
	void OnClick () {
		currentScreen.SetActive(false);
		nextScreen.SetActive(true);
	}
}
