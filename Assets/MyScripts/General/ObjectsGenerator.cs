using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectsGenerator : MonoBehaviour
{

    public float LayerSpeed;

    public List<GameObject> Objects;
    public float repeatRateMin;
    public float repeatRateMax;

    public List<GameObject> Pool;

    //public Transfori SpawnPos;

    public Transform[] SpawnPoses;

    public bool systematicLayer;
    public bool reversed;
    public Transform nextSpawn;

    // Use this for initialization
    void Start()
    {
        if (systematicLayer)
        {
            Invoke("GenerateSystematicObjects", repeatRateMax);
        }
        else
        {
            Invoke("GenerateObjects", 0);
        }
        

        if (SpawnPoses == null || SpawnPoses.Length == 0)
        {
            SpawnPoses = new Transform[1];
            SpawnPoses[0] = transform.GetChild(0);
        }

        //SpawnPos =



    }

    void GenerateObjects()
    {
        int index = Random.Range(0, Objects.Count);
        int poolIndex = GetObjectFromPool(Objects[index].name);

        if (poolIndex == -1)
        {

            GameObject go = Instantiate(Objects[index], Vector3.zero, Quaternion.identity) as GameObject;
            go.transform.parent = transform;
            /*
            if(go.GetComponent<SpriteRenderer>()){
                //float picWidth = go.GetComponent<SpriteRenderer> ().bounds.size.x;
                float picWidth = go.transform.localScale.x;
                if(picWidth>2)
                    go.transform.localPosition = new Vector2(picWidth/2, 0);
                else
                    go.transform.localPosition = new Vector2(10, 0);
                //go.GetComponent<Mover> ().SetSpeed(LayerSpeed);
            }
            else{

                go.transfori.localPosition = new Vector2(10, 0);
            }
            */
            //go.transform.localPosition = SpawnPos.localPosition;

            go.transform.localPosition = SpawnPoses[Random.Range(0, SpawnPoses.Length - 1)].localPosition;
        }
        else
        {
            ActivateObject(poolIndex);
        }
        Invoke("GenerateObjects", Random.Range(repeatRateMin, repeatRateMax));

    }

    void GenerateSystematicObjects()
    {
        //int index = Random.Range(0, Objects.Count);
        int poolIndex = GetObjectFromPool(Objects[0].name);

        if (poolIndex == -1)
        {

            GameObject go = Instantiate(Objects[0], Vector3.zero, Quaternion.identity) as GameObject;
            go.transform.parent = transform;
            if (reversed)
            {
                bool lastReversed = (nextSpawn.parent.localScale.x < 0);
                if (lastReversed)
                {
                    Vector3 temp = go.transform.localScale;
                    temp.x = Mathf.Abs(temp.x);
                    go.transform.localScale = temp;
                }
                else
                {
                    Vector3 temp = go.transform.localScale;
                    temp.x = -1* Mathf.Abs(temp.x);
                    go.transform.localScale = temp;
                }
                    //.Scale(new Vector3(-1,1,1)); 
            }

            go.transform.position = nextSpawn.position;
            nextSpawn = go.transform.GetChild(0);
        }
        else
        {
            ActivateObjectSystematic(poolIndex);
        }
        Invoke("GenerateSystematicObjects", repeatRateMin);

    }
    

    private int GetObjectFromPool(string goName)
    {
        if (Pool.Count == 0)
        {
            return -1;
        }
        for (int i = 0; i < Pool.Count; i++)
        {
            if (Pool[i].name.Contains(goName))
            {
                return i;
            }
        }

        return -1;
    }

    void ActivateObject(int poolIndex)
    {

        GameObject go = Pool[poolIndex];
        go.SetActive(true);

        /*
        if (go.GetComponent<SpriteRenderer>())
        {
            float picWidth = go.GetComponent<SpriteRenderer>().bounds.size.x;
            if (picWidth > 20)
                go.transform.localPosition = new Vector2(picWidth / 2, 0);
            else
                go.transform.localPosition = new Vector2(10, 0);
            //go.GetComponent<Mover> ().SetSpeed(LayerSpeed);
        }
        else
         * */
        {
            go.transform.localPosition = SpawnPoses[Random.Range(0, SpawnPoses.Length - 1)].localPosition;
        }

        go.GetComponent<Mover>().SetSpeed(LayerSpeed);

        Pool.RemoveAt(poolIndex);

    }

    void ActivateObjectSystematic(int poolIndex)
    {

        GameObject go = Pool[poolIndex];
        go.SetActive(true);

        if (reversed)
        {
            bool lastReversed = (nextSpawn.parent.localScale.x < 0);
            if (lastReversed)
            {
                Vector3 temp = go.transform.localScale;
                temp.x = Mathf.Abs(temp.x);
                go.transform.localScale = temp;
            }
            else
            {
                Vector3 temp = go.transform.localScale;
                temp.x = -1 * Mathf.Abs(temp.x);
                go.transform.localScale = temp;
            }
            //.Scale(new Vector3(-1,1,1)); 
        }

        go.transform.position = nextSpawn.position;
        nextSpawn = go.transform.GetChild(0);

        go.GetComponent<Mover>().SetSpeed(LayerSpeed);

        Pool.RemoveAt(poolIndex);

    }
}
