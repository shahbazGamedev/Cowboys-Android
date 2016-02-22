using UnityEngine;
using System.Collections;

public class Weapons_PUs_Manager : MonoBehaviour {

    public static Weapons_PUs_Manager Instance;

    public Transform[] WeaponsButtonsGOs;
    public Transform[] PowerUpsButtonsGOs;
   


    //public int WeaponsNumber = 9;
    //public string[] WeaponsNames = new string[]{"pistol", "gun", "bottle", "bomb", "bow", "dynamite", "molotove", "iceBomb", "electricBomb"};

    public int[] OwnedWeapons;


    public int[] BulletsNumbersList;
    public int[] WeaponUpgradeLevelsList;


    public GameObject[] bulletsPrefabs;
    public Transform[] bulletsSpawns;

    public float[] SpawnDelayes = { .3f, .3f, .6f, .6f, .5f, .6f, .6f, .6f, .6f, .6f, .3f };
    public float[] ReshootDelayes = { .6f, .6f, .8f, .8f, .9f, .8f, .8f, .8f, .8f, .8f, .6f };

    public string[] ShootAnimationNamesList = new string[] { "PistolShoot", 
        "GunShoot", 
        "BottleShoot", 
        "BombShoot", 
        "BowShoot", 
        "DynamiteShoot", 
        "BottleShoot", 
        "BombShoot", 
        "BombShoot",
        "BombShoot",
        "KidAttack"};

    public int[] currentBulletsCounts;

    public GameObject electricFieldGO;
    public Transform shieldBarTransform;

	void Awake () {
        Instance = this;
	}

    void Start()
    {

        InitWeapons_PUsData();
        FillWeaponsUI();
    }


    private void InitWeapons_PUsData()
    {
        OwnedWeapons = new int[DataHelper.WEAPONS_NUMBER];
        BulletsNumbersList = new int[DataHelper.WEAPONS_NUMBER];
        WeaponUpgradeLevelsList = new int[DataHelper.WEAPONS_NUMBER];
        currentBulletsCounts = new int[DataHelper.WEAPONS_NUMBER];

        for (int i = 0; i < DataHelper.WEAPONS_NUMBER; i++)
        {


            OwnedWeapons[i] = PlayerPrefs.GetInt(DataHelper.WEAPONS_NAMES[i], DataHelper.DEFAULT_OWNED[i]);


            WeaponUpgradeLevelsList[i] = PlayerPrefs.GetInt(DataHelper.WEAPONS_NAMES[i]+"_level", 0);

            //temp
           // WeaponUpgradeLevelsList[i] = 4;

            BulletsNumbersList[i] = PlayerPrefs.GetInt(DataHelper.WEAPONS_NAMES[i]+ "_bullets", DataHelper.WEAPONS_DEFAULT_BULLETS[i]);

            if (BulletsNumbersList[i] == 0)
            {
               // OwnedWeapons[i] = 0;
            }

            currentBulletsCounts[i] = BulletsNumbersList[i];

            
        }

        

    }

    private void FillWeaponsUI()
    {
        int currentStage = PlayerPrefs.GetInt("currentStage", 1);
        for (int i = 0; i < 6; i++)
        {
            Debug.Log(OwnedWeapons[i]);
            if (OwnedWeapons[i] == 1)
            {
                WeaponsButtonsGOs[i].gameObject.SetActive(true);
                AdjustWeaponBullets(i, BulletsNumbersList[i]);

            }
            else if (currentStage >= DataHelper.ULOCKING_LEVEL[i])
            {
                WeaponsButtonsGOs[i].gameObject.SetActive(true);
                WeaponsButtonsGOs[i].GetComponent<Button_SwitchWeapon>().locked = true;
                WeaponsButtonsGOs[i].GetComponent<UIButton>().normalSprite = "lock_" + WeaponsButtonsGOs[i].GetComponent<UIButton>().normalSprite;
                WeaponsButtonsGOs[i].GetComponentInChildren<UILabel>().text = "";
            }
            else
            {
                WeaponsButtonsGOs[i].gameObject.SetActive(false);
            }

        }

        for (int i = 6 ; i < DataHelper.WEAPONS_NUMBER; i++)
        {
            if (OwnedWeapons[i] == 1)
            {
                PowerUpsButtonsGOs[i-6].gameObject.SetActive(true);
                AdjustPUsBullets(i, BulletsNumbersList[i]);

            }
            else
            {
                PowerUpsButtonsGOs[i-6].gameObject.SetActive(false);
            }
           
        }

        WeaponsButtonsGOs[0].parent.GetComponent<UIGrid>().Reposition();
        //PowerUpsButtonsGOs[0].parent.GetComponent<UIGrid>().Reposition();

    }

    public void AdjustWeaponBullets(int index, int value)
    {
        WeaponsButtonsGOs[index].GetComponentInChildren<UILabel>().text = value + "";
        BulletsNumbersList[index] = value;

        PlayerPrefs.SetInt(DataHelper.WEAPONS_NAMES[index] + "_bullets", BulletsNumbersList[index]);

        if (value <= 0)
        {
            GameObject.FindGameObjectWithTag("defaultWeapon").GetComponent<Button_SwitchWeapon>().SwitchWeapon();
        }
    }

    public void AdjustPUsBullets(int index, int value)
    {
        PowerUpsButtonsGOs[index-6].GetComponentInChildren<UILabel>().text = value + "";

        BulletsNumbersList[index] = value;

        PlayerPrefs.SetInt(DataHelper.WEAPONS_NAMES[index] + "_bullets", BulletsNumbersList[index]);
        if (value <= 0)
        {
            GameObject.FindGameObjectWithTag("defaultWeapon").GetComponent<Button_SwitchWeapon>().SwitchWeapon();
        }
    }

    public void DisplayElectricFieldFX()
    {
        if (electricFieldGO)
        {
            electricFieldGO.SetActive(true);
            Invoke("DisableElectricField", .48f);
        }
    }

    void DisableElectricField()
    {
        electricFieldGO.SetActive(false);
    }

    //public void DisplayShieldBar()
    //{
    //    if (shieldBarTransform)
    //    {
    //        shieldBarTransform.gameObject.SetActive(true);

    //    }
    //}


    

}
