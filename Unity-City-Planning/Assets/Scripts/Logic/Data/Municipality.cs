using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;
using System;
using System.IO;

[Serializable]
public class Municipality
{
    public int streetSpace, buildingSpace, greenSpace;
    public Vector2Int size;
    public List<Building> buildings;
    public List<Road> roads;

    // We will need to change from an int as street space to actual streets (two coordinates that are connected)
    public int StreetSpace
    {
        get { return streetSpace; }
        set { }
    }

    public int BuildingSpace
    {
        get { return buildingSpace; }
        private set { }
    }

    public int GreenSpace
    {
        get { return greenSpace; }
        private set { }
    }

    public Vector2Int Size
    {
        get { return size; }
        set { updateGreenSpace(); }
    }

    public Municipality(List<Building> buildings, List<Road> roads, int streetSpace, Vector2Int size)
    {
        this.buildings = buildings;
        this.roads = roads;
        this.StreetSpace = streetSpace;
        this.Size = size;
        updateStreetSpace();
        updateBuildingSpace();
    }

    private void updateStreetSpace()
    {
        int space = 0;

        // No calcs required, streets are only an int for now
        space = streetSpace;

        streetSpace = space;
        updateGreenSpace();
    }

    private void updateBuildingSpace()
    {
        int space = 0;

        foreach (Building b in buildings)
        {
            space += b.Size.x * b.Size.y;
        }

        buildingSpace = space;
        updateGreenSpace();
    }

    private void updateGreenSpace()
    {
        greenSpace = size.x * size.y - streetSpace - buildingSpace;
    }

    public static void InsertRoad(Road newRoad)
    {
        string dataPath = "CityData/city1.json";
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

        //if there are collisions, the road should not be added to the JSON file
        if (newRoad.collisionWithBuildings(municipality.buildings))
        {
            ErrorHandler.instance.reportError("This road crosses a building! Please choose different start and end points.");
        }
        else
        {
            municipality.roads.Add(newRoad);

            StreamWriter writer = new StreamWriter(dataPath, false);
            writer.WriteLine(JsonUtility.ToJson(municipality, true));
            writer.Close();
        }
    }

    public static void InsertBuilding(Building newBuilding)
    {

        string dataPath = "CityData/city1.json";

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

        //if there are collisions, the building should not be added to the JSON file
        if (newBuilding.collisionWithBuildings(municipality.buildings) || newBuilding.collisionWithRoads(municipality.roads))
        {
            ErrorHandler.instance.reportError("An entity already exists at this location" + newBuilding.location.ToString());
        }
        else
        {
            municipality.buildings.Add(newBuilding);

            StreamWriter writer = new StreamWriter(dataPath, false);
            writer.WriteLine(JsonUtility.ToJson(municipality, true));
            writer.Close();
        }
    }

}
