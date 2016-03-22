using UnityEngine;
using System.Collections;

public class ButtonPause : MonoBehaviour {

	public GameObject PauseScreen;

	void Start () {
	
	}
	
	// Update is called once per frame
	void OnClick () {
		if(Time.timeScale == 1){
			Time.timeScale = 0;
			PauseScreen.SetActive(true);
            SoundManager.StopSFX();
		}
	}
}
