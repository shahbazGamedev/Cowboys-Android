using UnityEngine;
using System.Collections;

public class ScreenManager : MonoBehaviour {

    public static ScreenManager Instance;

    public Transform CameraLeftPos;
    public Transform CameraRightPos;

    public Transform HUD_leftRef;
    public Transform HUD_rightRef;
    public Transform HUD_upRef;
    public Transform HUD_downRef;
    public Transform HUD_leftTopCornerRef;

    public Camera UICamera;

    public Transform EnemyEvadesParent;

    public Transform EvadesForKidLeftPos;
    public Transform EvadesForManRightPos;

    public Transform InsideScreenTrigg_LeftPos;
    public Transform InsideScreenTrigg_RightPos;

    public Transform AnimalsRightEscape;
    public Transform AnimalsLeftEscape;

   
   // public Transform coinsCollectLocation;

    void Awake()
    {
        Instance = this;
    }

	void Start () {
       // float yo = HUD_upRef.GetComponent<BoxCollider>().center.y+HUD_upRef.GetComponent<BoxCollider>().size.y/2;
        //Debug.Log(HUD_upRef.localPosition+"  ,  "+yo);
        //Debug.Log(HUD_upRef.localPosition.y - yo);
	}

    public bool CheckScreenBorder()
    {

        Vector3 mouseScreenPos = Input.mousePosition;

        Vector2 leftTopQuarter = new Vector2(UICamera.WorldToScreenPoint(HUD_leftTopCornerRef.position).x, UICamera.WorldToScreenPoint(HUD_leftTopCornerRef.position).y);

        if (IsVector3InQuarter(mouseScreenPos, leftTopQuarter))
        {
            Debug.Log("in left top quarter");
            if (mouseScreenPos.x < UICamera.WorldToScreenPoint(HUD_leftRef.position).x)
            {
                return false;
            }
            if (mouseScreenPos.y > UICamera.WorldToScreenPoint(HUD_leftRef.position).y)
            {
                return false;
            }
            return true;
        }

        if (mouseScreenPos.y > UICamera.WorldToScreenPoint(HUD_upRef.position).y)
        {
            return false;
        }

        if (mouseScreenPos.y < UICamera.WorldToScreenPoint(HUD_downRef.position).y)
        {
            return false;
        }

        if (mouseScreenPos.x < UICamera.WorldToScreenPoint(HUD_leftRef.position).x)
        {
            return false;
        }

        if (mouseScreenPos.x > UICamera.WorldToScreenPoint(HUD_rightRef.position).x)
        {
            return false;
        }

        return true;
    }

    private bool IsVector3InQuarter(Vector3 target, Vector2 quarter)
    {
        return (target.x < quarter.x && target.y > quarter.y);
    }
}
