using UnityEngine;
using System.Collections;

public class ButtonShop : MonoBehaviour {

	public GameObject ShopPanel;
	public GameObject Fan;

	public GameObject mainMenuPanel;

	public bool open = true;

	void Start () {
	
	}
	
	void OnClick () {

        OpenShop();

		
	}

    public void OpenShop()
    {

        ShopPanel.SetActive(open);
        //Fan.SetActive(!open);
        mainMenuPanel.SetActive(!open);

    }
}
