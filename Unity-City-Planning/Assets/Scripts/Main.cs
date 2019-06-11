using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Runtime.InteropServices;

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
                new Building(15,3, new Vector2Int(2,3), Vector3Int.one),
                new Building(2,0,  new Vector2Int(2,3), Vector3Int.one),
                new Building(2,4,  new Vector2Int(2,3), Vector3Int.one)
            });


            List<Road> roads = new List<Road>(
                new Road[]{
                new Road(new Vector2Int(2,1), new Vector2Int(0,1)),
                new Road(new Vector2Int(7,4), new Vector2Int(0,4))
            });
            
            StreamWriter writer = new StreamWriter(dataPath, false);
            writer.WriteLine(JsonUtility.ToJson(
                new Municipality(buildings, roads, 0, Vector2Int.zero, 0.5, 200), 
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
