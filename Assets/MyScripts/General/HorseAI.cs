using UnityEngine;
using System.Collections;

public class HorseAI : MonoBehaviour {
  public bool isAttacked;

    public bool reserved;

    Animator anim;

    float MIN_Idle_Multip = 0.8f;
    float MAX_Idle_Multip = 1.1f;

    //float MIN_stopIdle_Multip = .03f;
    //float MAX_stopIdle_Multip = .5f;

    public Vector3 originalPos;
    private Transform originalParent;

    bool isReleased;



    void Start()
    {
        // GetComponent<Collider2D>().isTrigger = false;
        originalParent = transform.parent;
        originalPos = transform.position;

        anim = GetComponent<Animator>();

        anim.SetFloat("Run_Multip", Random.Range(MIN_Idle_Multip, MAX_Idle_Multip));
        //anim.SetFloat("stopIdle_Multip", Random.Range(MIN_stopIdle_Multip, MAX_stopIdle_Multip));

        //ChangeMultipliers();
        //InvokeRepeating("ChangeMultipliers", 1, 3f);
    }


    void ChangeMultipliers()
    {
        anim.SetFloat("Run_Multip", Random.Range(MIN_Idle_Multip, MAX_Idle_Multip));
        //anim.SetFloat("stopIdle_Multip", Random.Range(MIN_stopIdle_Multip, MAX_stopIdle_Multip));
    }

    public void PickCow()
    {
        anim.SetBool("fly", true);
        // GetComponent<Collider2D>().isTrigger = false;
    }

    public void ReleaseCow()
    {
        transform.parent = originalParent;
        //transform.localPosition = new Vector3(transform.localPosition.x, originalPos.y, 1);
        //GetComponent<Collider2D>().isTrigger = false;

        isReleased = true;
    }

    void Update()
    {
        if (isReleased)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, originalPos.y, 1), 4 * Time.deltaTime);

            if (transform.position.y <= originalPos.y + .1f)
            {
                transform.position = new Vector3(transform.position.x, originalPos.y, 1);
                anim.SetBool("fly", false);
                isAttacked = false;
                isReleased = false;
                transform.rotation = Quaternion.identity;
                reserved = false;
            }
        }
    }

    void OnDisable()
    {
        //CancelInvoke("ChangeMultipliers");
    }

}
