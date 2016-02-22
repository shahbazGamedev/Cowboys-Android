using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimalsManager : MonoBehaviour {

    public static AnimalsManager Instance;

    public List<Transform> RightCows;
    public List<Transform> LeftCows;

    //public Transform [] RightCows;
   // public Transform [] LeftCows;

    public UILabel animalsCounterLabel;

    void Awake () {
        Instance = this;
	}

    void Start()
    {
        animalsCounterLabel.text = (RightCows.Count + LeftCows.Count).ToString();
    }


    public Transform PickRightRandomCow()
    {
        int index = Random.Range(0, RightCows.Count - 1);

        int trials = 0;
        while (RightCows[index].GetComponent<CowAI>().reserved && trials <5)
        {
            index = Random.Range(0, RightCows.Count - 1);
            trials++;
        }

        if (trials >= 5)
        {
            return null;
        }

        RightCows[index].GetComponent<CowAI>().reserved = true;

        return RightCows[index];

    }

    public Transform PickLeftRandomCow()
    {
        int index = Random.Range(0, LeftCows.Count - 1);

        int trials = 0;
        while (LeftCows[index].GetComponent<CowAI>().reserved && trials < 5)
        {
            index = Random.Range(0, LeftCows.Count - 1);
            trials++;
        }

        if (trials >= 5)
        {
            return null;
        }

        LeftCows[index].GetComponent<CowAI>().reserved = true;

        return LeftCows[index];

    }

    public void TerminateRightCow(Transform cow)
    {
        RightCows.Remove(cow);
        if (RightCows.Count <= 0)
        {
            GameMaster.Instance.Lose();
        }
        animalsCounterLabel.text = (RightCows.Count + LeftCows.Count).ToString();

    }

    public void TerminateLeftCow(Transform cow)
    {
        LeftCows.Remove(cow);
        if (LeftCows.Count <= 0)
        {
            GameMaster.Instance.Lose();
        }
        animalsCounterLabel.text = (LeftCows.Count + RightCows.Count).ToString();

    }

}
