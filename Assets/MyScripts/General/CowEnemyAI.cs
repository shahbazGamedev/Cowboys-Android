using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CowEnemyAI : MonoBehaviour {


	public float startSpeed;
	public float moveSpeed;
	public float evadeSpeed = 3;

	//public float posXToStartEvade = 5.5f;
	public float timeBetweenEvades = 2;
	public float timeToAttack = 7;

	private GameObject player;
	private Animator anim;

	private GameObject [] evadePoints;

	public bool isDead;

	bool willAttack;
	bool insideScreen;
	Vector3 nextEvadePos;

	bool isKid;

	public float damage = 10;

    public bool isFreezed = false;

    private Transform currentTarget;

    bool willAbuse;

    Vector3 direction;

    Vector3 attackDirection;

    bool willAttackCow;

    public Transform currentHoldCow;

    public float escape_speed = .6f;

    public bool hasEscaped;

	// Use this for initialization
	void Start () {
		isDead = false;
		player = GameObject.FindGameObjectWithTag("Player");
		anim = gameObject.transform.GetChild(0).GetComponent<Animator>();

		evadePoints = GameObject.FindGameObjectsWithTag("enemyPoints");

      //  InvokeRepeating("SelectAnimalOrPlayer", 3.5f, timeBetweenEvades + 1);
		//isKid = player.GetComponent<PlayerController>().isKid;

        currentTarget = player.transform;
        attackDirection = direction;
        //currentTarget = AnimalsManager.Instance.PickRightRandomCow();
       // direction = transform.position - currentTarget.position;
	}
	
	void FixedUpdate () {
        if(!willAttackCow)
            attackDirection = direction;
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

                isKid = player.GetComponent<StaticPlayerController>().isKid;

                if (!isKid)
                {
                    direction = transform.position - player.transform.position;
                }
                else
                    direction = transform.position - player.transform.FindChild("Kid").position;

               // direction = transform.position - player.transform.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                if (isKid)
                {
                    angle += 180;
                }

                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                
                //this if works only at the first of enemy appearing inside screen
                if (((!isKid  && transform.position.x <= GameMaster.Instance.X_startEvadePos) ||
                    ((isKid) && transform.position.x >= GameMaster.Instance.X_startEvadePos)) && !willAbuse && !willAttack)
                {
                   // Debug.Log("gatko neela");
                    insideScreen = true;
                }


                if (insideScreen)
                {
                    if (!IsInvoking("Evade") && !currentHoldCow)
                    {
                        
                        InvokeRepeating("Evade", 0, timeBetweenEvades + Random.Range(0, 2));
                        int num = Random.Range(0, 6);
                        ////
                        //num = 0;
                        if (num == 0)
                        {
                            currentTarget = player.transform;
                            willAttackCow = false;
                            Invoke("GoToAttack", timeToAttack + Random.Range(1, 10));
                        }
                        else if(!willAttackCow)
                        {
                            willAttackCow = true;

                            if (AnimalsManager.Instance)
                            {
                                if (!isKid)
                                    currentTarget = AnimalsManager.Instance.PickRightRandomCow();
                                else
                                    currentTarget = AnimalsManager.Instance.PickLeftRandomCow();
                            }

                            if (!currentTarget)
                            {
                                currentTarget = player.transform;
                                willAttackCow = false;
                                Invoke("GoToAttack", timeToAttack + Random.Range(1, 10));
                            }
                            else
                            {
                                Invoke("GoToPickCow", timeToAttack + Random.Range(1, 10));
                            }

                        }
                      //  Invoke("GoToAttack", timeToAttack + Random.Range(1, 10));
                        
                    }
                    if (!isFreezed)
                        transform.position = Vector3.Lerp(transform.position, nextEvadePos, Time.deltaTime * evadeSpeed);

                    //transfori.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                }
                else
                {
                   
                   
                        transform.Translate(-moveSpeed * Time.deltaTime * attackDirection.normalized, Space.World);
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
               // currentHoldCow.SendMessage("ReleaseCow");
                CancelInvoke("Evade");
                // StopFreezing();
            }
        }

        if (currentTarget.tag == "Player" && Vector3.Distance(currentTarget.transform.GetChild(0).position, transform.position) <= 1.5f && (willAbuse || willAttack))
        {
            currentTarget.SendMessage("ApplyDamage", damage);
            Invoke("GoToEvade", .3f);
        }

        if (currentTarget.tag == "Cow" && Vector3.Distance(currentTarget.transform.position, transform.position) <= 0.2f && (willAbuse|| willAttack))
        {
            if (!currentHoldCow)
            {
               // Debug.Log(currentTarget.name + " ,, " + name);

                if (currentTarget.transform == player.transform)
                    return;
                if (currentTarget.transform.GetComponent<CowAI>().isAttacked)
                    return;

                currentTarget.GetComponent<CowAI>().isAttacked = true;
                currentHoldCow = currentTarget.transform;

                currentHoldCow.gameObject.SendMessage("PickCow");


                //attackDirection = transform.position - currentHoldCow.parent.GetChild(0).transform.position;
                attackDirection = transform.position - GameMaster.Instance.AnimalsEscapePoint.position;


                //Debug.DrawRay(transform.position, attackDirection, Color.red);

                currentTarget.parent = transform;

                currentTarget.transform.localPosition = new Vector3(0, -.5f, 0);

                //el mafrod yeb2a code andaf mn keda

                moveSpeed = escape_speed;
                // direction = attackDirection;

            }
            Debug.Log("hoy hoy");
        }

        //is escaping

        if (Vector3.Distance(transform.position, GameMaster.Instance.AnimalsEscapePoint.position) <= 2)
        {
            GetComponent<Collider2D>().enabled = false;
            if (Vector3.Distance(transform.position, GameMaster.Instance.AnimalsEscapePoint.position) <= .1f)
            {
                if (AnimalsManager.Instance)
                {
                    if (!isKid)
                        AnimalsManager.Instance.TerminateRightCow(currentHoldCow);
                    else
                        AnimalsManager.Instance.TerminateLeftCow(currentHoldCow);
                }
               
                hasEscaped = true;
                DisableEnemy();
            }
        }

        Debug.DrawRay(transform.position, attackDirection, Color.red);
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
            attackDirection = direction;
            CancelInvoke("Evade");
            insideScreen = false;
            willAttack = true;

        }
	}

    void GoToPickCow()
    {
        
        //currentTarget = AnimalsManager.Instance.PickRightRandomCow();

       
        
        attackDirection = transform.position - currentTarget.position;
        if (attackDirection.x < 0)
        {
            Vector3 temp = attackDirection;
            temp.x *= -1;
           // attackDirection = temp;
        }

        CancelInvoke("Evade");
        insideScreen = false;
        willAbuse = true;
        //cqrrentTarget.GetComponent<Collider2D>().isTrigger = true;


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

		    Invoke("GoToEvade", .3f);
        }
	}

	void AtackWithExplode(){

		Invoke("DisableEnemy", 1f);
	}


    void OnTriggerEnter2D(Collider2D coll)
    {

        if (coll.gameObject.tag == "Player")
        {
            coll.gameObject.SendMessage("ApplyDamage", damage);
            Invoke("GoToEvade", .3f);
        }
        //else if (coll.gameObject.tag == "Cow" && !currentHoldCow)
        //{
        //    Debug.Log(coll.name + " ,, " + name);


        //    if (coll.transform == player.transform)
        //        return;
        //    if (coll.transform.GetComponent<CowAI>().isAttacked)
        //        return;

        //    currentTarget.GetComponent<CowAI>().isAttacked = true;
        //    currentHoldCow = coll.transform;

        //    currentHoldCow.gameObject.SendMessage("PickCow");


        //    attackDirection = transform.position - currentHoldCow.parent.GetChild(0).transform.position;

        //    //Debug.DrawRay(transform.position, attackDirection, Color.red);

        //    currentTarget.parent = transform;

        //    currentTarget.transform.localPosition = new Vector3(0, -.4f, 0);

        //    //el mafrod yeb2a code andaf mn keda

        //    moveSpeed = 0.6f;
        //    // direction = attackDirection;

        //}


        Debug.Log("hoy hoy");
    }

 

	public void DisableEnemy(){
        if (currentHoldCow && !hasEscaped) 
            currentHoldCow.SendMessage("ReleaseCow");

        if (hasEscaped)
        {
            EnemyManager.Instance.AdjustRemainingEnemies(gameObject);
        }
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
        //if(currentHoldCow)
          //  currentHoldCow.SendMessage("ReleaseCow");

        isFreezed = false;
        transform.GetChild(0).gameObject.SetActive(true);
        if(transform.GetChild(2).name.Substring(0, 3) == "fre")
            transform.GetChild(2).gameObject.SetActive(false);

        
    }
}
