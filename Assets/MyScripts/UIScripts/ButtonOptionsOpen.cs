using UnityEngine;
using System.Collections;

public class ButtonOptionsOpen : MonoBehaviour {

	public TweenPosition cameraTweener;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void OnClick () {
		cameraTweener.Toggle();
	}
}
