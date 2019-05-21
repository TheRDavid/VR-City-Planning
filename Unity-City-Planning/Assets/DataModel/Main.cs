using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class Main : MonoBehaviour
{

    void Start()
    {

        string dataPath = "CityData/";
        string dataFileName = "city1.json";

        if (!File.Exists(dataPath))
        {
            Debug.Log("create new file");
            List<Building> buildings = new List<Building>(
                new Building[]{
                new Building(5,3, new Vector2Int(2,3),Vector3Int.one),
                new Building(2,4,  new Vector2Int(1,3),Vector3Int.one),
                new Building(2,4,  new Vector2Int(0,3),Vector3Int.one)
            });


            List<Road> roads = new List<Road>(
                new Road[]{
            new Road(new Vector2Int(2,1),new Vector2Int(0,1)),
            new Road(new Vector2Int(2,4),new Vector2Int(0,4))
            });
            
            StreamWriter writer = new StreamWriter(dataPath+dataFileName, false);
            writer.WriteLine(JsonUtility.ToJson(
                new Municipality(buildings, roads, 0, Vector2Int.zero), 
                true));
            writer.Close();
        }

        Debug.Log("read from file");

        GameObject mainObject = GameObject.Find("mainObject");

        mainObject.AddComponent<UnityDataWatcher>();
        DataHandler dataHandler = new DataHandler(dataPath, dataFileName, mainObject.GetComponent<UnityDataWatcher>());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
