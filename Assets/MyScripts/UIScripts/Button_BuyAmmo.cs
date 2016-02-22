using UnityEngine;
using System.Collections;

public class Button_BuyAmmo : MonoBehaviour {

    public int WeaponIndex;
    public int currentAmmo;

    public int price;
    public int ammoAmountToBuy = 1;
    

    public UILabel priceLabel;
    public UILabel ammoLabel;

	void Start () {
        currentAmmo = StoreItemsManager.Instance.weaponsAmmos[WeaponIndex];

        UpdateAmmoLabel();

	}


    void UpdateAmmoLabel()
    {
        ammoLabel.text = currentAmmo.ToString();
    }
	
	void OnClick () {
        
        if (GoldManager.Instance.CheckGold(price))
        {
            currentAmmo = StoreItemsManager.Instance.AddAmmo(WeaponIndex, ammoAmountToBuy);
            GoldManager.Instance.UpdateGold(-1 * price);

            UpdateAmmoLabel();
        }

      
	}
}
