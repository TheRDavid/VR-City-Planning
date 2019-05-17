using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class Main : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject prefab;
    void Start()
    {
        List<Building> buildings = new List<Building>(
            new Building[]{
            new Building(5,3, new Vector2Int(2,3),Vector3Int.one),
            new Building(2,4,  new Vector2Int(1,3),Vector3Int.one),
            new Building(2,4,  new Vector2Int(0,3),Vector3Int.one)
        });
        Municipality m = new Municipality(buildings, 0, Vector2Int.zero);

        foreach (Building b in buildings)
        {
            Instantiate(prefab, locationToUnityLocation(b.Location), Quaternion.identity);
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
