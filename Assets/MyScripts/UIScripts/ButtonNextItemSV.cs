using UnityEngine;
using System.Collections;

public class ButtonNextItemSV : MonoBehaviour {

	public bool next = true;

	public UICenterOnChild centerOnChildComp;
	public UITable table;

	//public UIScrollView sv;

	public Vector2 v;

	void Start () {
	
	}
	
	void OnClick () {
        GameObject tab = GameObject.FindGameObjectWithTag("TabOpened");
        centerOnChildComp = tab.GetComponent<UICenterOnChild>();
        table = tab.GetComponent<UITable>();

		if(next){
			int index = table.GetChildList().IndexOf(centerOnChildComp.centeredObject.transform)+1;
			if(index<table.GetChildList().Count){
				centerOnChildComp.CenterOn(table.GetChildList()[index]);
			}
		}
		else{
			int index = table.GetChildList().IndexOf(centerOnChildComp.centeredObject.transform)-1;
			if(index > -1){
				centerOnChildComp.CenterOn(table.GetChildList()[index]);
			}
		}

		//sv.customMovement = v;
	}
}
