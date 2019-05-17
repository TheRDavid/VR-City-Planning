using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class Main : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject building;
    public GameObject road;

    void Start()
    {
        building = Resources.Load("Prefabs/Building") as GameObject;
        road = Resources.Load("Prefabs/Road") as GameObject;

        List<Building> buildings = new List<Building>(
            new Building[]{
            new Building(5,3, new Vector2Int(2,3),Vector3Int.one),
            new Building(2,4,  new Vector2Int(1,3),Vector3Int.one),
            new Building(2,4,  new Vector2Int(0,3),Vector3Int.one)
        });

        Municipality m = new Municipality(buildings, 0, Vector2Int.zero);

        foreach (Building b in buildings)
        {
            Instantiate(building, locationToUnityLocation(b.Location), Quaternion.identity);
        }

        List<Road> roads = new List<Road>(
            new Road[]{
            new Road(new Vector2Int(2,1),new Vector2Int(0,1)),
            new Road(new Vector2Int(2,4),new Vector2Int(0,4))
        });

        foreach (Road r in roads)
        {
            GameObject thisRoad = Instantiate(road, locationToUnityLocation(r.Start), Quaternion.identity);
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
