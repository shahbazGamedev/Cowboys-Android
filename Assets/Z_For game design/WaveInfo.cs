using UnityEngine;
using System.Collections;

public class WaveInfo : MonoBehaviour {

    public UILabel BlueNumLabel;
    public UILabel pinkNumLabel;
    public UILabel greenNumLabel;
    public UILabel yellowNumLabel;

    public int BlueNum;
    public int pinkNum;
    public int greenNum;
    public int yellowNum;

	// Use this for initialization
	void Start () {



        //UpdateInfo();
       
	}

    void UpdateInfo()
    {
        BlueNum = int.Parse(BlueNumLabel.text);
        pinkNum = int.Parse(pinkNumLabel.text);
        greenNum = int.Parse(greenNumLabel.text);
        yellowNum = int.Parse(yellowNumLabel.text);
    }

    public void SetWavesInfo(string infoStr)
    {
        string [] str = infoStr.Split(',');

        BlueNumLabel.transform.GetComponentInParent<UIInput>().value = str[0];
        pinkNumLabel.transform.GetComponentInParent<UIInput>().value = str[1];
        greenNumLabel.transform.GetComponentInParent<UIInput>().value = str[2];
        yellowNumLabel.transform.GetComponentInParent<UIInput>().value = str[3];

        UpdateInfo();
    }


    public string GetWaveInfoStringToSave()
    {
        string target;

        target = BlueNumLabel.transform.GetComponentInParent<UIInput>().value + "," +
            pinkNumLabel.transform.GetComponentInParent<UIInput>().value + "," +
            greenNumLabel.transform.GetComponentInParent<UIInput>().value + "," +
            yellowNumLabel.transform.GetComponentInParent<UIInput>().value;

        return target;
    }

	public string GetWaveInfo () {
        UpdateInfo();
        string info = "0_1";


        info = 0 + "_" + BlueNum + "," + 
            3 + "_" + pinkNum + "," +
            6 + "_" + greenNum + "," + 
            8 + "_" + yellowNum;

        return info;
       
	}
}
