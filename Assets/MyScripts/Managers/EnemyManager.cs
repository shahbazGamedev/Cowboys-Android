using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

public class EnemyManager : MonoBehaviour {

	public static EnemyManager Instance;

	List<Vector2> [] enemiesGroups;
	//public int [] groups;
	public GameObject [] enemyPrefabs;
	public Transform [] frontSpawnPoints;
	public Transform [] backSpawnPoints;

	public UISlider levelProgressBar;

	public int kidGroupIndex;
	public int grandpaReFlipIndex;

	public float spawnEnemiesDelay = 1f;

	public int currentGroup = 0;

	public GameObject enemyHealtgBarPrefab;
	public Transform BarsParent;

	public int currentRemaining;

	private bool updateProgressBar;

	public GameObject coinPrefab;

	public List<GameObject> healthBarsPool;

    public GameObject hpGiftPrefab;

    public TweenScale doubleKillTweener;
    public TweenScale tripleKillTweener;
    public TweenScale allKillTweener;

    public GameObject rangedEnemyBulletPrefab;

    public float multiKillTimer = 0f;
    public float multikillRate = .1f;
    public int multikills;

    private Dictionary<int, List<GameObject>> EnemiesPool; 

	void Awake(){
		Instance = this;

        healthBarsPool = new List<GameObject>();
        EnemiesPool = new Dictionary<int, List<GameObject>>();

	}

	void Start () {

	    enemiesGroups = EnemiesSpawnData.Instance.InitEnemiesGroups();
        
	    Invoke("SpawnNextFrontGroup", spawnEnemiesDelay);

        
		
	}

    bool started;
    public void StartEnemies()
    {
        //enemyPrefabs[0].GetComponent<EnemyAI>().moveSpeed = 0;

        enemiesGroups = EnemiesSpawnData.Instance.InitEnemiesGroups();
        Invoke("SpawnNextFrontGroup", spawnEnemiesDelay);

        started = true;
    }


	/*
	void SpawnEnemy () {
		int index = Random.Range (0, spawnPoints.Length);
		GameObject enemy = Instantiate (enemyPrefab, spawnPoints[index].position, spawnPoints[index].rotation) as GameObject;

		GameObject healthBar = Instantiate (enemyHealtgBarPrefab, Vector3.zero, Quaternion.identity) as GameObject;

		healthBar.transform.parent = BarsParent;
		healthBar.transform.localScale = new Vector3 (0.4974203f, 0.4974203f, 0.4974203f);

		//enemy.GetComponent<EnemyHealth>().healthBar = healthBar.GetComponent<UISlider>();

	}


*/

	void SpawnNextFrontGroup(){
		//currentRemaining = enemiesGroups[currentGroup].Count;
		currentRemaining = CalculateRowTotalEnemies (enemiesGroups[currentGroup]);
		Vector2 data;
		for(int i=0; i<enemiesGroups[currentGroup].Count; i++){
			data = enemiesGroups[currentGroup][i];
			for(int j=0; j<data.y ; j++){
                GameObject enemy;
                if (EnemiesPool.ContainsKey((int)data.x))
                {
                    enemy = EnemiesPool[(int)data.x][0];
                    UpdateEnemiesPool((int)data.x, null);

                    enemy.transform.position = frontSpawnPoints[i + j].position;
                    enemy.transform.rotation = frontSpawnPoints[i + j].rotation;

                    
                    enemy.GetComponent<EnemyHealth>().Reset();
                    enemy.GetComponent<EnemyAI>().Reset();
                    enemy.SetActive(true);
                }
                else
                {
                    enemy = Instantiate(enemyPrefabs[(int)data.x], frontSpawnPoints[i + j].position, frontSpawnPoints[i + j].rotation) as GameObject;

                    //UpdateEnemiesPool((int)data.x, enemy);
                }


                enemy.name = (int)data.x + "";
				GameObject healthBar = CreateHealthBar();

				enemy.GetComponent<EnemyHealth>().healthBar = healthBar.GetComponent<UISlider>();
				healthBar.SetActive(true);
			}
		}
	}

	private GameObject CreateHealthBar (){
		GameObject healthBar;
		if (healthBarsPool.Count != 0) {
			healthBar = healthBarsPool[0];
			healthBarsPool.RemoveAt(0);
			healthBar.GetComponent<UISlider>().value = 1;
            healthBar.transform.localPosition = DataHelper.HEALTH_BAR_DEAD;
		}
		else{
			healthBar = Instantiate (enemyHealtgBarPrefab, Vector3.zero, Quaternion.identity) as GameObject;
			healthBar.transform.parent = BarsParent;
			healthBar.transform.localScale = new Vector3 (0.4974203f, 0.4974203f, 0.4974203f);
		}
		return healthBar;
	}


    private void UpdateEnemiesPool(int key, GameObject enemy)
    {
        if (enemy != null)
        {
            if (EnemiesPool.ContainsKey(key))
            {
                EnemiesPool[key].Add(enemy);
            }
            else
            {
                Debug.Log(key +" kk "+ gameObject.name);
                List<GameObject> list = new List<GameObject>();
                list.Add(enemy);
                EnemiesPool.Add(key, list);
            }
            return;
        }

        if (EnemiesPool[key].Count <= 1)
        {
            EnemiesPool.Remove(key);
            return;
        }

        EnemiesPool[key].RemoveAt(0);

    }

	private int CalculateRowTotalEnemies(List<Vector2> rowList){
		int count = 0;
		for (int i=0; i<rowList.Count; i++) {
			count += (int)rowList[i].y;
		}
		return count;
	}

	public void AdjustRemainingEnemies(GameObject go){
        UpdateEnemiesPool(int.Parse(go.name), go);
        currentRemaining--;
        
       
		if(currentRemaining == 0){
            Debug.Log(currentGroup+"");
			//InvokeRepeating(AdjustProgressBar, );
			//levelProgressBar.value = (float)((float)(currentGroup+1)/ (float)(groups.Length));
			currentGroup ++;

			if(currentGroup == enemiesGroups.Length ){
				GameMaster.Instance.Invoke("Win", DataHelper.WIN_DELAY);
				return;
			}

			if(currentGroup == kidGroupIndex){
				Invoke("SpawnNextBackGroup", spawnEnemiesDelay+4);
				GameMaster.Instance.Invoke("ChangeToKid", 3);
				return;
			}

            /*
			if(currentGroup == grandpaReFlipIndex){
				Invoke("SpawnNextFrontGroup", spawnEnemiesDelay);
				GameMaster.Instance.Invoke("FlipGrandpaToRight", 3);
				return;
			}
            */
			if(currentGroup<enemiesGroups.Length){
                
				if(!GameMaster.Instance.isKid)
					Invoke("SpawnNextFrontGroup", spawnEnemiesDelay);
				else
					Invoke("SpawnNextBackGroup", spawnEnemiesDelay);
			}
			//levelProgressBar.value = (float)((float)(currentGroup+1)/ (float)(groups.Length));
		}
       
	}

	void SpawnNextBackGroup(){

		currentRemaining = CalculateRowTotalEnemies (enemiesGroups[currentGroup]);
		Vector2 data;
		for(int i=0; i<enemiesGroups[currentGroup].Count; i++){

			data = enemiesGroups[currentGroup][i];
			for(int j=0; j<data.y ; j++){
                GameObject enemy;

                if (EnemiesPool.ContainsKey((int)data.x))
                {
                    enemy = EnemiesPool[(int)data.x][0];
                    UpdateEnemiesPool((int)data.x, null);

                    enemy.transform.position = backSpawnPoints[i + j].position;
                    enemy.transform.rotation = backSpawnPoints[i + j].rotation;


                    enemy.GetComponent<EnemyHealth>().Reset();
                    enemy.GetComponent<EnemyAI>().Reset();
                    enemy.SetActive(true);
                }
                else
                {
                   enemy = Instantiate(enemyPrefabs[(int)data.x], backSpawnPoints[i + j].position, backSpawnPoints[i + j].rotation) as GameObject;
                    //UpdateEnemiesPool((int)data.x, enemy);
                }


                enemy.name = (int)data.x + "";

                GameObject healthBar = Instantiate (enemyHealtgBarPrefab, Vector3.zero, Quaternion.identity) as GameObject;
				
				healthBar.transform.parent = BarsParent;
				healthBar.transform.localScale = new Vector3 (0.4974203f, 0.4974203f, 0.4974203f);
				enemy.GetComponent<EnemyHealth>().healthBar = healthBar.GetComponent<UISlider>();
			}
		}
	}

	void Update(){
       // if(started)
		    levelProgressBar.value = Mathf.Lerp(levelProgressBar.value , 1- (float)((float)(currentGroup)/ (float)(enemiesGroups.Length)), Time.deltaTime * 2);

	}

	public void DropCoin(Transform coinPos, int goldAmount){

        GameObject go = Instantiate(coinPrefab, coinPos.position, Quaternion.identity) as GameObject;

        go.GetComponent<CoinDropped>().goldAmount = goldAmount;
       
        //GameObject go =  Instantiate(coinPrefab, coinPos.position, coinPos.rotation)as GameObject;

        //go.transform.parent = GameObject.FindGameObjectWithTag( "uiCamera").transform;

        //go.transform.localPosition = coinPos.localPosition;
        //go.transform.rotation = coinPos.rotation;
        //go.transform.localScale = new Vector3(1,1,1);

        //go.GetComponent<Button_CoinDropped>().startingPos = go.transform.localPosition;
        //go.GetComponent<Button_CoinDropped> ().goldAmount = goldAmount;

	}

    public void DropHpGift(Transform enemy)
    {
        GameObject go = Instantiate(hpGiftPrefab, enemy.position, Quaternion.identity) as GameObject;

        Vector2 forceDirection = new Vector2(-.5f, .7f) * 200;

        go.GetComponent<Rigidbody2D>().AddRelativeForce(forceDirection);
    }

    public void KillAll()
    {
        GameObject [] EnemiesArea = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < EnemiesArea.Length; i++)
        {

            if (EnemiesArea[i].activeSelf)
                EnemiesArea[i].SendMessage("ApplyDamage", 1000);

        }
    }

    public void TriggerMultikills()
    {
        multiKillTimer = .3f;

        InvokeRepeating("ResetTimer", 0, multikillRate);
    }

    void ResetTimer()
    {
        if (multiKillTimer <= 0)
        {

            multiKillTimer = 0;
            if (multikills > 4)
                multikills = 100;
            MultiKillComment();
            CancelInvoke("ResetTimer");
            return;
        }
        multiKillTimer -= multikillRate;
    }

    public void MultiKillComment()
    {
        switch (multikills)
        {
            case 2:
                DoubleKillComment();
                break;
            case 3:
                TripleKillComment();
                break;
            case 4:
                TripleKillComment();
                break;
            case 100:
                AllKillComment();
                break;
            default:
                break;
        }
        multikills = 0;
    }

    void DoubleKillComment()
    {
        doubleKillTweener.Toggle();
    }

    void TripleKillComment()
    {
        tripleKillTweener.Toggle();
    }
    void AllKillComment()
    {
        allKillTweener.Toggle();
    }
}
