using UnityEngine;
using System.Collections;

public class LoadingBar : MonoBehaviour {
	
	float amount  = .02f;
	
	AsyncOperation async ;
	
	private UIProgressBar progBar;

    public string levelName;


	void Start () {
		
		progBar = gameObject.GetComponent<UISlider> ();
        progBar.value = 0;
		
		Invoke ("LoadGameScene", .1f);
	}

	void LoadGameScene(){
		
		//Application.backgroundLoadingPriority = ThreadPriority.High;
		async = Application.LoadLevelAsync("Stage1_Level1");
		InvokeRepeating ("AdjustProgressBar", Time.deltaTime, amount);
	}
	
	void AdjustProgressBar(){
		//		Debug.Log (async.progress);
		if(!async.isDone){
			progBar.value = async.progress;
		}
		
	}
	
}
