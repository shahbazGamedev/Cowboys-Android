using UnityEngine;
using System.Collections;

public class OpenCloseScreens : MonoBehaviour {

	public GameObject currentScreen;
	public GameObject nextScreen;

	public TabsManager tabsMangager;

	void Start () {
        if(!tabsMangager)
		    tabsMangager = gameObject.transform.GetComponentInParent<TabsManager>();
	}

	void OnClick(){

		OpenClose ();


		
	}


	public void OpenClose(){
		if(nextScreen){
			
			if(tabsMangager){
				tabsMangager.currentOpened.SetActive(false);
				tabsMangager.currentOpened = nextScreen;
			}
			else{
				currentScreen.SetActive(false);
			}
			
			nextScreen.SetActive(true);
		}
	}
	

}
