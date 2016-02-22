using UnityEngine;
using System.Collections;

public class BulletMoverRangedEnemy : MonoBehaviour {

    public float bulletSpeed = 5;
    public float damage = 80;
    public Animator anim;

    bool disabled = false;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if(!disabled)
            GetComponent<Rigidbody2D>().velocity = transform.right * bulletSpeed;
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            other.SendMessage("ApplyDamage", damage);
            DisableBullet();
            bulletSpeed = 0;
        }
    }

    void DisableBullet()
    {
        if (anim)
        {

            anim.SetBool("hit", true);
            transform.localScale = new Vector3(1f, 1f, 1);
            GetComponent<Collider2D>().enabled = false;
            
            Invoke("DestroyAfter", .6f);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void DestroyAfter()
    {
        Destroy(gameObject);
    }
}
