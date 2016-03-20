using UnityEngine;
using System.Collections;

public class GameDesign : MonoBehaviour {

    

    public UIGrid wavesGrid;
    public GameObject wavePrefab;

    public UILabel[] blueLabels;
    public UILabel[] pinkLabels; //yellow
    public UILabel[] greenLabels; // ranged
    public UILabel[] yellowLabels; //rolling


    public UIInput wavesNumInput;
    public UILabel kidIndexLabel;

 

	// Use this for initialization
	void Start () {
        Time.timeScale = 0;

        InitTheSavedData();


	}

    void InitTheSavedData()
    {
        InitSavedEnemiesPrefabsData();
        InitSavedWavesData();
    }

    void InitSavedEnemiesPrefabsData()
    {
        //blue
        blueLabels[0].transform.parent.GetComponent<UIInput>().value = PlayerPrefs.GetInt("gd_blue_maxhp", 80).ToString();
        blueLabels[1].transform.parent.GetComponent<UIInput>().value = PlayerPrefs.GetInt("gd_blue_dmg", 10).ToString();
        blueLabels[2].transform.parent.GetComponent<UIInput>().value = PlayerPrefs.GetInt("gd_blue_coins", 250).ToString();
        blueLabels[3].transform.parent.GetComponent<UIInput>().value = PlayerPrefs.GetFloat("gd_blue_mvSpeed", 4f).ToString();
        blueLabels[4].transform.parent.GetComponent<UIInput>().value = PlayerPrefs.GetFloat("gd_blue_evSpeed", 3f).ToString();
        blueLabels[5].transform.parent.GetComponent<UIInput>().value = PlayerPrefs.GetFloat("gd_blue_atkInt", 7f).ToString();
        blueLabels[6].transform.parent.GetComponent<UIInput>().value = PlayerPrefs.GetFloat("gd_blue_evInt", 2f).ToString();

        //pink
        pinkLabels[0].transform.parent.GetComponent<UIInput>().value = PlayerPrefs.GetInt("gd_yellow_maxhp", 80).ToString();
        pinkLabels[1].transform.parent.GetComponent<UIInput>().value = PlayerPrefs.GetInt("gd_yellow_dmg", 10).ToString();
        pinkLabels[2].transform.parent.GetComponent<UIInput>().value = PlayerPrefs.GetInt("gd_yellow_coins", 250).ToString();
        pinkLabels[3].transform.parent.GetComponent<UIInput>().value = PlayerPrefs.GetFloat("gd_yellow_mvSpeed", 4f).ToString();
        pinkLabels[4].transform.parent.GetComponent<UIInput>().value = PlayerPrefs.GetFloat("gd_yellow_evSpeed", 3f).ToString();
        pinkLabels[5].transform.parent.GetComponent<UIInput>().value = PlayerPrefs.GetFloat("gd_yellow_atkInt", 7f).ToString();
        pinkLabels[6].transform.parent.GetComponent<UIInput>().value = PlayerPrefs.GetFloat("gd_yellow_evInt", 2f).ToString();

        //green
        greenLabels[0].transform.parent.GetComponent<UIInput>().value = PlayerPrefs.GetInt("gd_ranged__maxhp", 80).ToString();
        greenLabels[1].transform.parent.GetComponent<UIInput>().value = PlayerPrefs.GetInt("gd_ranged__dmg", 10).ToString();
        greenLabels[2].transform.parent.GetComponent<UIInput>().value = PlayerPrefs.GetInt("gd_ranged__coins", 250).ToString();
        greenLabels[3].transform.parent.GetComponent<UIInput>().value = PlayerPrefs.GetFloat("gd_ranged__mvSpeed", 4f).ToString();
        greenLabels[4].transform.parent.GetComponent<UIInput>().value = PlayerPrefs.GetFloat("gd_ranged__evSpeed", 3f).ToString();
        greenLabels[5].transform.parent.GetComponent<UIInput>().value = PlayerPrefs.GetFloat("gd_ranged__atkInt", 7f).ToString();
        greenLabels[6].transform.parent.GetComponent<UIInput>().value = PlayerPrefs.GetFloat("gd_ranged__evInt", 2f).ToString();

        //yellow
        yellowLabels[0].transform.parent.GetComponent<UIInput>().value = PlayerPrefs.GetInt("gd_rolling_maxhp", 80).ToString();
        yellowLabels[1].transform.parent.GetComponent<UIInput>().value = PlayerPrefs.GetInt("gd_rolling_dmg", 10).ToString();
        yellowLabels[2].transform.parent.GetComponent<UIInput>().value = PlayerPrefs.GetInt("gd_rolling_coins", 250).ToString();
        yellowLabels[3].transform.parent.GetComponent<UIInput>().value = PlayerPrefs.GetFloat("gd_rolling_mvSpeed", 1f).ToString();
        yellowLabels[4].transform.parent.GetComponent<UIInput>().value = PlayerPrefs.GetFloat("gd_rolling_evSpeed", 0f).ToString();
        yellowLabels[5].transform.parent.GetComponent<UIInput>().value = PlayerPrefs.GetFloat("gd_rolling_atkInt", 0f).ToString();
        yellowLabels[6].transform.parent.GetComponent<UIInput>().value = PlayerPrefs.GetFloat("gd_rolling_evInt", 0f).ToString();
    }

    void InitSavedWavesData()
    {
        wavesNumInput.value = PlayerPrefs.GetInt("gd_wavesNum", 2).ToString();
        AdjustWavesNumUI(wavesNumInput.value);

        FillWavesUIWithSavedData();
    }
	

    public void AdjustWavesNumUI(string numStr)
    {

        int gridChilds = wavesGrid.GetChildList().Count;
        int result = int.Parse(numStr) - gridChilds;
        Debug.Log(result);
        if (result < 0)
        {
            result *= -1;
            for (int i = wavesGrid.GetChildList().Count-1; i >= wavesGrid.GetChildList().Count-result; i--)
            {
                GameObject.Destroy(wavesGrid.GetChild(i).gameObject);
                wavesGrid.Reposition();
            }


        }
        else
        {
            for (int i = 0; i < result; i++)
            {
                GameObject go = Instantiate(wavePrefab, Vector3.zero, Quaternion.identity) as GameObject;
                wavesGrid.AddChild(go.transform);
                go.transform.position = Vector3.zero;
                go.transform.localScale = new Vector3(1, 1, 1);

                go.name = "item_" + i;
                go.GetComponentInChildren<UILabel>().text = "#" + (wavesGrid.GetIndex(go.transform)+1);
            }
        }
       


        wavesGrid.Reposition();
    }

    void FillWavesUIWithSavedData()
    {
        string wavesDataString = PlayerPrefs.GetString("gd_wavesData", "1,1,1,1$2,2,0,0");

        string [] dataSplitted = wavesDataString.Split('$');

        for (int i = 0; i < wavesGrid.GetChildList().Count; i++)
        {
            wavesGrid.GetChild(i).GetComponent<WaveInfo>().SetWavesInfo(dataSplitted[i]);

        }

    }
    public void StartGame()
    {
        if (wavesGrid.GetChildList().Count == 0)
            return;


        SaveCurrentValues();

        Time.timeScale = 1;

        InitEnemyWavesData();
        InitEnemiesPrefabs();
        EnemyManager.Instance.kidGroupIndex = int.Parse(kidIndexLabel.text)-1;
        EnemyManager.Instance.StartEnemies();
    }


    void SaveCurrentValues()
    {
        //save the emeny prefabs data

        //blue
        PlayerPrefs.SetInt("gd_blue_maxhp", int.Parse(blueLabels[0].text));
        PlayerPrefs.SetInt("gd_blue_dmg", int.Parse(blueLabels[1].text));
        PlayerPrefs.SetInt("gd_blue_coins", int.Parse(blueLabels[2].text));
        PlayerPrefs.SetFloat("gd_blue_mvSpeed", float.Parse(blueLabels[3].text));
        PlayerPrefs.SetFloat("gd_blue_evSpeed", float.Parse(blueLabels[4].text));
        PlayerPrefs.SetFloat("gd_blue_atkInt", float.Parse(blueLabels[5].text));
        PlayerPrefs.SetFloat("gd_blue_evInt", float.Parse(blueLabels[6].text));

        //pink
        PlayerPrefs.SetInt("gd_yellow_maxhp", int.Parse(pinkLabels[0].text));
        PlayerPrefs.SetInt("gd_yellow_dmg", int.Parse(pinkLabels[1].text));
        PlayerPrefs.SetInt("gd_yellow_coins", int.Parse(pinkLabels[2].text));
        PlayerPrefs.SetFloat("gd_yellow_mvSpeed", float.Parse(pinkLabels[3].text));
        PlayerPrefs.SetFloat("gd_yellow_evSpeed", float.Parse(pinkLabels[4].text));
        PlayerPrefs.SetFloat("gd_yellow_atkInt", float.Parse(pinkLabels[5].text));
        PlayerPrefs.SetFloat("gd_yellow_evInt", float.Parse(pinkLabels[6].text));


        //green
        PlayerPrefs.SetInt("gd_ranged__maxhp", int.Parse(greenLabels[0].text));
        PlayerPrefs.SetInt("gd_ranged__dmg", int.Parse(greenLabels[1].text));
        PlayerPrefs.SetInt("gd_ranged__coins", int.Parse(greenLabels[2].text));
        PlayerPrefs.SetFloat("gd_ranged__mvSpeed", float.Parse(greenLabels[3].text));
        PlayerPrefs.SetFloat("gd_ranged__evSpeed", float.Parse(greenLabels[4].text));
        PlayerPrefs.SetFloat("gd_ranged__atkInt", float.Parse(greenLabels[5].text));
        PlayerPrefs.SetFloat("gd_ranged__evInt", float.Parse(greenLabels[6].text));


        //yellow
        PlayerPrefs.SetInt("gd_rolling_maxhp", int.Parse(yellowLabels[0].text));
        PlayerPrefs.SetInt("gd_rolling_dmg", int.Parse(yellowLabels[1].text));
        PlayerPrefs.SetInt("gd_rolling_coins", int.Parse(yellowLabels[2].text));
        PlayerPrefs.SetFloat("gd_rolling_mvSpeed", float.Parse(yellowLabels[3].text));
        PlayerPrefs.SetFloat("gd_rolling_evSpeed", float.Parse(yellowLabels[4].text));
        PlayerPrefs.SetFloat("gd_rolling_atkInt", float.Parse(yellowLabels[5].text));
        PlayerPrefs.SetFloat("gd_rolling_evInt", float.Parse(yellowLabels[6].text));


        //Save the wavesData

        PlayerPrefs.SetInt("gd_wavesNum", int.Parse(wavesNumInput.value));

        string toBeSaved = "";
        for (int i = 0; i < wavesGrid.GetChildList().Count; i++)
        {
            if (i != 0)
            {
                toBeSaved += '$';
            }
            toBeSaved += wavesGrid.GetChild(i).GetComponent<WaveInfo>().GetWaveInfoStringToSave();

        }
        if (toBeSaved == "")
        {
            toBeSaved = "1,1,1,1$2,2,0,0";
        }

        PlayerPrefs.SetString("gd_wavesData", toBeSaved);

    }

    void InitEnemyWavesData()
    {
        int gridCount = wavesGrid.GetChildList().Count;
        string temp = "";

        EnemiesSpawnData.Instance.enemiesGroupsData = new string[gridCount];

        for (int i = 0; i < gridCount; i++)
        {
            temp = wavesGrid.GetChild(i).GetComponent<WaveInfo>().GetWaveInfo();
            EnemiesSpawnData.Instance.enemiesGroupsData[i] = temp;
        }
    }

    void InitEnemiesPrefabs()
    {
        //bluuuuuuuuuue
        GameObject blue = EnemyManager.Instance.enemyPrefabs[0];
        blue.GetComponent<EnemyHealth>().MaxHealth      = int.Parse(blueLabels[0].text);
        blue.GetComponent<EnemyAI>().damage             = int.Parse(blueLabels[1].text);
        blue.GetComponent<EnemyHealth>().dieGoldAmount  = int.Parse(blueLabels[2].text);
        blue.GetComponent<EnemyAI>().moveSpeed          = float.Parse(blueLabels[3].text);
        blue.GetComponent<EnemyAI>().evadeSpeed = float.Parse(blueLabels[4].text);
        blue.GetComponent<EnemyAI>().timeToAttack = float.Parse(blueLabels[5].text);
        blue.GetComponent<EnemyAI>().timeBetweenEvades = float.Parse(blueLabels[6].text);

        blue.GetComponent<EnemyHealth>().currentHealth = blue.GetComponent<EnemyHealth>().MaxHealth;

        //yellow
        GameObject yellow = EnemyManager.Instance.enemyPrefabs[3];
        yellow.GetComponent<EnemyHealth>().MaxHealth = int.Parse(pinkLabels[0].text);
        yellow.GetComponent<EnemyAI>().damage = int.Parse(pinkLabels[1].text);
        yellow.GetComponent<EnemyHealth>().dieGoldAmount = int.Parse(pinkLabels[2].text);
        yellow.GetComponent<EnemyAI>().moveSpeed = float.Parse(pinkLabels[3].text);
        yellow.GetComponent<EnemyAI>().evadeSpeed = float.Parse(pinkLabels[4].text);
        yellow.GetComponent<EnemyAI>().timeToAttack = float.Parse(pinkLabels[5].text);
        yellow.GetComponent<EnemyAI>().timeBetweenEvades = float.Parse(pinkLabels[6].text);

        yellow.GetComponent<EnemyHealth>().currentHealth = yellow.GetComponent<EnemyHealth>().MaxHealth;

        //ranged
        GameObject ranged = EnemyManager.Instance.enemyPrefabs[6];
        ranged.GetComponent<EnemyHealth>().MaxHealth = int.Parse(greenLabels[0].text);
        ranged.GetComponent<EnemyAI>().damage = int.Parse(greenLabels[1].text);
        ranged.GetComponent<EnemyHealth>().dieGoldAmount = int.Parse(greenLabels[2].text);
        ranged.GetComponent<EnemyAI>().moveSpeed = float.Parse(greenLabels[3].text);
        ranged.GetComponent<EnemyAI>().evadeSpeed = float.Parse(greenLabels[4].text);
        ranged.GetComponent<EnemyAI>().timeToAttack = float.Parse(greenLabels[5].text);
        ranged.GetComponent<EnemyAI>().timeBetweenEvades = float.Parse(greenLabels[6].text);

        ranged.GetComponent<EnemyHealth>().currentHealth = ranged.GetComponent<EnemyHealth>().MaxHealth;

        //rolling
        GameObject rolling = EnemyManager.Instance.enemyPrefabs[8];
        rolling.GetComponent<EnemyHealth>().MaxHealth = int.Parse(yellowLabels[0].text);
        rolling.GetComponent<EnemyAI>().damage = int.Parse(yellowLabels[1].text);
        rolling.GetComponent<EnemyHealth>().dieGoldAmount = int.Parse(yellowLabels[2].text);
        rolling.GetComponent<EnemyAI>().moveSpeed = float.Parse(yellowLabels[3].text);
        rolling.GetComponent<EnemyAI>().evadeSpeed = float.Parse(yellowLabels[4].text);
        rolling.GetComponent<EnemyAI>().timeToAttack = float.Parse(yellowLabels[5].text);
        rolling.GetComponent<EnemyAI>().timeBetweenEvades = float.Parse(yellowLabels[6].text);

        rolling.GetComponent<EnemyHealth>().currentHealth = rolling.GetComponent<EnemyHealth>().MaxHealth;
    }
}
