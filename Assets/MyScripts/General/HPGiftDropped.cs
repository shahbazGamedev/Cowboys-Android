using UnityEngine;
using System.Collections;

public class HPGiftDropped : MonoBehaviour {
   
    public LayerMask mask;

    bool isTaken;

    void Start()
    {


        Invoke("DisableNotTaken", DataHelper.DISABLE_HPGIFT_DELAY);
    }

    void TakeHp()
    {
        isTaken = true;
    }

    public void Disable()
    {
        GameMaster.Instance.TakeHpStar();
        gameObject.SetActive(false);


    }

    void DisableNotTaken()
    {
        gameObject.SetActive(false);
    }

    void Update()
    {

        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Enemies"), true);

        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Coins"), true);

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = -10f;

            Vector2 v = Camera.main.ScreenToWorldPoint(mousePosition);

            Collider2D[] col = Physics2D.OverlapPointAll(v, mask.value);



            if (col.Length > 0)
            {
                if (col[0].transform == transform)
                    TakeHp();

            }
        }

        if (isTaken)
        {
            GetComponent<Animator>().SetBool("Collect", true);
            GetComponent<Collider2D>().enabled = false;
            GetComponent<Rigidbody2D>().gravityScale = 0;
            GetComponent<Rigidbody2D>().isKinematic = true;

            Invoke("Disable", .65f);
            isTaken = false;



        }

    }
}
