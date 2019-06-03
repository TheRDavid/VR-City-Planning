using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class InsertRoad : MonoBehaviour
{
    public InputField startX, endX;
    public InputField startY, endY;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Insert(){
        int inputStartX = 0;
        int inputStartY = 0;
        int inputEndX = 0;
        int inputEndY = 0;

        if (int.TryParse(startX.text, out inputStartX)
            && int.TryParse(startY.text, out inputStartY)
            && int.TryParse(endX.text, out inputEndX)
            && int.TryParse(endY.text, out inputEndY))
        {
            Road newRoad = new Road(new Vector2Int(inputStartX, inputStartY),
                                    new Vector2Int(inputEndX, inputEndY));

            string dataPath = "CityData/city1.json";

            if (!File.Exists(dataPath))
            {
                List<Building> buildings = new List<Building>(
                    new Building[] { });

                List<Road> roads = new List<Road>(
                    new Road[] { newRoad });

                StreamWriter writer = new StreamWriter(dataPath, false);
                writer.WriteLine(JsonUtility.ToJson(
                    new Municipality(buildings, roads, 0, Vector2Int.zero),
                    true));
                writer.Close();
            }
            else {
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

                    startX.text = "";
                    startY.text = "";
                    endX.text = "";
                    endY.text = "";

                    return;
                }

                //if there are collisions, the road should not be added to the JSON file
                if (newRoad.collisionWithBuildings(municipality.buildings))
                {
                    ErrorHandler.instance.reportError("This road crosses a building! Please choose different start and end points.");
                }
                else
                {
                    municipality.roads.Add(newRoad);

                    StreamWriter writer = new StreamWriter(dataPath, false);
                    writer.WriteLine(JsonUtility.ToJson(municipality));
                    writer.Close();
                }

                startX.text = "";
                startY.text = "";
                endX.text = "";
                endY.text = "";
            }
        } 
        else {
            ErrorHandler.instance.reportError("Invalid values were entered for the road. Please enter only integer numbers.");
        }
    }
}
