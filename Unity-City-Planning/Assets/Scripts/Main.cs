using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Runtime.InteropServices;
using Valve.VR;

public class Main : MonoBehaviour
{
    void Start()
    {
        ErrorHandler.instance = new UErrorHandler();

        string dataFileName = "city1";

        string cityDataDir = "CityData/";
        string dataPath = cityDataDir + dataFileName + ".json";
        string conditionsPath = cityDataDir + dataFileName + ".conditions.json";

        if (!File.Exists(dataPath))
        {
            List<Building> buildings = new List<Building>(
                new Building[]{
                new Building(15,3, new Vector2Int(2,4), Vector3Int.one, "Business"),
                new Building(2,0,  new Vector2Int(2,5), Vector3Int.one, "Business"),
                new Building(2,4,  new Vector2Int(2,6), Vector3Int.one, "Default"),
                new Building(5,3, new Vector2Int(4,4), Vector3Int.one, "Default"),
                new Building(7,0,  new Vector2Int(4,5), Vector3Int.one, "Industrial"),
                new Building(2,14,  new Vector2Int(4,6), Vector3Int.one, "Default")
            });

            List<Road> roads = new List<Road>(
                new Road[]{
                new Road(new Vector2Int(3,1), new Vector2Int(3,7)),
                new Road(new Vector2Int(1,1), new Vector2Int(1,6))
            });

            StreamWriter writer = new StreamWriter(dataPath, false);
            writer.WriteLine(JsonUtility.ToJson(
                new Municipality(buildings, roads, new Vector2Int(10,10), 0.5, 200), 
                true));
            writer.Close();
        }

        if (!File.Exists(conditionsPath))
        {
            ConditionList conditions = new ConditionList();
            Condition mainRoadHighlighter = new Condition("Length", Condition.ConditionType.larger, "4", new Visualizer.ColorHighlighter(0.0f, 0.4f, 0.5f, 0.75f));
            Condition highConsumptionHouseHighlighter = new Condition("Consumption", Condition.ConditionType.largerEquals, "10", new Visualizer.ColorHighlighter(1.0f, 0.2f, 0, 1));
            Condition emptyHousePulser = new Condition("Population", Condition.ConditionType.equals, "0", new Visualizer.SizePulser(1, 1.5f, 1, 1));
            conditions.conditions.Add(mainRoadHighlighter);
            conditions.conditions.Add(highConsumptionHouseHighlighter);
            conditions.conditions.Add(emptyHousePulser);
            string jsonRepresentation = JsonUtility.ToJson(conditions,true);
            
            StreamWriter writer = new StreamWriter(conditionsPath, false);
            writer.WriteLine(jsonRepresentation);
            writer.Close();
        }

        GameObject mainObject = GameObject.Find("mainObject");

        mainObject.AddComponent<UnityDataWatcher>();
        DataHandler dataHandler = new DataHandler(cityDataDir, dataFileName, mainObject.GetComponent<UnityDataWatcher>());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
