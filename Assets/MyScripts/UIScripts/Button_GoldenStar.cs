using UnityEngine;
using System.Collections;

public class Button_GoldenStar : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Destroy(gameObject, 2f);
	}
	
	// Update is called once per frame
	void OnClick () {
		GameMaster.Instance.TakeGoldenStar();
		gameObject.SetActive(false);
	}
}
