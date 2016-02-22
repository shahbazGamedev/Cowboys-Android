using UnityEngine;
using System.Collections;

public class OpenLevel : MonoBehaviour {

	public string levelName;
	public UILabel LoadingLabel;

    public GameObject loadingBar;

	void Start () {
	
	}
	
	void OnClick () {
		Caching.CleanCache ();
		
		Time.timeScale = 1;
        if (Application.loadedLevel == 0 && loadingBar)
        {
            loadingBar.SetActive(true);
            loadingBar.GetComponent<LoadingBar>().levelName = levelName;
            return;
        }
			

        Application.LoadLevel(levelName);
	}
}
