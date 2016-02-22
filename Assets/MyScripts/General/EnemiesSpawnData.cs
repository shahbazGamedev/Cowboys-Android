using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

public class EnemiesSpawnData : MonoBehaviour {

	public static EnemiesSpawnData Instance;


	//G_3,R_5,B_9,Y_6
	public string [] enemiesGroupsData;
	public int enemiesTypes = 4;

	void Awake () {
		Instance = this;
	}



	public List<Vector2> [] InitEnemiesGroups(){
		List<Vector2> [] groups = new List<Vector2>[enemiesGroupsData.Length];

		List<Vector2> groupList;

		for(int i=0; i<enemiesGroupsData.Length; i++){
			string [] groupStr = enemiesGroupsData[i].Split(',');
			float groupAmount;
			groupList = new List<Vector2>() ;
			Vector2 v;
			for(int j=0; j<groupStr.Length; j++){
				groupAmount = float.Parse( groupStr[j].Substring(2));

				//Debug.Log("has to be underscore" + groupStr[j].Substring(2));
				string temp = groupStr[j][0].ToString();
				v = new Vector2(float.Parse(temp), groupAmount); 
				groupList.Add(v);
			}
			groups[i] = groupList;
			//Debug.Log("nana_i: " + groupList[0].x + ", "+groupList[0].y + " oPP "+ groupList[1].x);
		}
		//string 

		return groups;
	}

}
