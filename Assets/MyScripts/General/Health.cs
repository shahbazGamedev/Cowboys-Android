using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {

	public float MaxHealth = 100;
	public float currentHealth = 100;

    public float shieldHealth = 0;

	public UISlider healthBar;
    public SpriteRenderer ShieldSprite;
    public UISlider shieldBar;

	// Use this for initialization
	void Start () {
		if(healthBar){
			healthBar.value = currentHealth/MaxHealth;
		}

        if (transform.GetChild(transform.childCount - 1).name == "Shield")
        {
            ShieldSprite = transform.GetChild(transform.childCount - 1).GetComponent<SpriteRenderer>();
            
        }

        //RestoreShield();
	}
	
	// Update is called once per frame
	public void ApplyDamage (float amount) {

        if (shieldHealth > 0)
        {
            float amountDiff = shieldHealth - amount;
            shieldHealth -= amount;
            if (shieldHealth <= 0)
            {
                shieldHealth = 0;
            }
            UpdateShieldSprite();

            if (amountDiff >= 0)
            {
                return;
            }
            else
                amount -= amountDiff;
        }
        else
        {
            GameMaster.Instance.AdjustGold(-1 * DataHelper.COINS_LOSE_POINTS);
            GameMaster.Instance.DisplayDroppedCoins();
        }

		currentHealth -= amount;

		if(currentHealth <= 0){
			currentHealth = 0;


            if(GameMaster.Instance.isStaticEnvironment)
                GetComponent<StaticPlayerController>().Die();
            else
                GetComponent<DynamicPlayerController>().Die();


		}

       

		if(healthBar){
			healthBar.value =  (float)( currentHealth/MaxHealth);
		}
        GameMaster.Instance.RankWithPlayerHealth = false;
	}

    public void AddHp(float amount)
    {
        currentHealth += amount;

        if (currentHealth >= MaxHealth)
        {
            currentHealth = MaxHealth;

        }

        if (healthBar)
        {
            healthBar.value = (float)(currentHealth / MaxHealth);
        }
    }

    public void RestoreHealth()
    {
        currentHealth = MaxHealth;

        if (healthBar)
        {
            healthBar.value = currentHealth / MaxHealth;
        }

        if (GameMaster.Instance.isStaticEnvironment)
            GetComponent<StaticPlayerController>().Revive();
        else
            GetComponent<DynamicPlayerController>().Revive();

    }

    private void UpdateShieldSprite()
    {
        Color c = ShieldSprite.color;
        c.a = (float)(shieldHealth/DataHelper.SHIELD_HEALTH);
       // Debug.Log("c= " + c.a);
        ShieldSprite.color = c;

        shieldBar.value = shieldHealth / DataHelper.SHIELD_HEALTH;
    }

    float secs;
    float rate = .07f;

    public void RestoreShield()
    {
        if (IsInvoking("ReduceShieldValue"))
            CancelInvoke("ReduceShieldValue");

        shieldHealth = DataHelper.SHIELD_HEALTH;
        UpdateShieldSprite();

        shieldBar.gameObject.SetActive(true);
        shieldBar.value = 1;

       // secs = DataHelper.SHIELD_TIME;
        InvokeRepeating("ReduceShieldValue", 0, rate);
    }

    void ReduceShieldValue()
    {
       // secs -= rate;

        shieldHealth -= DataHelper.SHIELD_HEALTH / DataHelper.SHIELD_TIME * rate;
        UpdateShieldSprite();

        if (shieldHealth <= 0)
        {
            shieldHealth = 0;
            CancelInvoke("ReduceShieldValue");
            shieldBar.gameObject.SetActive(false);
        }

        shieldBar.value = shieldHealth / DataHelper.SHIELD_HEALTH;
    }
}
