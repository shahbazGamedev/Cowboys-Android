using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {


	public float startSpeed;
	public float moveSpeed;
	public float evadeSpeed = 3;

	//public float posXToStartEvade = 5.5f;
	public float timeBetweenEvades = 2;
	public float timeToAttack = 7;

	private GameObject player;
	private Animator anim;

	private GameObject [] evadePoints;

    [HideInInspector]
	public bool isDead;

	bool willAttack;
	bool insideScreen;
	Vector3 nextEvadePos;

	bool isKid;
	bool grandFlipped;

	public float damage = 10;
    private float startDamag;

    [HideInInspector]
    public bool isFreezed = false;

    [HideInInspector]
    public bool isAttacking = false;

    public bool isRanged;
    private Transform bulletSpawnPosition;
    public bool isGrounded;

	// Use this for initialization
	void Start () {
		isDead = false;
		player = GameObject.FindGameObjectWithTag("Player");
		anim = gameObject.transform.GetChild(0).GetComponent<Animator>();

		evadePoints = GameObject.FindGameObjectsWithTag("enemyPoints");

        if (isRanged)
            bulletSpawnPosition = transform.GetChild(3);
		//isKid = player.GetComponent<PlayerController>().isKid;

        startSpeed = moveSpeed;
        startDamag = damage;
	}
	
	void FixedUpdate () {

        if (isFreezed)
        {
            CancelInvoke("Evade");
            CancelInvoke("GoToAttack");
            CancelInvoke("GoToEvade");
            //  return;

        }
        else
        {

            if (!isDead)
            {
                if(GameMaster.Instance.isStaticEnvironment)
                    isKid = player.GetComponent<StaticPlayerController>().isKid;
                else
                    isKid = player.GetComponent<DynamicPlayerController>().isKid;
                grandFlipped = GameMaster.Instance.grandFilpped;

                Vector3 direction = transform.position - player.transform.GetChild(0).transform.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                if (isGrounded)
                    angle = 0;

                if (isKid || grandFlipped)
                {
                    if(!isGrounded)
                    angle += 180;
                }

                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                if (((!isKid && transform.position.x <= GameMaster.Instance.X_startEvadePos && transform.position.y <= GameMaster.Instance.Y_startEvadePos)
                    || (isKid && transform.position.x >= GameMaster.Instance.X_startEvadePos && transform.position.y <= GameMaster.Instance.Y_startEvadePos))
                    && !willAttack)
                {

                    insideScreen = true;
                }

                if (insideScreen)
                {
                    if (!IsInvoking("Evade"))
                    {
                        InvokeRepeating("Evade", 0, timeBetweenEvades + Random.Range(0, 2));

                        if(!isRanged)
                            Invoke("GoToAttack", timeToAttack + ((evadeSpeed ==0)? 0 : Random.Range(1, 10)));
                        else
                        {
                            if (!IsInvoking("AttackRanged"))
                            {
                                Invoke("AttackRanged", timeToAttack);
                            }
                        }
                            
                    }
                    if (!isFreezed)
                        transform.position = Vector3.Lerp(transform.position, nextEvadePos, Time.deltaTime * evadeSpeed);

                    //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                }
                else
                {

                    //transform.rotatioj = Quaternion.AngleAxis(angle, Vector3.forward);
                    if (!isKid && !grandFlipped)
                    {
                        transform.Translate(-moveSpeed * Time.deltaTime, 0, 0, Space.Self);
                    }
                    else
                    {
                        transform.Translate(moveSpeed * Time.deltaTime, 0, 0, Space.Self);
                    }
                }


                if (Vector3.Distance(player.transform.GetChild(0).position, transform.position) <= 0.5f && !isAttacking)
                {
                    //Debug.Log("awyan hena");
                    if (!isFreezed)
                    {
                        //Attack();
                        Invoke("GoToEvade", .3f);
                        player.gameObject.SendMessage("ApplyDamage", damage);
                        isAttacking = true;
                    }

                    Debug.Log("attacking");

                    if (evadeSpeed == 0)
                    {
                        moveSpeed = 0;
                        direction = transform.position - player.transform.position;
                    }
                }

                if (isGrounded && Mathf.Abs(player.transform.GetChild(0).position.x - transform.position.x) <= 1f && !isAttacking)
                {
                    if (!isFreezed)
                    {
                        //Attack();
                        Invoke("GoToEvade", .3f);
                        player.gameObject.SendMessage("ApplyDamage", damage);
                        isAttacking = true;
                    }

                    Debug.Log("attacking");

                    if (evadeSpeed == 0)
                    {
                        moveSpeed = 0;
                        direction = transform.position - player.transform.position;
                    }
                }

                if (isKid)
                {
                    Vector3 temp = transform.localScale;
                    temp.x = -1;
                    transform.localScale = temp;

                }
            }
            else
            {
                CancelInvoke("Evade");
                // StopFreezing();
            }
        }
        //if (isBreezed)
        //{
        //    CancelInvoke("Evade");
        //    CancelInvoke("GoToAttack");
        //    CancelInvoke("GoToEvade");
        //  //  return;

        //}

	}

	void Evade(){
       // if (!isFreezed)
        
        {
            Vector3 lastPoint = nextEvadePos;
            while (nextEvadePos == lastPoint)
            {
                nextEvadePos = evadePoints[Random.Range(0, evadePoints.Length)].transform.position - randomPos(0f, 2f);
            }
        }
	}

	private Vector3 randomPos(float f, float t){
		return new Vector3(Random.Range(f,t), Random.Range(f,t), 0 );
	}

	void GoToAttack(){
      //  if (!isFreezed)
        {
            isAttacking = false;
            CancelInvoke("Evade");
            insideScreen = false;
            willAttack = true;
        }
	}

    void AttackRanged()
    {
        if (isFreezed)
        {
            CancelInvoke("Evade");

            return;

        }
        if (isDead)
            return;
        isAttacking = false;
        CancelInvoke("Evade");
        insideScreen = false;
       // willAttack = true;
        Invoke("SpawnBullet", 0.25f);
        

        anim.SetBool("Attack", true);
        Invoke("StopRangedAttack", 0.45f);
    }

    void SpawnBullet()
    {
        
        GameObject bullet = Instantiate(EnemyManager.Instance.rangedEnemyBulletPrefab, bulletSpawnPosition.position, bulletSpawnPosition.rotation) as GameObject;
        
        bullet.GetComponent<BulletMoverRangedEnemy>().damage = damage;
        if (isKid)
        {
            bullet.transform.right = -1 * bulletSpawnPosition.right;
        }

    }
    void StopRangedAttack()
    {
        anim.SetBool("Attack", false);
    }

	void GoToEvade(){
       // if (!isFreezed)
        {
            GetComponent<Collider2D>().enabled = true;
            insideScreen = true;
        }
	}


	void Attack(){

       // if (!isFreezed) 
        { 

		    Invoke("GoToEvade", 1);
        }
	}

	void AtackWithExplode(){

		Invoke("DisableEnemy", 1f);
	}

    /*
	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == "Player"){
			coll.gameObject.SendMessage("ApplyDamage", damage);
		}

		GetComponent<Collider2D>().enabled = false;
		Debug.Log ("hoy hoy");
	}

	void OnTriggerEnter2D(Collider2D coll) {
		if (coll.gameObject.tag == "Player"){
			coll.gameObject.SendMessage("ApplyDamage", damage);
		}
		
		Debug.Log ("hoy hoy");
	}
    */
	public void DisableEnemy(){
		gameObject.SetActive(false);
	}

    public void FreezEnemy()
    {
        isFreezed = true;
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(2).gameObject.SetActive(true);

        Invoke("StopFreezing", DataHelper.FREEZE_TIME);

    }

    public void StopFreezing()
    {
        isFreezed = false;
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(2).gameObject.SetActive(false);
    }

    public void Reset()
    {
        isDead = false;
        isFreezed = false;
        isAttacking = false;

        moveSpeed = startSpeed;
        damage = startDamag;

        CancelInvoke("Evade");
        CancelInvoke("GoToAttack");
        CancelInvoke("GoToEvade");
    }
}
