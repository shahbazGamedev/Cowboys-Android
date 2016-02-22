using UnityEngine;
using System.Collections;

public class ReviveItem : MonoBehaviour {

    public int reviveIndex;

    [HideInInspector]public int currentRevives;

    public int revivePrice;
    public int reviveAmountToBuy = 1;
    public UILabel revivePriceLabel;

    void Start()
    {
       revivePrice = DataHelper.REVIVE_PRICES[reviveIndex];
       revivePriceLabel.text = revivePrice + "";

    }


    public void DoAddRevive()
    {

        if (GoldManager.Instance.CheckGold(revivePrice))
        {
            Debug.Log("elhabal dah?" + revivePrice);
            GoldManager.Instance.UpdateRevive(reviveAmountToBuy);
            GoldManager.Instance.UpdateGold(-1 * revivePrice);

           // UpdateAmmoLabel();
        }


    }

}
