using UnityEngine;
using System.Collections;

public class ButtonResume : MonoBehaviour {

	public GameObject PauseScreen;
	
	void Start () {
		
	}
	
	// Update is called once per frame
	void OnClick () {
		
		PauseScreen.SetActive(false);

        Time.timeScale = 1;
	}
}
