using UnityEngine;
using System.Collections;

public class DeactivateAfter : MonoBehaviour {

	// Use this for initialization
	void OnEnable () {
        Invoke("Deactivate", 3);
	}
	
    void Deactivate()
    {
        gameObject.SetActive(false);
	}
}
