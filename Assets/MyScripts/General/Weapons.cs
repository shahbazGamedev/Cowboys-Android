using UnityEngine;
using System.Collections;

public class Weapons : MonoBehaviour {

    private Weapons_PUs_Manager mngScript;

	public int currentWeapon = 0;

	public int WeaponsNumber = 9;

    public int kidWeaponIndex = 9;

    //public string[] ShootAnimationNamesList = new string[] { "PistolShoot", "GunShoot", "BottleShoot", "BombShoot", "BowShoot", "DynamiteShoot", "KidAttack", "BottleShoot", "BombShoot", "BombShoot" };
	//public int [] BulletsNumbersList;
	//public int[] WeaponUpgradeLevelsList;


	//public GameObject [] bulletsPrefabs;
	//public Transform [] bulletsSpawns;
	//public 

	//public float [] SpawnDelayes = {.1f, .1f, .6f, .4f, .4f, .4f};
	//public float [] ReshootDelayes = {.5f, .5f, .8f, .8f, .8f, .8f};

	public bool reloading;

	//public int [] currentBulletsCounts;

	public float reloadingTime = 3;

    private Button_SwitchWeapon lastSelected;

    private DynamicPlayerController dpCntScript;

    private StaticPlayerController spCntScript;

	void Start () {
        currentWeapon = DataHelper.DEFAULT_SELECTED_WEAPON_INDEX;
        lastSelected = GameObject.FindGameObjectWithTag("defaultWeapon").GetComponent<Button_SwitchWeapon>();
        mngScript = Weapons_PUs_Manager.Instance;

        spCntScript = GetComponent<StaticPlayerController>();
        dpCntScript = GetComponent<DynamicPlayerController>();
	}

	public void WeaponShootAnimation (Animator anim, bool state) {
       // Debug.Log(currentWeapon+"");
        if(anim.isInitialized)
		    anim.SetBool(mngScript.ShootAnimationNamesList[currentWeapon], state);
	}

	public void Reload(){

	}

	public void SpawnManShooting(){
        SoundManager.PlaySFX(DataHelper.SFX_FIRING[currentWeapon]);
        if (currentWeapon == 6)
        {
            if (spCntScript)
            {
                spCntScript.manAnim.SetBool("bottle_idle", true);
            }
            if (dpCntScript)
            {
                dpCntScript.manAnim.SetBool("bottle_idle", true);
            }
        }
        else if (currentWeapon == 7)
        {
            if (spCntScript)
            {
                spCntScript.manAnim.SetBool("bomb_idle", true);
            }
            if (dpCntScript)
            {
                dpCntScript.manAnim.SetBool("bomb_idle", true);
            }
        }
        else
        {
            if (spCntScript)
            {
                spCntScript.manAnim.SetBool(DataHelper.WEAPONS_NAMES[currentWeapon] + "_idle", true);
            }
            if (dpCntScript)
            {
                dpCntScript.manAnim.SetBool(DataHelper.WEAPONS_NAMES[currentWeapon] + "_idle", true);
            }
        }
		//new Fire
        int weaponLevel = mngScript.WeaponUpgradeLevelsList[currentWeapon];

        if (currentWeapon < 6) {
		    if(currentWeapon == 0){
			    FireNormal(currentWeapon, weaponLevel);
		    }
		    else if(currentWeapon == 1){
			    FireNormal(currentWeapon, weaponLevel);
		    }
		    else if(currentWeapon == 2){ //bottle
			    FireBottles(currentWeapon, weaponLevel);
                
                //infinite bottle count
                return;
		    }
		    else if(currentWeapon == 3){
			    FireNormal(currentWeapon, weaponLevel);
		    }
		    else if(currentWeapon == 4){ //bowww
			    FireBows(currentWeapon, weaponLevel);
		    }
		    else if(currentWeapon == 5){ //dynamite
			    FireNormal(currentWeapon, weaponLevel);
		    }

		    mngScript.currentBulletsCounts[currentWeapon] --;
            mngScript.AdjustWeaponBullets(currentWeapon, mngScript.currentBulletsCounts[currentWeapon]);
         }
        else
        {
            if (currentWeapon == DataHelper.KID_WEAPON_INDEX)
            {
                return;
            }
            if (currentWeapon == 6)
            {
                FireNormal(currentWeapon, weaponLevel);
            }
            else if (currentWeapon == 7)
            {
                FireNormal(currentWeapon, weaponLevel);
            }
            else if (currentWeapon == 8)
            { 
               // FireNormal(currentWeapon, weaponLevel);
            }
            mngScript.currentBulletsCounts[currentWeapon]--;
            mngScript.AdjustPUsBullets(currentWeapon, mngScript.currentBulletsCounts[currentWeapon]);
        }
        
	}

	private void FireNormal(int weap, int weapLevel){

		Vector3 mousePos =  Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePos.z =0;
		GameObject bullet = Instantiate (mngScript.bulletsPrefabs[weap] ,
                                         mngScript.bulletsSpawns[weap].position,
                                         mngScript.bulletsSpawns[weap].rotation) as GameObject;
        bullet.transform.parent = mngScript.bulletsSpawns[weap].transform.parent;
        bullet.transform.localScale = mngScript.bulletsSpawns[weap].transform.localScale;
        bullet.transform.position = mngScript.bulletsSpawns[weap].position;
		bullet.transform.parent = null;
		Vector3 direction = mousePos - bullet.transform.position;
		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		if(angle > 80){
			angle = 80;
		}

        //if(angle < 0){
        //    angle = 0;
        //}

		BulletMover bmScript = bullet.GetComponent<BulletMover> ();
		bmScript.damage = DataHelper.WeaponsPowerPerUpgrade[weap, weapLevel];

        //new for modificatoins
        if (weap == 0 || weap == 1) //pistol or gun
        {
            bmScript.SetDestination(mousePos);
        }
       

		bullet.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
	}

	private void FireBottles(int weap, int weapLevel){
		GameObject [] bulletsList = new GameObject[DataHelper.WeaponsFiresPerUpgrade[weap, weapLevel]];

		Vector3 mousePos =  Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePos.z =0;

		for(int i=0; i<bulletsList.Length; i++){

            GameObject bullet = Instantiate(mngScript.bulletsPrefabs[weap],
                                             mngScript.bulletsSpawns[weap].position,
                                             mngScript.bulletsSpawns[weap].rotation) as GameObject;
            bullet.transform.parent = mngScript.bulletsSpawns[weap].transform.parent;
            bullet.transform.localScale = mngScript.bulletsSpawns[weap].transform.localScale;
            bullet.transform.position = mngScript.bulletsSpawns[weap].GetChild(i).position;
			bullet.transform.parent = null;

			
			BulletMover bmScript = bullet.GetComponent<BulletMover> ();
			bmScript.damage = DataHelper.WeaponsPowerPerUpgrade[weap, weapLevel];

            //new for modificatoins
            //bmScript.SetDestination(mousePos);
            

           

			bulletsList[i] = bullet;
		}

		Vector3 direction = mousePos - bulletsList[0].transform.position;
		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		if(angle > 80){
			angle = 80;
		}
        //if(angle < 0){
        //    angle = 0;
        //}
		//bulletsList[0].transform.rotation = Quaternikn.AngleAxis(angle, Vector3.forward);

		for(int i=0; i<bulletsList.Length; i++){
            if (i == 1)
            {
                bulletsList[i].transform.rotation = Quaternion.AngleAxis(angle - 10, Vector3.forward);
                bulletsList[0].GetComponent<BulletMover>().brothers.Add(bulletsList[i].transform);
            }
            else if (i == 2)
            {
                bulletsList[i].transform.rotation = Quaternion.AngleAxis(angle + 10, Vector3.forward);
                bulletsList[0].GetComponent<BulletMover>().brothers.Add(bulletsList[i].transform);
            }
            else
            {
                bulletsList[i].transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                bulletsList[0].GetComponent<BulletMover>().SetDestination(mousePos);

            }
		}
	}

	private void FireBows(int weap, int weapLevel){
		GameObject [] bulletsList = new GameObject[DataHelper.WeaponsFiresPerUpgrade[weap, weapLevel]];

		Vector3 mousePos =  Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePos.z =0;
		
		for(int i=0; i<bulletsList.Length; i++)
        {

            GameObject bullet = Instantiate(mngScript.bulletsPrefabs[weap],
                                             mngScript.bulletsSpawns[weap].position,
                                             mngScript.bulletsSpawns[weap].rotation) as GameObject;
            bullet.transform.parent = mngScript.bulletsSpawns[weap].transform.parent;
            bullet.transform.localScale = mngScript.bulletsSpawns[weap].transform.localScale;
            bullet.transform.position = mngScript.bulletsSpawns[weap].position;
			bullet.transform.parent = null;
			
			
			BulletMover bmScript = bullet.GetComponent<BulletMover> ();
			bmScript.damage = DataHelper.WeaponsPowerPerUpgrade[weap, weapLevel];

            //new for modificatoins
            //bmScript.SetDestination(mousePos);
            
            bulletsList[i] = bullet;
		}
		
		Vector3 direction = mousePos - bulletsList[0].transform.position;
		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		if(angle > 80){
			angle = 80;
		}
        //if(angle < 0){
        //    angle = 0;
        //}
		//bulletsList[0].transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		
		for(int i=0; i<bulletsList.Length; i++){
            if (i == 0)
            {
                bulletsList[i].transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                bulletsList[0].GetComponent<BulletMover>().SetDestination(mousePos);
            }
            else if (i == 1)
            {
                bulletsList[i].transform.rotation = Quaternion.AngleAxis(angle - 10, Vector3.forward);
                bulletsList[0].GetComponent<BulletMover>().brothers.Add(bulletsList[i].transform);
            }
            else if (i == 2)
            {
                bulletsList[i].transform.rotation = Quaternion.AngleAxis(angle + 10, Vector3.forward);
                bulletsList[0].GetComponent<BulletMover>().brothers.Add(bulletsList[i].transform);
            }
            else if (i == 3)
            {
                bulletsList[i].transform.rotation = Quaternion.AngleAxis(angle - 20, Vector3.forward);
                bulletsList[0].GetComponent<BulletMover>().brothers.Add(bulletsList[i].transform);
            }
            else if (i == 4)
            {
                bulletsList[i].transform.rotation = Quaternion.AngleAxis(angle + 20, Vector3.forward);
                bulletsList[0].GetComponent<BulletMover>().brothers.Add(bulletsList[i].transform);
            }


				//bulletsList[i].transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		}
	}


	public void SpawnFlippedManShooting(){
		Vector3 mousePos =  Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePos.z =0;

        GameObject bullet = Instantiate(mngScript.bulletsPrefabs[currentWeapon],
                                         mngScript.bulletsSpawns[currentWeapon].position,
                                         mngScript.bulletsSpawns[currentWeapon].rotation) as GameObject;
        bullet.transform.parent = mngScript.bulletsSpawns[currentWeapon].transform.parent;
        bullet.transform.localScale = mngScript.bulletsSpawns[currentWeapon].transform.localScale;
        bullet.transform.position = mngScript.bulletsSpawns[currentWeapon].position;
		bullet.transform.parent = null;
		Vector3 direction = mousePos - bullet.transform.position ;
		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

		bullet.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

		//Vector3 tempScale = bullet.transform.localScale;

		//tempScale.x *= -1;
		//bullet.transform.localScale = tempScale;

        mngScript.currentBulletsCounts[currentWeapon]--;
        mngScript.AdjustWeaponBullets(currentWeapon, mngScript.currentBulletsCounts[currentWeapon]);
	}

    public void FireElecticBomb()
    {
        SoundManager.PlaySFX("Man_Electric_thunder_Shoot");
        mngScript.DisplayElectricFieldFX();
        GameObject[] EnemiesArea;
        EnemiesArea = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < EnemiesArea.Length; i++)
        {
            EnemiesArea[i].SendMessage("ApplyElectircDamage");

        }

        mngScript.currentBulletsCounts[DataHelper.ELECTRIC_BOMB_INDEX]--;
        mngScript.AdjustPUsBullets(DataHelper.ELECTRIC_BOMB_INDEX, mngScript.currentBulletsCounts[DataHelper.ELECTRIC_BOMB_INDEX]);
        
    }

    public void FireShiled()
    {
        //mngScript.DisplayShieldBar();
        GetComponent<Health>().RestoreShield();

        mngScript.currentBulletsCounts[DataHelper.SHIELD_INDEX]--;
        mngScript.AdjustPUsBullets(DataHelper.SHIELD_INDEX, mngScript.currentBulletsCounts[DataHelper.SHIELD_INDEX]);
    }


	public void SpawnKidShooting(){
		Vector3 mousePos =  Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePos.z =0;

        GameObject bullet = Instantiate(mngScript.bulletsPrefabs[kidWeaponIndex],
                                         mngScript.bulletsSpawns[kidWeaponIndex].position,
                                         mngScript.bulletsSpawns[kidWeaponIndex].rotation) as GameObject;
        bullet.transform.parent = mngScript.bulletsSpawns[kidWeaponIndex].transform.parent;
        bullet.transform.localScale = mngScript.bulletsSpawns[kidWeaponIndex].transform.localScale;
        bullet.transform.position = mngScript.bulletsSpawns[kidWeaponIndex].position;
		Vector3 direction = mousePos - bullet.transform.position ;
		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

		/*
		if(angle < 100){
			angle = 100;
		}
		if(angle > 200){
			Debug.Log("habal");
			angle = 200;
		}
*/
        BulletMover bmScript = bullet.GetComponent<BulletMover>();
        bmScript.SetDestination(mousePos);

		bullet.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        SoundManager.PlaySFX("Kid_Shoot");
	}

	void Update(){

        //herooo
        if (!GameMaster.Instance.isKid && mngScript.currentBulletsCounts[currentWeapon] <= 0)
        {
           // reloading = true;
           // Invoke("endReloading", reloadingTime);
        }

		/*
		if(Input.GetAxis("Mouse ScrollWheel") > 0){
			currentWeapon ++;
			if(currentWeapon > 4){
				currentWeapon = 0;
			}
		}
		*/
	}

	void endReloading(){
        mngScript.currentBulletsCounts[currentWeapon] = mngScript.BulletsNumbersList[currentWeapon];
        if (currentWeapon > 5)
        {
            mngScript.AdjustPUsBullets(currentWeapon, mngScript.currentBulletsCounts[currentWeapon]);
        }

        else
        {
            mngScript.AdjustWeaponBullets(currentWeapon, mngScript.currentBulletsCounts[currentWeapon]);
        }
        
		reloading = false;
	}



	public void switchWeapons(Button_SwitchWeapon button, int weaponIndex){
       
        //before switching

        //molotove and icebomb

        if (GetComponent<DynamicPlayerController>())
        {
            GetComponent<DynamicPlayerController>().EnableShooting();
            GetComponent<DynamicPlayerController>().ResetRotation();
        }
        if (GetComponent<StaticPlayerController>())
        {
            GetComponent<StaticPlayerController>().EnableShooting();
            GetComponent<StaticPlayerController>().ResetRotation();
        }

         if (currentWeapon == 6)
        {
            if (spCntScript)
            {
                spCntScript.manAnim.SetBool("bottle_idle", false);
            }
            if (dpCntScript)
            {
                dpCntScript.manAnim.SetBool("bottle_idle", false);
            }
        }
        else if (currentWeapon == 7)
        {
            if (spCntScript)
            {
                spCntScript.manAnim.SetBool("bomb_idle", false);
            }
            if (dpCntScript)
            {
                dpCntScript.manAnim.SetBool("bomb_idle", false);
            }
        }
        else
        {
            if (spCntScript)
            {
                spCntScript.manAnim.SetBool(DataHelper.WEAPONS_NAMES[currentWeapon] + "_idle", false);
            }
            if (dpCntScript)
            {
                dpCntScript.manAnim.SetBool(DataHelper.WEAPONS_NAMES[currentWeapon] + "_idle", false);
            }
        }
        //after switching
         if (currentWeapon != weaponIndex)
         {
             if (dpCntScript)
                 dpCntScript.EnableShooting();
             // if(spCntScript)
             //   spCntScript.EnableShooting();

         }
       
        currentWeapon = weaponIndex;

        if (lastSelected)
        {
            lastSelected.UnhighlightButton();

        }
        lastSelected = button;
        //reloading = false;
		CancelInvoke("endReloading");
	}

}
