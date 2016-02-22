using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D other){
		Debug.Log("balala");
		if(other.gameObject.tag == "Player"){
			//Time.timeScale = 0;
            other.gameObject.GetComponent<DynamicPlayerController>().Die();
			//GameMaster.Instance.Lose();
		}
		gameObject.GetComponent<Collider2D>().enabled = false;
	}
}
