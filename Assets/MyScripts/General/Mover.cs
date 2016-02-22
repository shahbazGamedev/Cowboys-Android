using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour
{

    //public float Speed;
    public float moveSpeed;

    private ObjectsGenerator OG;

    private float originalMoveSpeed;
    void Start()
    {
        OG = transform.parent.GetComponent<ObjectsGenerator>();
        if (OG)
            moveSpeed = OG.LayerSpeed;

        originalMoveSpeed = moveSpeed;
    }

    void FixedUpdate()
    {
         if (GameMaster.Instance!=null && GameMaster.Instance.isDead)
        {
            moveSpeed = Mathf.Lerp(moveSpeed, 0, 1.8f * Time.deltaTime);
           // return;
        }
         else
         {
             moveSpeed = Mathf.Lerp(moveSpeed, originalMoveSpeed, 1.8f * Time.deltaTime);
         }
        transform.Translate(-moveSpeed * Time.smoothDeltaTime, 0, 0, Space.World);

        if (transform.position.x < -80)
        {
            if (OG)
                OG.Pool.Add(gameObject);
            gameObject.SetActive(false);
        }

    }

    public void SetSpeed(float Speed)
    {
        moveSpeed = Speed;
    }

    void OnDisable()
    {
        moveSpeed = 0;
    }
}
