using UnityEngine;
using System.Collections;

public class Button_SwitchWeapon : MonoBehaviour {

	private Weapons grandpaWeapons;
	public int weaponIndex;

    private UIButton button;
    private string normalWeaponSpriteName;

    public bool locked;
    public Transform unlockMsg;

	// Use this for initialization
	void Awake () {
		grandpaWeapons = GameObject.FindGameObjectWithTag("Player").GetComponent<Weapons>();

        button = GetComponent<UIButton>();
        normalWeaponSpriteName = button.tweenTarget.GetComponent<UISprite>().spriteName;

        //bottle (the default selected)
        if (weaponIndex == DataHelper.DEFAULT_SELECTED_WEAPON_INDEX)
        {
            HighlightButton();
        }
	}

    private void HighlightButton()
    {
        button.normalSprite = "hl_"+normalWeaponSpriteName;
    }

    public void UnhighlightButton()
    {
        button.normalSprite = normalWeaponSpriteName;

    }

	// Update is called once per frame
	void OnClick () {

        //
        if (locked)
        {
            unlockMsg.GetComponent<TweenAlpha>().Toggle();
            return;
        }

        if (weaponIndex == DataHelper.ELECTRIC_BOMB_INDEX)
        {
            if(!IsInvoking("endReloadingElectric")){
                grandpaWeapons.FireElecticBomb();
                Invoke("endReloadingElectric", Weapons_PUs_Manager.Instance.ReshootDelayes[weaponIndex]);
            }
        }
        else if (weaponIndex == DataHelper.SHIELD_INDEX)
        {
            if (!IsInvoking("endReloadingShield"))
            {
                grandpaWeapons.FireShiled();
                Invoke("endReloadingShield", Weapons_PUs_Manager.Instance.ReshootDelayes[weaponIndex]);
            }
        }
        else
        {
            SwitchWeapon();
        }
	}

    public void SwitchWeapon()
    {
       // Debug.Log(grandpaWeapons);

       // grandpaWeapons.currentWeapon = weaponIndex;
        grandpaWeapons.switchWeapons(this, weaponIndex);
        HighlightButton();
    }

    void endReloadingElectric()
    {
        CancelInvoke("endReloadingElectric");
    }

    void endReloadingShield()
    {
        CancelInvoke("endReloadingShield");
    }

}
