using UnityEngine;
using System.Collections;

public class DeactivateAfter : MonoBehaviour {

    public bool isCoinReduced;

	// Use this for initialization
	void OnEnable () {
        Invoke("Deactivate", 3);
	}
	
    void Deactivate()
    {
        gameObject.SetActive(false);

        if (isCoinReduced)
        {
            GameMaster.Instance.coinsReducedPool.Add(gameObject);
        }
	}
}
