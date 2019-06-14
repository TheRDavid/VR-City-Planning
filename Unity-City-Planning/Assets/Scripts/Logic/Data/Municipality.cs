using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;
using System;
using System.IO;

[Serializable]
public class Municipality
{
    [System.NonSerialized] public int streetSpace, buildingSpace, greenSpace;
    public static Municipality instance;
    [System.NonSerialized] private Road ground = new Road(Vector2Int.zero, Vector2Int.zero);
    public Vector2Int size;
    public List<Building> buildings;
    public List<Road> roads;

    public int totalPopulation;
    public int totalConsumption;
    public double populationDensity;
    public double renewableEnergyProduction;
    public int CO2Emissions;

    public double commercialAccess;
    public double natureAccess;

    // We will need to change from an int as street space to actual streets (two coordinates that are connected)
    public int StreetSpace
    {
        get { return streetSpace; }
        private set { }
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
        set { 
            updateGreenSpace();
            calculatePopulationConsumption();
        }
    }

    public Municipality(List<Building> buildings, List<Road> roads, Vector2Int size, double energyProd, int CO2)
    {
        instance = this;
        ground.ID = "ground";
        this.buildings = buildings;
        this.roads = roads;
        this.size = size;
        this.renewableEnergyProduction = energyProd;
        this.CO2Emissions = CO2;
        updateStreetSpace();
        updateBuildingSpace();

        calculatePopulationConsumption();
    }

    public void updateSpaces(){
        updateStreetSpace();
        updateBuildingSpace();
    }

    private void updateStreetSpace()
    {
        int space = 0;

        // No calcs required, streets are only an int for now
        foreach(Road r in roads){
            space += Math.Abs((int) r.Length);
        }

        streetSpace = space;
        updateGreenSpace();
    }

    public MapEntity entityByID(string ID)
    {
        foreach (Road r in roads)
        {
            if (r.ID.Equals(ID)) return r;
        }
        foreach (Building r in buildings)
        {
            if (r.ID.Equals(ID)) return r;
        }
        return null;
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
        greenSpace = Size.x * Size.y - streetSpace - buildingSpace;
    }

    private void calculatePopulationConsumption()
    {
        int totalPop = 0;
        int totalCons = 0;

        foreach (Building b in this.buildings)
        {
            totalPop += b.population;
            totalCons = b.consumption;
        }

        this.totalPopulation = totalPop;
        this.totalConsumption = totalCons;
        this.populationDensity = this.totalPopulation / (this.Size.x * this.Size.y);
    }

    public void calculateCommercialAccess(){
        int peopleWithAccess = 0;

        foreach (Building b in this.buildings){

            foreach (Building ind in this.buildings){

                if (ind.Category != "Business"){
                    continue;
                }

                if (ind.distance(b) <= 50){
                    peopleWithAccess += b.population;
                    break;
                }
            }
        }

        commercialAccess = peopleWithAccess / totalPopulation;
    }

    public void calculateNatureAccess(){
        // we are currently not tracking the location of greenspace
        // so instead, we calculate the proportion of greenspace per population

        natureAccess = greenSpace / totalPopulation;
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
