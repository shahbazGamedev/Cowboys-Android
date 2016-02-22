using UnityEngine;
using System.Collections;

public class StoreItem : MonoBehaviour {

    

    public int weaponIndex;
    //public Transform WeaponMainParent;

    public UISprite[] upgradeSprites;
    public UILabel upgradePriceLabel;

    public Transform upgradeButton;

    public int currentAmmo;

    public int ammo_1_price;
    public int ammo_1_AmountToBuy = 1;
    public UILabel ammo_1_priceLabel;

    public int ammo_2_price;
    public int ammo_2_AmountToBuy = 1;
    public UILabel ammo_2_priceLabel;


    public UILabel ammoLabel;

    int weaponLevel;

    public UIPlayTween TweenUnlocked;
    public UIPlayTween TweenLocked;

    private string unlockSpriteName;

   // public UIButton unlockButton;

    void Start()
    {

        unlockSpriteName = transform.GetChild(0).GetComponentInChildren<UISprite>().spriteName;

        TweenUnlocked = GetComponentsInChildren<UIPlayTween>()[1];

        if (weaponIndex == 1 || weaponIndex == 4) { 
            TweenLocked = GetComponentsInChildren<UIPlayTween>()[2];

            After();

        }

        weaponLevel = StoreItemsManager.Instance.weaponsLevels[weaponIndex];
        Debug.Log(weaponLevel+"");
        if (weaponLevel < 0)
        {
            ammoLabel.enabled = false;
            TweenUnlocked.enabled = false;
            transform.GetChild(0).GetComponentInChildren<UISprite>().spriteName = "lock_" + unlockSpriteName;

            if (weaponLevel == -2)
            {
                LockItem();
            }
            if (weaponLevel == -1)
            {
                UnlockAndDisable();
            }
           

            return;
        }

        

        UnlockAndEnable();
        if (name == "gun" || name == "bow")
        {

            After();

        }

       
    }

    private void LockItem()
    {
        Debug.Log("locking...");
        if (TweenLocked)
            TweenLocked.enabled = true;
        GetComponentInChildren<TweenPosition>().to = new Vector3(-130, 0, 0);

        if (TweenLocked)
        {
            TweenLocked.tweenTarget.transform.GetChild(0).GetComponent<UIButton>().isEnabled = false;
        }
    }

    private void UnlockAndDisable()
    {
       
        if (TweenLocked)
            TweenLocked.enabled = true;
        GetComponentInChildren<TweenPosition>().to = new Vector3(-130, 0, 0);
        if (TweenLocked)
            TweenLocked.tweenTarget.transform.GetChild(0).GetComponent<UIButton>().isEnabled = true;
    }

    private void UnlockAndEnable()
    {
        if(ammoLabel)
            ammoLabel.enabled = true;
        TweenUnlocked.enabled = true;


        if (TweenLocked)
        {

           // TweenLocked.enabled = false;
            
        }


        transform.GetChild(0).GetComponentInChildren<UISprite>().spriteName = unlockSpriteName;

        //has to be done 
        if (upgradeSprites.Length > 0)
            UpdateUpgradeSprites(weaponLevel);

        currentAmmo = StoreItemsManager.Instance.weaponsAmmos[weaponIndex];

        if (ammoLabel)
            UpdateAmmoLabel();

    }

    public void DoUpgradeWeapon()
    {

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

    private void UpdateUpgradeSprites(int level)
    {
        int i;
       // Debug.Log(level + "");
        for (i = 0; i < level; i++)
        {
            upgradeSprites[i].enabled = true;
        }

        for (i = level; i < upgradeSprites.Length; i++)
        {
            upgradeSprites[i].enabled = false;
        }
        if (level <= 3)
        {
            upgradePriceLabel.text = DataHelper.WeaponsPricesPerUpgrade[weaponIndex, level] + "";
        }
        else
        {
            upgradePriceLabel.text = "Upgraded";
            upgradeButton.GetComponent<BoxCollider>().enabled = false;
        }
    }

    void UpdateAmmoLabel()
    {
        ammoLabel.text = currentAmmo.ToString();
    }

    public void DoAddAmmo_1()
    {

        if (GoldManager.Instance.CheckGold(ammo_1_price))
        {
            currentAmmo = StoreItemsManager.Instance.AddAmmo(weaponIndex, ammo_1_AmountToBuy);
            GoldManager.Instance.UpdateGold(-1 * ammo_1_price);

            UpdateAmmoLabel();
        }


    }

    public void DoAddAmmo_2()
    {

        if (GoldManager.Instance.CheckGold(ammo_2_price))
        {
            currentAmmo = StoreItemsManager.Instance.AddAmmo(weaponIndex, ammo_2_AmountToBuy);
            GoldManager.Instance.UpdateGold(-1 * ammo_2_price);

            UpdateAmmoLabel();
        }


    }

    public void DoUnlockWeapon()
    {

        if (GoldManager.Instance.CheckGold(DataHelper.UNLOCK_PRICES[weaponIndex]))
        {
            weaponLevel = StoreItemsManager.Instance.UnlockWeapon(weaponIndex);

            GoldManager.Instance.UpdateGold(-1 * DataHelper.UNLOCK_PRICES[weaponIndex]);

            UnlockAndEnable();	
            UpdateAndPlayTweens();
        }

    }

    private void UpdateAndPlayTweens()
    {
        //its dependent on that its only gun and bow (which that both have 2popups not 1 like bottle)
        GetComponentInChildren<TweenPosition>().to = new Vector3(-275, 0, 0);

        TweenLocked.Play(false);
        TweenUnlocked.Play(true);
        GetComponentInChildren<TweenPosition>().PlayForward();

        DisableUnlockingPopup();
    }

    public void DisableUnlockingPopup()
    {
        Invoke("After", .4f);
        
    }

    void After()
    {
        TweenLocked.enabled = false;
    }
}
