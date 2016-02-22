using UnityEngine;
using System.Collections;

public class StoreItemsManager : MonoBehaviour {

	public static StoreItemsManager Instance;


	public int [] weaponsLevels;
    public int[] weaponsAmmos;

    private int ShopOpenFlag;

    public ButtonShop OpenShopButtonScript;

	void Awake(){
		Instance = this;
		//PlayerPrefs.DeleteAll();

        InitWeaponsData();

       
	}

    void Start()
    {
        ShopOpenFlag = PlayerPrefs.GetInt("openShop", 0);
        if (ShopOpenFlag == 1)
        {
            OpenShopButtonScript.OpenShop();
            PlayerPrefs.SetInt("openShop", 0);
        }
    }

    void InitWeaponsData()
    {
		weaponsLevels = new int[DataHelper.WEAPONS_NUMBER];
        weaponsAmmos = new int[DataHelper.WEAPONS_NUMBER];

        /*testt
        for (int i = 0; i < WeaponsData.WEAPONS_NUMBER; i++)
        {
            weaponsLevels[i] = PlayerPrefs.GetInt(WeaponsData.WEAPONS_NAMES[i] + "_level", 0);
            weaponsAmmos[i] = PlayerPrefs.GetInt(WeaponsData.WEAPONS_NAMES[i] + "_bullets", 0);
        }
        */
        int currentStage = PlayerPrefs.GetInt("currentStage", 1);
        

        for (int i = 0; i < DataHelper.WEAPONS_NUMBER; i++)
        {

            if (currentStage >= DataHelper.ULOCKING_LEVEL[i])
            {
                if (DataHelper.DEFAULT_OWNED[i] == 1)
                {
                    weaponsLevels[i] = PlayerPrefs.GetInt(DataHelper.WEAPONS_NAMES[i] + "_level", 0);
                }
                else
                {
                    weaponsLevels[i] = PlayerPrefs.GetInt(DataHelper.WEAPONS_NAMES[i] + "_level", -1);
                }
            }
            else
            {
                //temppppp
                //weaponsLevels[i] = PlayerPrefs.GetInt(DataHelper.WEAPONS_NAMES[i] + "_level", -1);

                weaponsLevels[i] = PlayerPrefs.GetInt(DataHelper.WEAPONS_NAMES[i] + "_level", -2);
            }

            weaponsAmmos[i] = PlayerPrefs.GetInt(DataHelper.WEAPONS_NAMES[i] + "_bullets", DataHelper.WEAPONS_DEFAULT_BULLETS[i]);
            if (weaponsAmmos[i] > 0)
            {
                PlayerPrefs.SetInt(DataHelper.WEAPONS_NAMES[i] , 1);
            }
        }


	}



	void Update(){

		//int index = GetWeaponIndexByName(cod.centeredObject.name);
		//upgradePriceLabel.text = WeaponsData.WeaponsPricesPerUpgrade[index, weaponsLevels[index]]+"";

		//UpdateUpgradeSprites(index);

	}



	public int  UpgradeWeapon (int weaponIndex) {
        int level = PlayerPrefs.GetInt(DataHelper.WEAPONS_NAMES[weaponIndex] + "_level", 0);
		if (level >= 4)
			return 4;
		weaponsLevels[weaponIndex] ++;
		PlayerPrefs.SetInt(DataHelper.WEAPONS_NAMES[weaponIndex]+"_level", level+1);

		return level+1;
	}

    public int AddAmmo(int weaponIndex, int ammoAmount)
    {
        //int ammo = weaponsAmmos[weaponIndex];

        weaponsAmmos[weaponIndex] += ammoAmount ;
        PlayerPrefs.SetInt(DataHelper.WEAPONS_NAMES[weaponIndex] + "_bullets", weaponsAmmos[weaponIndex]);

        return weaponsAmmos[weaponIndex];
    }


	/*
	public void UpgradeWeapon () {
		int level = PlayerPrefs.GetInt(cod.centeredObject.name+"_level", 0);
		weaponsLevels[GetWeaponIndexByName(cod.centeredObject.name)] ++;
		PlayerPrefs.SetInt(cod.centeredObject.name+"_level", lerel+1);
	}
*/
	private int GetWeaponIndexByName(string weaponName){
		if(weaponName == "pistol"){
			return 0;
		}
		else if(weaponName == "gun"){
			return 1;
		}
		else if(weaponName == "bottle"){
			return 2;
		}
		else if(weaponName == "bomb"){
			return 3;
		}
		else if(weaponName == "bow"){
			return 4;
		}
		else if(weaponName == "dynamite"){
			return 5;
		}
		return -1;
	}

    public int UnlockWeapon(int weaponIndex)
    {
        PlayerPrefs.SetInt(DataHelper.WEAPONS_NAMES[weaponIndex], 1);
        PlayerPrefs.SetInt(DataHelper.WEAPONS_NAMES[weaponIndex] + "_level", 0);
        weaponsLevels[weaponIndex] = 0;

        return weaponsLevels[weaponIndex];

    }

    

}
