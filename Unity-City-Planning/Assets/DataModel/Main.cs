using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class Main : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject buildingPrefab;
    public GameObject roadPrefab;

    void Start()
    {
        buildingPrefab = Resources.Load("Prefabs/Building") as GameObject;
        roadPrefab = Resources.Load("Prefabs/Road") as GameObject;

        string dataPath = "CityData/city1.json";

        Municipality m;

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

            m = new Municipality(buildings, roads, 0, Vector2Int.zero);
            
            StreamWriter writer = new StreamWriter(dataPath, false);
            writer.WriteLine(JsonUtility.ToJson(m, true));
            writer.Close();
        } else
        {
            Debug.Log("read from file");
            FileStream dataFile = File.Open(dataPath, FileMode.Open);
            StreamReader reader = new StreamReader(dataFile);
            m = JsonUtility.FromJson<Municipality>(reader.ReadToEnd());
        }

        foreach (Building b in m.buildings)
        {
            Instantiate(buildingPrefab, locationToUnityLocation(b.Location), Quaternion.identity);
        }

        foreach (Road r in m.roads)
        {
            GameObject thisRoad = Instantiate(roadPrefab, locationToUnityLocation(r.Start), Quaternion.identity);
            //thisRoad.transform.localScale = new Vector3(0.1F, 0, 0);
        }
    }

    public Vector3Int locationToUnityLocation(Vector2Int location)
    {
        return new Vector3Int(location.x, 0, location.y);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
