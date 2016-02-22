using UnityEngine;
using System.Collections;

public class PopUpsManager : MonoBehaviour {

    public static PopUpsManager Instance;

    public UIPanel ConfirmUpgradeWeaponPanel;
    public UIPanel ConfirmBuyAmmoPanel;
    public UIPanel ConfirmUnlockWeaponPanel;
    public UIPanel ConfirmBuyRevivePanel;

    public UIPanel NotEnoughMoneyPanel;


    private StoreItem currentSelectedTarget;
    private Transform currentTargetAmmoButton;

    private ReviveItem currentSelectedRevive;

	void Awake () {
        Instance = this;
	}


    public void AcivateConfirmUpgradeWeaponPanel(StoreItem targetItem)
    {
        ConfirmUpgradeWeaponPanel.GetComponent<TweenAlpha>().Toggle();

        currentSelectedTarget = targetItem;

    }

    public void ConfirmUpgrade()
    {
        currentSelectedTarget.DoUpgradeWeapon();
        ConfirmUpgradeWeaponPanel.GetComponent<TweenAlpha>().Toggle();
    }

    public void CancelUpgrade()
    {
        currentSelectedTarget = null;
        ConfirmUpgradeWeaponPanel.GetComponent<TweenAlpha>().Toggle();

    }

    public void AcivateConfirmBuyAmmoPanel(StoreItem targetItem, Transform ammoButton)
    {
        ConfirmBuyAmmoPanel.GetComponent<TweenAlpha>().Toggle();

        currentSelectedTarget = targetItem;
        currentTargetAmmoButton = ammoButton;

    }

    public void ConfirmBuyAmmo()
    {
       // Debug.Log("blalaa?? "+currentTargetAmmoButton.name[currentTargetAmmoButton.name.Length - 1].ToString());
        currentSelectedTarget.SendMessage("DoAddAmmo_" + currentTargetAmmoButton.name[currentTargetAmmoButton.name.Length-1].ToString());
        ConfirmBuyAmmoPanel.GetComponent<TweenAlpha>().Toggle();
    }

    public void CancelBuyAmmo()
    {
        currentSelectedTarget = null;
        currentTargetAmmoButton = null;
        ConfirmBuyAmmoPanel.GetComponent<TweenAlpha>().Toggle();
        
    }

    public void AcivateConfirmUnlockWeaponPanel(StoreItem targetItem)
    {
        ConfirmUnlockWeaponPanel.GetComponent<TweenAlpha>().Toggle();

        currentSelectedTarget = targetItem;

    }

    public void ConfirmUnlock()
    {
        currentSelectedTarget.DoUnlockWeapon();
        ConfirmUnlockWeaponPanel.GetComponent<TweenAlpha>().Toggle();
    }

    public void CancelUnlock()
    {
        currentSelectedTarget = null;
        ConfirmUnlockWeaponPanel.GetComponent<TweenAlpha>().Toggle();

    }

    public void ActivateNotEnoughMoneyPanel()
    {
        NotEnoughMoneyPanel.GetComponent<TweenAlpha>().Toggle();
    }

    public void AcivateConfirmBuyRevivePanel(ReviveItem targetItem, Transform reviveButton)
    {
        ConfirmBuyRevivePanel.GetComponent<TweenAlpha>().Toggle();

        currentSelectedRevive = targetItem;
        currentTargetAmmoButton = reviveButton;

    }

    public void ConfirmBuyRevive()
    {
        // Debug.Log("blalaa?? "+currentTargetAmmoButton.name[currentTargetAmmoButton.name.Length - 1].ToString());
        currentSelectedRevive.SendMessage("DoAddRevive");
        ConfirmBuyRevivePanel.GetComponent<TweenAlpha>().Toggle();
    }

    public void CancelBuyRevive()
    {
        currentSelectedRevive = null;
        currentTargetAmmoButton = null;
        ConfirmBuyRevivePanel.GetComponent<TweenAlpha>().Toggle();

    }

}
