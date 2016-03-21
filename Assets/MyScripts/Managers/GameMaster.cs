using UnityEngine;
using System.Collections;

public class GameMaster : MonoBehaviour
{

    public static GameMaster Instance;
    public GameObject KidPlayer;

    public bool isStaticEnvironment;
    public bool grandFilpped;

    //public Transform EnemyEvadesParent;

    // public Transform LeftEvadesParentPosition;
    //public Transform LeftStartEvadePosition;

    //public float evadesPosX = -5f;

    private GameObject player;

    public GameObject winScreen;
    public GameObject loseScreen;
    public UIPanel reviveScreen;

    public GameObject[] starsIcons;

    //public float posXToStartEvade = 5.5f;

    public int OwnedCoins;
    public UILabel goldLabel;

    public int collectedCoins;

    public int totalEnemiesNumer;

    public int starsCount;

    public float GoldenStarTime = 15;
    public GameObject GoldenStarGO;

    public UILabel winGoldNumLabel;

    public UILabel[] weaponBulletsLabels;

    //public Transform rightEvadesPos;
    //public Transform rightCameraPos;
    public float switchCameraSpeed;

    public float delayToLose = 2;

    public bool zoomBehave;

    public bool zoomCamera;
    public Transform zoomToLocation;
    public float zoomCameraSpeed = 2;
    public float followCameraSpeed = 3;
    public float zoomAmount;
    public float zoomOutCameraSpeed = 1;

    private Vector3 originalCameraLocation;
    private float originalCameraZoom;

    public float X_startEvadePos;
    public float Y_startEvadePos;

    public bool CameraStartEffect;

    // public float LeftHudThreshold = 72f;
    //public float UnderScreenThreshold = 36f;
    // public float AboveScreenThreshold = -30f;

    public bool isDead;

    // public Transform rightEscapePoint;
    // public Transform leftEscapePoint;

    // public Transform leftCameraPosition;
    // public Transform RightCameraPosition;

    public bool isKid;

    public Transform AnimalsEscapePoint;

    private int collectedGold;
    //  public Transform coinsCollLocation;

    public Transform pivote_Man;
    public Transform pivote_Kid;

    private bool camEffect;


    public bool inputEnabled;

    public GameObject[] coinsReduced;

    public Transform minus5Coins;

    void Awake()
    {
        //temp for test
        //PlayerPrefs.DeleteAll();


        Instance = this;

    }

    void Start()
    {
        inputEnabled = false;
        Debug.Log(Screen.width + " , " + Screen.height);

        //AboveScreenThreshold = Screen.height -( Screen.height * .25f);
        // LeftHudThreshold = (70 * Screen.width / Screen.height);
        // UnderScreenThreshold = (55 * Screen.width / Screen.height);

        // AboveScreenThreshold = Screen.height - 141;
        //  LeftHudThreshold = 92;
        // UnderScreenThreshold = 120;

        X_startEvadePos = ScreenManager.Instance.InsideScreenTrigg_RightPos.position.x;
        Y_startEvadePos = ScreenManager.Instance.InsideScreenTrigg_RightPos.position.y;

        originalCameraLocation = Camera.main.transform.position;
        originalCameraZoom = Camera.main.orthographicSize;

        player = GameObject.FindGameObjectWithTag("Player");

        //setting man pivote
        player.transform.GetChild(0).position = pivote_Man.position;

        if (isStaticEnvironment)
        {
            grandFilpped = true;
            AnimalsEscapePoint = ScreenManager.Instance.AnimalsRightEscape;

        }

        OwnedCoins = PlayerPrefs.GetInt("gold", 50000);
        goldLabel.text = OwnedCoins + "";
        collectedCoins = 0;
        starsCount = 0;

        Invoke("DropGoldenStar", GoldenStarTime);

        AdjustGold(0);

        //Invoke("Win", 5);

        if (CameraStartEffect)
        {
            Camera.main.transform.GetComponent<Animation>().enabled = true;
            originalCameraLocation = ScreenManager.Instance.CameraRightPos.position;
        }
    }

    public void EnableInput()
    {
        inputEnabled = true;
    }
    void EnableCameraTween()
    {
        camEffect = true;
    }
    void DropGoldenStar()
    {
        GoldenStarGO.SetActive(true);
    }

    public void AdjustGold(int amount)
    {
        OwnedCoins += amount;
        collectedGold += amount;
        PlayerPrefs.SetInt("gold", OwnedCoins);
        goldLabel.text = OwnedCoins + "";

        collectedCoins++;
        if (collectedCoins == totalEnemiesNumer)
        {
            starsCount++;
        }

    }

   

    /*
	public void AdjustWeaponBullets(int index, int value){
		weaponBulletsLabels[index].text = value+"";
	}
    */
    public void TakeGoldenStar()
    {
        starsCount++;
    }

    public void ChangeToKid()
    {

        //setting kid pivote
        player.transform.GetChild(0).position = pivote_Kid.position;

        isKid = true;
        Weapons_PUs_Manager.Instance.WeaponsButtonsGOs[0].parent.gameObject.SetActive(false);
        //Weapons_PUs_Manager.Instance*PowerUpsButtonsGOs[0].parent.gameObject.SetActive(false);

        //no molotove no icebomb
        Weapons_PUs_Manager.Instance.PowerUpsButtonsGOs[0].gameObject.SetActive(false);
        Weapons_PUs_Manager.Instance.PowerUpsButtonsGOs[1].gameObject.SetActive(false);
        Weapons_PUs_Manager.Instance.PowerUpsButtonsGOs[3].gameObject.SetActive(false);
        // Weapons_PUs_Manager.Instance.PowerUpsButtonsGOs[1].parent.GetComponent<UIGrid>().Reposition();
        if (zoomBehave)
            zoomCamera = true;
       

        if (isStaticEnvironment)
        {
            player.GetComponent<StaticPlayerController>().SwitchToKid();
            AnimalsEscapePoint = ScreenManager.Instance.AnimalsLeftEscape;
        }
        else
            player.GetComponent<DynamicPlayerController>().SwitchToKid();


        //player.GetComponent<PlayerController>().
        player.GetComponent<Weapons>().currentWeapon = DataHelper.KID_WEAPON_INDEX;

        ScreenManager.Instance.EnemyEvadesParent.position = ScreenManager.Instance.EvadesForKidLeftPos.position;
        X_startEvadePos = ScreenManager.Instance.InsideScreenTrigg_LeftPos.position.x;
        Y_startEvadePos = ScreenManager.Instance.InsideScreenTrigg_LeftPos.position.y;

        //EnemyEvadesParent.transform.position += new Vector3(evadesPosX, 0, 0);

        //jumpButtoj.SetActive(false);
        ////posXToStartEvade *= -1;
        //ObstaclesParent.SetActive(false);
    }


    /*
    public void FlipGrandpaToRight()
    {
        grandFilpped = false;
        /// player.GetComponent<DynamicPlayerController>().FlipToRight();

        EnemyEvadesParent.transform.position = rightEvadesPos.position;
        posXToStartEvade *= -1;
    }
    */

    public void Win()
    {
        SoundManager.StopSFX();
        CancelInvoke("DropGoldenStar");
        //obstaclesParent.SetActive(false);
        Health playerHealthScript = player.GetComponent<Health>();
        if (playerHealthScript.currentHealth == playerHealthScript.MaxHealth)
        {
            starsCount++;
        }

        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
            winScreen.SetActive(true);
            //hero
            winGoldNumLabel.text = collectedGold + "";

            for (int i = 0; i < starsCount; i++)
            {
                starsIcons[i].SetActive(true);
            }


        }

    }
    public void Lose()
    {
        if (!isDead)
        {
            CancelInvoke("DropGoldenStar");
            Invoke("DisplayRevive", delayToLose);
            isDead = true;
        }
        //obstaclesParent.SetActive(false);
    }

    void DisplayRevive()
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0.1f;
            reviveScreen.alpha = 1;
            reviveScreen.transform.GetComponentInChildren<ReviveCounter>().PlayCounter();

        }
    }

    public void ResumeLose()
    {
        reviveScreen.alpha = 0;

        Time.timeScale = 0;
        loseScreen.SetActive(true);


    }

    public void Revive()
    {
        Time.timeScale = 1;
        reviveScreen.alpha = 0;
        player.GetComponent<Health>().RestoreHealth();

        isDead = false;
        EnemyManager.Instance.KillAll();

    }

    void Update()
    {
        //tempppp
        if (Input.GetKey(KeyCode.Escape))
        {
            Time.timeScale = 1;
            Application.LoadLevel(Application.loadedLevel);

        }

        /*
        if (camEffect)
        {
           // Debug.Log("loggg heree");
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position,
                                                          ScreenManager.Instance.CameraRightPos.position,
                                                          Time.deltaTime * switchCameraSpeed);
        }

        if (Vector3.Distance(Camera.main.transform.position, ScreenManager.Instance.CameraRightPos.position) <= .02)
        {
            camEffect = false;
            Camera.main.transform.position = ScreenManager.Instance.CameraRightPos.position;
        }
        */
        if (zoomCamera)
        {
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position,
                                                          zoomToLocation.position,
                                                          Time.deltaTime * followCameraSpeed);
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize,
                                                      zoomAmount,
                                                      Time.deltaTime * zoomCameraSpeed);
            if (!IsInvoking("CameraBack"))
            {
                Invoke("CameraBack", 2f);
            }
        }
       
        else
        {
            if (isStaticEnvironment && isKid)
                originalCameraLocation = ScreenManager.Instance.CameraLeftPos.position;
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position,
                                                          originalCameraLocation,
                                                          Time.deltaTime * followCameraSpeed);
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize,
                                                      originalCameraZoom,
                                                      Time.deltaTime * zoomOutCameraSpeed);
        }

    }

    void CameraBack()
    {
        zoomCamera = false;
    }

    public void EnableShop()
    {
        PlayerPrefs.SetInt("openShop", 1);
    }

    public void TakeHpStar()
    {
        player.GetComponent<Health>().AddHp(player.GetComponent<Health>().MaxHealth * DataHelper.HP_RESTORE_PERCENTAGE/100);
    }


    public void DisplayDroppedCoins()
    {
        minus5Coins.GetComponent<TweenPosition>().ResetToBeginning();
        minus5Coins.GetComponent<TweenAlpha>().Toggle();
        minus5Coins.GetComponent<TweenPosition>().Toggle();

        if (coinsReduced.Length > 0)
        {
            Vector2 forceDirection;
            GameObject go;
            foreach (GameObject coin in coinsReduced)
            {
                if (coin.active == true)
                {
                    go = GameObject.Instantiate(coinsReduced[0], Vector3.zero, Quaternion.identity) as GameObject;
                    go.transform.parent = coinsReduced[0].transform.parent;
                    go.transform.localScale = coinsReduced[0].transform.localScale;

                    go.transform.position = player.transform.position;

                    go.SetActive(true);

                    forceDirection = new Vector2(Random.Range(-0.7f, 0.7f), Random.Range(0.8f, 1f)) * 200;

                    go.GetComponent<Rigidbody2D>().AddForce(forceDirection);

                }
                else
                {
                    coin.transform.position = player.transform.position;

                    coin.SetActive(true);

                    forceDirection = new Vector2(Random.Range(-0.7f, 0.7f), Random.Range(0.8f, 1f)) * 200;

                    coin.GetComponent<Rigidbody2D>().AddForce(forceDirection);
                }
                   
                
               
            }
        }
    }
}