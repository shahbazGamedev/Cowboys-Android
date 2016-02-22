using UnityEngine;
using System.Collections;

public class testtoo : MonoBehaviour {

    public Transform t;
    public Camera uiCam;
    public Camera mainCam;

	// Use this for initialization
	void Start () {
       //Vector3 v1 = uiCam.WorldToScreenPoint(t.position);
       //Debug.Log("v1 "+v1);

       //Vector3 v2 = mainCam.WorldToScreenPoint(t.position);
       //Debug.Log("v2 " + v2);

       // Vector3 v1 = mainCam.WorldToScreenPoint(t.position);
       //Debug.Log("v1 " + v1);

       //Vector3 v2 = uiCam.ScreenToWorldPoint(v1);
       //Debug.Log("v2 " + v2);

        Vector3 v1 = mainCam.WorldToScreenPoint(transform.position);
       Debug.Log("v1 "+v1);

       Vector3 v2 = uiCam.WorldToScreenPoint(t.position);
       Debug.Log("v2 " + v2);

	}
	
	// Update is called once per frame
	void Update () {
	    
	}
}
