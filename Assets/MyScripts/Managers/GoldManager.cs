using UnityEngine;
using System.Collections;

public class GoldManager : MonoBehaviour {

	public static GoldManager Instance;

	public int gold;
    public int revives;

	public UILabel goldLabel;
    public UILabel reviveLabel;

	void Awake () {
		Instance = this;
	}

	void Start(){
		gold = PlayerPrefs.GetInt ("gold", 50000);
		UpdateGold (0);

        revives = PlayerPrefs.GetInt("revive", 5);
        UpdateRevive(0);
	}
	
    public bool CheckGold(int amount)
    {
        if (amount > gold)
        {
            PopUpsManager.Instance.ActivateNotEnoughMoneyPanel();
            return false;
        }
           

        return true;
    }

	public bool UpdateGold (int amount) {
		if (gold < Mathf.Abs(amount) )
			return false;
		gold += amount;

		if (gold < 0) {
			gold = 0;
		}
		PlayerPrefs.SetInt ("gold", gold);
		goldLabel.text = gold+"";

		return true;
	}

    public bool UpdateRevive(int amount)
    {
        if (revives < Mathf.Abs(amount))
            return false;
        revives += amount;

        if (revives < 0)
        {
            revives = 0;
        }
        PlayerPrefs.SetInt("revive", revives);
        reviveLabel.text = revives + "";

        return true;
    }


}
