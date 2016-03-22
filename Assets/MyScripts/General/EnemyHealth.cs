using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour {

	public float MaxHealth = 100;
	public float currentHealth = 100;

	public UISlider healthBar;

	private Animator anim;

	public Camera uiCam;

	public bool habal = true;

	public float offsetX = -0.1f;
	public float offsetY = 0.1f;

	public int dieGoldAmount = 1;

    public SpriteRenderer FireFX;

    

	void Start () {

		currentHealth = MaxHealth;

		anim = GetComponentInChildren<Animator>();
		if(healthBar){
			healthBar.value = currentHealth/MaxHealth;
            healthBar.transform.GetChild(2).GetComponent<TweenAlpha>().ResetToBeginning();
            healthBar.transform.GetChild(2).GetComponent<TweenPosition>().ResetToBeginning();
            healthBar.transform.GetChild(2).GetComponent<UILabel>().alpha = 0;
		}
		uiCam = GameObject.FindGameObjectWithTag("uiCamera").GetComponent<Camera>();

        FireFX = transform.GetChild(1).GetComponent<SpriteRenderer>();

        transform.GetChild(2).GetComponent<SpriteRenderer>().sortingOrder += Random.Range(1, 5);

	}

	void Update(){
		//if(habal){
		//healthBar.gameObject.transform.position = uiCam.WorldToViewportPoint( transform.position );

		//healthBar.gameObject.transform.localPosition  = healthBar.gameObject.transform.localPosition - new Vector3(offsetX,  offsetY, 0);

		healthBar.gameObject.transform.position = uiCam.ScreenToWorldPoint(Camera.main.WorldToScreenPoint(transform.position)) + new Vector3(offsetX,offsetY,0);
		healthBar.gameObject.transform.position = new Vector3(healthBar.gameObject.transform.position.x, healthBar.gameObject.transform.position .y, 0);
		//}
        FireFX.transform.rotation = Quaternion.identity;
	}
	
	public void ApplyDamage (float amount) {
        if (currentHealth == 0)
            return;
		currentHealth -= amount;

        StartCoroutine("TweenEnemyColorToRed");

		if(currentHealth <= 0){
			currentHealth = 0;
            if (GetComponent<EnemyAI>())
            {
                GetComponent<EnemyAI>().StopFreezing();
                GetComponent<EnemyAI>().damage = 0;
            }
            else
            {
                GetComponent<CowEnemyAI>().StopFreezing();
                GetComponent<CowEnemyAI>().damage = 0;
            }
			Die();

		}

		if(healthBar){
			healthBar.value =  (float)( currentHealth/MaxHealth);
		}
	}

    float totalDmgPerSec = 0;
    float dmgPerSec = 0;

    public void ApplyDamagePerSecond()
    {
        if (currentHealth == 0)
            return;

        if (FireFX)
        {
            FireFX.transform.rotation = new Quaternion(0, 0, 0, 0);
            FireFX.enabled = true;
        }
        totalDmgPerSec = MaxHealth / 2;
        dmgPerSec = totalDmgPerSec / DataHelper.MOLOTOVE_DMG_TIME;

        InvokeRepeating("DamagePerSeconds", 0, DataHelper.MOLOTOVE_DMG_RATE);
      
    }

    private IEnumerator TweenEnemyColorToRed()
    {
       // Debug.Log("hoy");
        transform.GetComponentInChildren<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(.1f);
        transform.GetComponentInChildren<SpriteRenderer>().color = Color.white;
        yield return new WaitForSeconds(.1f);
        transform.GetComponentInChildren<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(.1f);
        transform.GetComponentInChildren<SpriteRenderer>().color = Color.white;
    }

    private void DamagePerSeconds()
    {

        if (totalDmgPerSec > 0)
        {
            totalDmgPerSec -= dmgPerSec;
            ApplyDamage(dmgPerSec);

        }
        else
        {
            CancelInvoke("DamagePerSeconds");
            FireFX.enabled = false;
        }

        


    }

    public void ApplyElectircDamage()
    {
        if (currentHealth == 0)
            return;

        if (GetComponent<EnemyAI>())
        {
            GetComponent<EnemyAI>().StopFreezing();
        }
        else
        {
            GetComponent<CowEnemyAI>().StopFreezing();
        }

        
        float totalDmg = (MaxHealth / 4)*3;
        currentHealth -= totalDmg;
        anim.SetBool("HitElectirc", true);

        Invoke("DisableElectircAnimation", 1.2f);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
           // GetComponent<EnemyAI>().StopFreezing();
            anim.SetBool("DieElectric", true);
            Invoke("DieElectric", 1.1f);
            

        }

        if (healthBar)
        {
            healthBar.value = (float)(currentHealth / MaxHealth);
        }

    }

    void DisableElectircAnimation()
    {
        anim.SetBool("HitElectirc", false);
    }



	void Die(){
		EnemyManager.Instance.AdjustRemainingEnemies(gameObject);


        anim.SetBool("Die", true);
        if (GetComponent<EnemyAI>())
        {
            GetComponent<EnemyAI>().StopFreezing();
            GetComponent<EnemyAI>().isDead = true;
            GetComponent<EnemyAI>().Invoke("DisableEnemy", 0.48f);
        }
        else
        {
            GetComponent<CowEnemyAI>().StopFreezing();
            GetComponent<CowEnemyAI>().isDead = true;
            GetComponent<CowEnemyAI>().Invoke("DisableEnemy", 0.48f);
            
        }

		EnemyManager.Instance.DropCoin(transform, dieGoldAmount);

        CalculateAndDropHp();

        
        healthBar.transform.localPosition = DataHelper.HEALTH_BAR_DEAD;

        //DisableHealthBar();

		

        healthBar.transform.GetChild(2).GetComponent<TweenAlpha>().Toggle();
        healthBar.transform.GetChild(2).GetComponent<TweenPosition>().Toggle();
        healthBar.transform.GetChild(2).GetComponent<UILabel>().text = "x"+dieGoldAmount;

        Invoke("DisableHealthBar", 0.5f);

		EnemyManager.Instance.healthBarsPool.Add (healthBar.gameObject);
        if (EnemyManager.Instance.multiKillTimer == 0)
        {
            
            EnemyManager.Instance.TriggerMultikills();
        }
        EnemyManager.Instance.multikills++;
       
	}

    public void DisableHealthBar()
    {
        healthBar.gameObject.SetActive(false);
    }

    void DieElectric()
    {
        EnemyManager.Instance.AdjustRemainingEnemies(gameObject);

        anim.SetBool("DieElectric", true);
        if (GetComponent<EnemyAI>())
        {
            GetComponent<EnemyAI>().StopFreezing();
            GetComponent<EnemyAI>().isDead = true;
            GetComponent<EnemyAI>().Invoke("DisableEnemy", 0.48f);
        }
        else
        {
            GetComponent<CowEnemyAI>().StopFreezing();
            GetComponent<CowEnemyAI>().isDead = true;
            GetComponent<CowEnemyAI>().Invoke("DisableEnemy", 0.48f);

        }

        EnemyManager.Instance.DropCoin(transform, dieGoldAmount);
        CalculateAndDropHp();

        healthBar.transform.localPosition = DataHelper.HEALTH_BAR_DEAD;
        //healthBar.gameObject.SetActive(false);

        healthBar.transform.GetChild(2).GetComponent<TweenAlpha>().Toggle();
        healthBar.transform.GetChild(2).GetComponent<TweenPosition>().Toggle();
        healthBar.transform.GetChild(2).GetComponent<UILabel>().text = "x" + dieGoldAmount;

        Invoke("DisableHealthBar", 0.7f);

        EnemyManager.Instance.healthBarsPool.Add(healthBar.gameObject);

        if (EnemyManager.Instance.multiKillTimer == 0)
        {
            
            EnemyManager.Instance.TriggerMultikills();
        }
        EnemyManager.Instance.multikills++;

    }

    private void CalculateAndDropHp()
    {
        int dropRatio = Random.Range(1 , (int)(100/DataHelper.HP_DROP_PROPABILITY));

        if (dropRatio == 1)
        {
            EnemyManager.Instance.DropHpGift(transform);
        }
       

    }

    public void Reset()
    {
        
        currentHealth = MaxHealth;

        if (healthBar)
        {
            healthBar.value = currentHealth / MaxHealth;
            healthBar.transform.GetChild(2).GetComponent<TweenAlpha>().ResetToBeginning();
            healthBar.transform.GetChild(2).GetComponent<TweenPosition>().ResetToBeginning();
            healthBar.transform.GetChild(2).GetComponent<UILabel>().alpha = 0;
        }


        transform.GetChild(2).GetComponent<SpriteRenderer>().sortingOrder += Random.Range(1, 5);
    }
}
