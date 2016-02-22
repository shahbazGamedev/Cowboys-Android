using UnityEngine;
using System.Collections;

public class StaticPlayerController : MonoBehaviour {

	//public float jumpForce = 600f;
	public Transform groundCheck;
	float groundRadius = 0.2f;
	public LayerMask whatIsGround;

	bool grounded = false;

	//public Animator anim;
	private bool canJump = true;

	private bool canShoot;

    public Transform bulletSpawn;

	public Animator manAnim;
	public Animator ManBodyAnim;

	public GameObject Man;
	public GameObject ManBody;
	public GameObject ManSplitted;

	private Quaternion originalRotation;

	private bool resetRotation;

	private Weapons weaponsScript;
	
	public bool isKid;
	public GameObject Kid;
	public Animator kidAnim;

	private Quaternion kidOriginalRotation;

	public float kidPosX;
	public float switchToKidSpeed = 5;

	bool dead;

	// Use this for initialization
	void Start () {
		canShoot = true;
		originalRotation = Man.transform.rotation;
        //Kiddd
		if(Kid)
			kidOriginalRotation = Kid.transform.rotation;

		weaponsScript = GetComponent<Weapons>();
	}

	void FixedUpdate(){
		grounded = Physics2D.OverlapCircle (groundCheck.position, groundRadius, whatIsGround);
	}
	
	// Update is called once per frame
	void Update () {


		if(!isKid){
            if(GameMaster.Instance.inputEnabled){
			    Attack();

			}

			if(resetRotation){
//				Debug.Log("hello");
				Man.transform.rotation = Quaternion.Slerp(Man.transform.rotation, originalRotation, Time.deltaTime * 10);
			}
		}
		else{
			KidAttack();
			if(resetRotation){
				//Debug.Log("hellokid");
				Kid.transform.rotation = Quaternion.Slerp(Kid.transform.rotation, kidOriginalRotation, Time.deltaTime * 10);
			}
		}

		/*
		if(Man.transform.rotation.z < -20){
			Quaternion rot = Man.transform.rotation;
			rot.z = 20;
			Man.transform.rotation = rot;
		}
*/
		if(isKid){
			
		}


	}
    /*
	public vkid Jump(){
		if(grounded && canJump){
			GetComponent<Rigidbody2D>().AddForce (new Vector2(0,jumpForce));
			anim.SetBool("Jump", true);
			AllTogetherAnim.SetBool("Jump", true);
			canJump = false;
			Invoke("ChangeCanJump", 1);
			
			Invoke("EnableAll_DisableRest", .05f);
			
			
			Invoke("ResumeMan_DisableAll", 0.9f);
			//canJump = false;
		}
		else{
			//jump = false;
			anim.SetBool("Jump", false);
			AllTogetherAnim.SetBool("Jump", false);
		}

	}
    */
    void Attack()
    {
		if(Input.GetMouseButtonDown(0)  && canShoot /*&& !manAnim.IsInTransition(0) && grounded*/){
			if(ScreenManager.Instance.CheckScreenBorder()){
                if (Weapons_PUs_Manager.Instance.currentBulletsCounts[weaponsScript.currentWeapon] > 0)
                {
                    resetRotation = false;
                    CancelInvoke("ResetRotation");


                    Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Vector3 direction = mousePos - bulletSpawn.transform.position;

                    direction.Normalize();

                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                    //			Debug.Log(angle);

                    if (angle > 60)
                    {
                        angle = 60;
                    }
                    if (angle < 0)
                    {
                        angle = 0;
                    }


                    if (manAnim.IsInTransition(0))
                        return;
                    weaponsScript.WeaponShootAnimation(manAnim, true);

                    if (manAnim.IsInTransition(0))
                        return;
                    Man.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);


                    canShoot = false;

                    weaponsScript.Invoke("SpawnManShooting", Weapons_PUs_Manager.Instance.SpawnDelayes[weaponsScript.currentWeapon]);

                    Invoke("EnableShooting", Weapons_PUs_Manager.Instance.ReshootDelayes[weaponsScript.currentWeapon]);
                    Invoke("ResetRotation", Weapons_PUs_Manager.Instance.ReshootDelayes[weaponsScript.currentWeapon] + .3f);
                }	
                else
                {
                    Invoke("EnableShooting", 0.2f);
                    Invoke("ResetRotation", 0.05f);
                    GameObject.FindGameObjectWithTag("defaultWeapon").GetComponent<Button_SwitchWeapon>().SwitchWeapon();
                }
			}
		}
		else{
			weaponsScript.WeaponShootAnimation(manAnim, false);
		}
	}

 
	void KidAttack(){
		if(Input.GetMouseButtonDown(0)  && canShoot && !manAnim.IsInTransition(0)){
			if(ScreenManager.Instance.CheckScreenBorder()){
				resetRotation = false;
				CancelInvoke("ResetRotation");
				
				
				Vector3 mousePos =  Camera.main.ScreenToWorldPoint(Input.mousePosition);
				Vector3 direction =  bulletSpawn.transform.position - mousePos;
				float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
				
				//			Debug.Log(angle);


				if(angle < -80){
					angle = -80;
				}
				if(angle > 0){
					angle = 0;
				}


				Kid.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
				
				weaponsScript.WeaponShootAnimation(kidAnim, true);
				
				canShoot = false;

                weaponsScript.Invoke("SpawnKidShooting", Weapons_PUs_Manager.Instance.SpawnDelayes[weaponsScript.currentWeapon]);
				//Invoke("SpawnBullet", .1f);
                Invoke("EnableShooting", Weapons_PUs_Manager.Instance.ReshootDelayes[weaponsScript.currentWeapon]);
                Invoke("ResetRotation", Weapons_PUs_Manager.Instance.ReshootDelayes[weaponsScript.currentWeapon] + .3f);
			}
		}
		else{
			weaponsScript.WeaponShootAnimation(kidAnim, false);
		}
	}

    public void Revive()
    {
        canJump = true;
        canShoot = true;
        dead = false;

        if (!isKid)
        { 
            EnableAll_DisableRest();
            ManBodyAnim.SetBool("Revive", true);
            Invoke("ResumeMan_DisableAll", 1f);
        }
        if (kidAnim)
        {
            kidAnim.SetBool("Revive", true);
            Invoke("DisableKidRevive", 0.7f);
        }

        
    }

    void DisableKidRevive()
    {
        if (kidAnim)
        {
            kidAnim.SetBool("Revive", false);
            kidAnim.SetBool("Die", false);
        }
    }

	public void Die(){
		canJump = false;
		canShoot = false;
		dead = true;

        if(!isKid)
		    EnableAll_DisableRest();
		//ManBodyAnim.SetBool("Die", true);

        if (kidAnim) { 
            kidAnim.SetBool("Die", true);
            
        }


		//Invoke("EnableAll_DisableRest", .05f);
		GameMaster.Instance.Lose();
	}
	
    
	void ResumeMan_DisableAll(){
		if(!dead){
			ManSplitted.SetActive(true);
			ManBody.SetActive(false);
		}
	}
    

	void EnableAll_DisableRest(){
		ManSplitted.SetActive(false);
		ManBody.SetActive(true);
	}
	public void EnableShooting(){
		canShoot = true;
	}

	public void ResetRotation(){
		resetRotation = true;
	}

	void ChangeCanJump(){
		canJump = true;
	}

	public void SwitchToKid(){
		isKid = true;

	}


    //public void FlipToRight(){
    //    Vector3 temp = Man.transform.localScale;
    //    temp.x *= -1;
    //    Man.transform.localScale = temp;

    //    temp = Man.transform.GetChild(0).localScale;
    //    temp.x *= -1;
    //    Man.transform.GetChild(0).localScale = temp;

    //    temp = Man_lowerBody.transform.localScale;
    //    temp.x *= -1;
    //    Man_lowerBody.transform.localScale = temp;

    //    temp = Alltogether.transform.localScale;
    //    temp.x *= -1;
    //    Alltogether.transform.localScale = temp;

    //}
}
