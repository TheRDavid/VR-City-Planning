using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class InsertBuilding : MonoBehaviour
{
    public InputField xCoord;
    public InputField yCoord;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Insert()
    {
        int x = 0;
        int y = 0;

        if (int.TryParse(xCoord.text, out x) && int.TryParse(yCoord.text, out y))
        {
            string dataPath = "CityData/city1.json";

            if (!File.Exists(dataPath))
            {
                List<Building> buildings = new List<Building>(
                    new Building[]{
                new Building(2,3, new Vector2Int(x,y),Vector3Int.one),
                });

                List<Road> roads = new List<Road>(
                    new Road[] { });

                StreamWriter writer = new StreamWriter(dataPath, false);
                writer.WriteLine(JsonUtility.ToJson(
                    new Municipality(buildings, roads, 0, Vector2Int.zero),
                    true));
                writer.Close();
            }
            else
            {
                //read the entire JSON file
                FileStream readStream = File.Open(dataPath, FileMode.Open);
                StreamReader reader = new StreamReader(readStream);
                reader.BaseStream.Seek(0, SeekOrigin.Begin);

                string jsonData = reader.ReadToEnd();
                readStream.Close();

                Municipality municipality;

                try
                {
                    municipality = JsonUtility.FromJson<Municipality>(jsonData);
                }
                catch (Exception ae)
                {
                    ErrorHandler.instance.reportError("Data file " + readStream.Name + " can not be read as Municipality -> it appears to be corrupt.\nDetails:\n" + ae.ToString());
                    return;
                }

                Building newBuilding = new Building(2, 3, new Vector2Int(x, y), Vector3Int.one);

                if (newBuilding.collisionWithBuildings(municipality.buildings) || newBuilding.collisionWithRoads(municipality.roads))
                {
                    //if there are collisions, the building should not be added to the JSON file
                    ErrorHandler.instance.reportError("An entity already exists at this location" + newBuilding.location.ToString());
                }
                else
                {
                    municipality.buildings.Add(newBuilding);

                    StreamWriter writer = new StreamWriter(dataPath, false);
                    writer.WriteLine(JsonUtility.ToJson(municipality));
                    writer.Close();
                }
            }
        }
        else {
            Debug.Log("Invalid values entered for coordinates");
        }


    }
}
