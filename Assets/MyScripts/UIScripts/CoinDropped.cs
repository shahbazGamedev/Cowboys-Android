using UnityEngine;
using System.Collections;

public class CoinDropped : MonoBehaviour
{


    public int goldAmount = 1;

    public LayerMask mask;
    //public LayerMask enemiesMask;

    bool isTaken;

    void Start()
    {
        

        Invoke("Disable", DataHelper.DISABLE_COIN_DELAY);
    }

    void TakeCoin()
    {
        isTaken = true;
        //GameMaster.Instance.AdjustGold(goldAmount);
        SoundManager.PlaySFX("Coins_Collected");
    }

    public void Disable()
    {
        if (IsInvoking("Disable"))
            CancelInvoke("Disable");
        GameMaster.Instance.AdjustGold(goldAmount);
        gameObject.SetActive(false);
       

    }

    void Update()
    {
        if (transform.position.x > 4)
        {
            transform.position = new Vector3(4, transform.position.y, transform.position.z);
        }

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
                    TakeCoin();

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
