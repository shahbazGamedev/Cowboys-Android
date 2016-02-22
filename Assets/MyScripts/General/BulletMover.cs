using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class BulletMover : MonoBehaviour {


	//public Vector2 Direction;
	public float bulletSpeed = 5;
	public float damage = 80;

	public Animator anim;
	bool disabled;

	public bool isBomb;
	public bool isDynamite;
    public bool isMolotove;
    public bool isIceBomb;

	public float dynamiteRadius = 7;
	//public float bombExplosionTime = 0.5f;

	public Collider2D[] NearestHits;

	GameObject [] EnemiesArea;

    private Vector3 Destination;

    public List<Transform> brothers = new List<Transform>();

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		Destroy (gameObject, 3);




		//NearestHits = new Collider2D[];
		//EnemiesArea = new List<GameObject> ();
	}


    public void SetDestination(Vector3 mousePosition)
    {
        Destination = mousePosition;

        if (!isBomb && !isIceBomb && !isDynamite && !isMolotove)
        {
            DisableColliders();
        }

    }

    private void DisableColliders()
    {
        GetComponent<Collider2D>().enabled = false;
        Collider2D[] childrenColliders = GetComponentsInChildren<Collider2D>();
        foreach (Collider2D coll in childrenColliders)
        {
            coll.enabled = false;
        }
    }

    private void EnableColliders()
    {
        GetComponent<Collider2D>().enabled = true;
        Collider2D[] childrenColliders = GetComponentsInChildren<Collider2D>();
        foreach (Collider2D coll in childrenColliders)
        {
            coll.enabled = true;
        }
    }


	// Update is called once per frame
	void Update () {
		//GetComponent<Rigidbody2D>().AddForce (transform.right * bulletSpeed * Time.timeScale);

        if (Destination != Vector3.zero && Vector3.Distance(transform.position, Destination) <= .5f && !disabled)
        {
            EnableColliders();
            DisableHittingBullet();
            //DisableBullet();
        }


        

		if(!disabled)
			GetComponent<Rigidbody2D>().velocity = transform.right * bulletSpeed ;
		else{
			GetComponent<Rigidbody2D>().velocity = Vector3.zero;
		}

	}

    
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.tag == "Enemy")
        {

            if (isBomb)
            {
                Explode();
            }
            if (isIceBomb)
            {
                disabled = true;
                GetComponent<SpriteRenderer>().enabled = false;
                Freeze_Enemies();
            }
            if (isDynamite && !disabled)
            {
                NearestHits = Physics2D.OverlapCircleAll(transform.position, dynamiteRadius);
                disabled = true;
                Kill_3_Enemies();
            }
            if (isMolotove && !disabled)
            {
                NearestHits = Physics2D.OverlapCircleAll(transform.position, dynamiteRadius);
                disabled = true;
                GetComponent<SpriteRenderer>().enabled = false;
                Kill_4_Enemies();
            }
            else
            {
                other.gameObject.SendMessage("ApplyDamage", damage);
                
                DisableBullet();
            }
        }

        //Debug.Log("i hit something");
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {

            if (isBomb)
            {
                Explode();
            }
            if (isIceBomb)
            {
                disabled = true;
                GetComponent<SpriteRenderer>().enabled = false;
                Freeze_Enemies();
            }
            if (isDynamite && !disabled)
            {
                NearestHits = Physics2D.OverlapCircleAll(transform.position, dynamiteRadius);
                disabled = true;
                Kill_3_Enemies();
            }
            if (isMolotove && !disabled)
            {
                NearestHits = Physics2D.OverlapCircleAll(transform.position, dynamiteRadius);
                disabled = true;
                GetComponent<SpriteRenderer>().enabled = false;
                Kill_4_Enemies();
            }
            else
            {
                other.gameObject.SendMessage("ApplyDamage", damage);

                DisableBullet();
            }
        }

        //Debug.Log("i hit something");
    }

    void DisableBullet()
    {
        if (anim)
        {
            
            anim.SetBool("hit", true);
            disabled = true;
            //bulletSpeed = 0;

            GetComponent<Collider2D>().enabled = false;

            Invoke("DestroyAfter", .6f);
        }
        else if (GetComponentInChildren<Animator>())
        {
            GetComponentInChildren<Animator>().SetBool("hit", true);
            disabled = true;
            GetComponent<Collider2D>().enabled = false;

            Invoke("DestroyAfter", .6f);
        }
        else
        {
            Destroy(gameObject);
        }
            
    }

    public void DisableHittingBullet()
    {
        if (anim)
        {
            Debug.Log("hmmm " + gameObject.name);
            anim.SetBool("hit", true);
            disabled = true;
            Invoke("DestroyAfter", .6f);

            //for the bottle

            foreach (Transform bro in brothers)
            {
                bro.GetComponent<BulletMover>().DisableHittingBullet();
            }
        }
        else if (GetComponentInChildren<Animator>())
        {
            GetComponentInChildren<Animator>().SetBool("hit", true);
            disabled = true;
            Invoke("DestroyAfter", .6f);


            //for the arrowww
            foreach (Transform bro in brothers)
            {
                bro.GetComponent<BulletMover>().DisableHittingBullet();
            }
        }
        else
        {
            Debug.Log("this will look weird");
            Destroy(gameObject);
        }
    }

    /*
	void OnTriggerEnter2D(Collider2D other){
		if(other.tag == "Enemy"){

			if(isBomb){
				Explode();
			}
            if (isIceBomb)
            {
                disabled = true;
                GetComponent<SpriteRenderer>().enabled = false;
                Freeze_Enemies();
            }
			if(isDynamite && !disabled){
				NearestHits = Physics2D.OverlapCircleAll(transform.position, dynamiteRadius);
				disabled = true;
				Kill_3_Enemies();
			}
            if (isMolotove && !disabled){
                NearestHits = Physics2D.OverlapCircleAll(transform.position, dynamiteRadius);
                disabled = true;
                GetComponent<SpriteRenderer>().enabled = false;
                Kill_4_Enemies();
            }
            else
            {
                other.gameObject.SendMessage("ApplyDamage", damage);
                if (anim)
                {
                    anim.SetBool("hit", true);
                    disabled = true;
                    //bulletSpeed = 0;

                    GetComponent<Collider2D>().enabled = false;

                    Invoke("DestroyAfter", .6f);
                }
                else
                    Destroy(gameObject);
            }
		}

		//Debug.Log("i hit something");
	}
     * */

	void Explode(){
		anim.SetBool("hit", true);
		EnemiesArea = GameObject.FindGameObjectsWithTag ("Enemy");

        int enemiesNum = EnemiesArea.Length;

		for(int i=0; i<EnemiesArea.Length; i++){

            if (EnemiesArea[i].activeSelf)
			    EnemiesArea[i].SendMessage("ApplyDamage", damage);

			//

			disabled = true;
		}

		Invoke ("ReScale", .25f);

       // EnemyManager.Instance.AllKillComment();

		GetComponent<SpriteRenderer> ().enabled = false;
		Invoke("DestroyAfter", 0.846f);
	}

	void Kill_3_Enemies(){
		//Debug.Log ("blkaaaaa");
		anim.SetBool("hit", true);
		int enemyCounter = 0;

		//Transform [] closestThree = GetClosestHits(3);
		for(int i=0; i<NearestHits.Length; i++){
			if(NearestHits[i].tag == "Enemy"){
			
				NearestHits[i].gameObject.SendMessage("ApplyDamage", damage);
				enemyCounter++;
			}

			if(enemyCounter == 3)
				break;
			

		}

       // EnemyManager.Instance.TripleKillComment();

		GetComponent<Collider2D> ().enabled = false;
		Invoke ("ReScaleDynamite", .25f);
		
		
		GetComponent<SpriteRenderer> ().enabled = false;
		Invoke("DestroyAfter", 0.846f);
	}

    void Kill_4_Enemies()
    {
        //Debug.Log ("blkaaaaa");
       // anim.SetBool("hit", true);
        int enemyCounter = 0;

        //Transform [] closestThree = GetClosestHits(3);
        for (int i = 0; i < NearestHits.Length; i++)
        {
            if (NearestHits[i].tag == "Enemy")
            {

                NearestHits[i].gameObject.SendMessage("ApplyDamagePerSecond");
                enemyCounter++;
            }

            if (enemyCounter == 4)
                break;


        }

       // EnemyManager.Instance.DoubleKillComment();

        GetComponent<Collider2D>().enabled = false;

        GetComponent<SpriteRenderer>().enabled = false;
        DestroyAfter();
       // Invoke("DestroyAfter", 0.846f);
    }

    void Freeze_Enemies()
    {
        EnemiesArea = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < EnemiesArea.Length; i++)
        {
            if(EnemiesArea[i].activeSelf)
                EnemiesArea[i].SendMessage("FreezEnemy");

        }

       // Invoke("ReScale", .25f);


        GetComponent<SpriteRenderer>().enabled = false;
        //Invoke("DestroyAfter", 0.846f);
        Destroy(gameObject);
    }
	void GetClosestHits(int number){
		float [] dists = new float[NearestHits.Length] ;
		for(int i=0; i<NearestHits.Length; i++){
			dists[i] = Vector3.Distance(NearestHits[i].transform.position, transform.position);
		}

		//float 

	}

	void ReScale(){
		transform.position = transform.position + new Vector3 (.3f, 0,0);
		transform.localScale = new Vector3(1.5f, 1.5f, 1);
		//transform.position = transform.position + new Vector3 (2, 0,0);
		GetComponent<SpriteRenderer> ().enabled = true;
		disabled = true;
	}

	void ReScaleDynamite(){
		transform.position = transform.position + new Vector3 (1.5f, .5f,0);
		transform.localScale = new Vector3(1.1f, 1.1f, 1);
		//transform.position = transform.position + new Vector3 (2, 0,0);
		GetComponent<SpriteRenderer> ().enabled = true;
		//disabled = true;
	}

	void DestroyAfter(){
		Destroy (gameObject);
	}
	
}
