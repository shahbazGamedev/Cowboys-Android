using UnityEngine;
using System.Collections;

public class ButtonMusicSwitch : MonoBehaviour {

	public GameObject onSprite;
	public GameObject offSprite;
	public GameObject banSprite;
	
	void Start () {
		
	}
	
	void OnClick () {
		if(onSprite.activeSelf){
			onSprite.SetActive(false);
			offSprite.SetActive(true);
			banSprite.SetActive(true);
		}
		else{
			onSprite.SetActive(true);
			offSprite.SetActive(false);
			banSprite.SetActive(false);
		}
	}
}
