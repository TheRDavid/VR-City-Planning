using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class Main : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        List<Building> buildings = new List<Building>(
            new Building[]{
            new Building(5,3, Vector3Int.one),
            new Building(2,4, new Vector3Int(1,2,3))
        });
        Municipality m = new Municipality(buildings, 0, Vector2Int.zero);
        Debug.Log("City population: " + m.StreetSpace + ", size: " + m.Size + ", green space: " + m.GreenSpace);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
