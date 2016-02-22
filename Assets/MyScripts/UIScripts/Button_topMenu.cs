using UnityEngine;
using System.Collections;

public class Button_topMenu : MonoBehaviour {

	private GameObject nonActiveSpriteGO;
	private GameObject activeSpriteGO;

	private UISprite nonActiveSpite;
	private UISprite activeSprite;

	//private TweenPosition activeTweener;

	public TabsManager tabsMangager;

    public Transform mainButton;
   // private bool notMe;

	void Start () {
		

		//activeTweener = activeSpriteGO.GetComponent<TweenPosition>();
        if (!tabsMangager)
        {
            tabsMangager = gameObject.transform.GetComponentInParent<TabsManager>();

            nonActiveSpriteGO = transform.GetChild(0).gameObject;
            activeSpriteGO = transform.GetChild(1).gameObject;

            nonActiveSpite = nonActiveSpriteGO.GetComponent<UISprite>();
            activeSprite = activeSpriteGO.GetComponent<UISprite>();
        }
        else
        {

            nonActiveSpriteGO = mainButton.GetChild(0).gameObject;
            activeSpriteGO = mainButton.GetChild(1).gameObject;

            nonActiveSpite = nonActiveSpriteGO.GetComponent<UISprite>();
            activeSprite = activeSpriteGO.GetComponent<UISprite>();

            
        }
        
	}
	
	void OnClick () {

        tabsMangager.currentPressed.GetComponent<Button_topMenu>().DeActivateButton();
        ActivateButton();

        if(mainButton){
           
            tabsMangager.currentPressed = mainButton.gameObject;
        }
        else
        {
            ActivateButton();
            tabsMangager.currentPressed = gameObject;
        }

	}

	public void ActivateButton(){
		nonActiveSpite.enabled = false;
		activeSprite.enabled = true;

		//activeTweener.Toggle();
	}

	public void DeActivateButton(){
		nonActiveSpite.enabled = true;
		activeSprite.enabled = false;
		
		//activeTweener.Toggle();
	}
}
