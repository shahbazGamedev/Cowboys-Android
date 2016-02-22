using UnityEngine;
using System.Collections;

public class ReviveCounter : MonoBehaviour {

    private UILabel counterLabel;
    int counter;

    public UILabel revivePriceLabel;
    public UILabel reviveNumberLabel;

    public UIButton reviveButton;

    private int revNum;

	// Use this for initialization
	void Start () {
        counterLabel = GetComponent<UILabel>();
	}
	
    public void PlayCounter()
    {
        revNum =  PlayerPrefs.GetInt("revive", 5);

        reviveNumberLabel.text = revNum.ToString();

        if(revNum <= 0){
            revivePriceLabel.transform.parent.gameObject.SetActive(true);
            revivePriceLabel.text = DataHelper.REVIVE_INGAME_PRICE.ToString();
            if (DataHelper.REVIVE_INGAME_PRICE > GameMaster.Instance.OwnedCoins)
            {
                reviveButton.isEnabled = false;
            }
            else
            {
                reviveButton.isEnabled = true;
            }
        }
        else
        {
            revivePriceLabel.transform.parent.gameObject.SetActive(false);
            reviveButton.isEnabled = true;
        }

        

        counter = DataHelper.REVIVE_COUNTER;
        counterLabel.text = counter.ToString();

        GetComponent<TweenScale>().enabled = true;

        if (!IsInvoking("CountDown"))
            InvokeRepeating("CountDown", 0, 0.1f);
       

	}

    void CountDown()
    {
        counter--;
        counterLabel.text = counter.ToString();
        if (counter <= 0)
        {
            StopCounting();
        }
    }

    public void StopCounting()
    {
        Debug.Log("fff");
        CancelInvoke("CountDown");
        GetComponent<TweenScale>().ResetToBeginning();
        GetComponent<TweenScale>().enabled = false;

        GameMaster.Instance.ResumeLose();

    }

    public void ResetCounting()
    {
        //revive
        if(revNum > 0){
            revNum --;
            PlayerPrefs.SetInt("revive", revNum);
        }
        else
        {
            GameMaster.Instance.AdjustGold(-1*DataHelper.REVIVE_INGAME_PRICE);
        }

        CancelInvoke("CountDown");
        GetComponent<TweenScale>().ResetToBeginning();
        GetComponent<TweenScale>().enabled = false;

        GameMaster.Instance.Revive();
    }
}
