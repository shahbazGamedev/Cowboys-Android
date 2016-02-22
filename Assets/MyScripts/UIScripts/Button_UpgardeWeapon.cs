using UnityEngine;
using System.Collections;

public class Button_UpgardeWeapon : MonoBehaviour {

	public int weaponIndex;

	public UISprite [] upgradeSprites;
	public UILabel upgradePriceLabel;

    int weaponLevel;

	void Start () {

        weaponLevel = StoreItemsManager.Instance.weaponsLevels[weaponIndex];
        Debug.Log(weaponIndex + "," + weaponLevel);
		//upgradePriceLabel.text = WeaponsData.WeaponsPricesPerUpgrade[weaponIndex, level]+"";

        UpdateUpgradeSprites(weaponLevel);
	}
	
	void UpdateWeaponUI () {
	
	}

	void OnClick(){
        if (weaponLevel >= 4)
            return;

        if (GoldManager.Instance.CheckGold(DataHelper.WeaponsPricesPerUpgrade[weaponIndex, weaponLevel]))
        {
            weaponLevel = StoreItemsManager.Instance.UpgradeWeapon(weaponIndex);
            GoldManager.Instance.UpdateGold(-1 * DataHelper.WeaponsPricesPerUpgrade[weaponIndex, weaponLevel - 1]);

            if (weaponLevel <= 4)
            {
                UpdateUpgradeSprites(weaponLevel);
            }
        }


	}

	private void UpdateUpgradeSprites(int level){
		int i;
		Debug.Log (level+"");
		for(i=0; i<level; i++ ){
			upgradeSprites[i].enabled = true;
		}

		for(i= level; i<upgradeSprites.Length; i++ ){
			upgradeSprites[i].enabled = false;
		}
		if (level <= 3) {
			upgradePriceLabel.text = DataHelper.WeaponsPricesPerUpgrade [weaponIndex, level] + "";
		}
		else{
			upgradePriceLabel.text = "Upgraded";
			GetComponent<UIButton>().isEnabled = false;
		}
	}
}
