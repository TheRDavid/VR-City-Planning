using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;
using System;

[Serializable]
public class Municipality
{
    public int streetSpace, buildingSpace, greenSpace;
    public Vector2Int size;
    public List<Building> buildings;
    public List<Road> roads;

    public int totalPopulation;
    public int totalConsumption;
    public double populationDensity;
    public double renewableEnergyProduction;
    public int CO2Emissions;

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
        set { 
            updateGreenSpace();
            calculatePopulationConsumption();
        }
    }

    public Municipality(List<Building> buildings, List<Road> roads, int streetSpace, Vector2Int size, double energyProd, int CO2)
    {
        this.buildings = buildings;
        this.roads = roads;
        this.StreetSpace = streetSpace;
        this.Size = size;
        this.renewableEnergyProduction = energyProd;
        this.CO2Emissions = CO2;

        updateStreetSpace();
        updateBuildingSpace();

        calculatePopulationConsumption();
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

        foreach(Building b in buildings)
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

        foreach(Building b in this.buildings){
            totalPop += b.population;
            totalCons = b.consumption;
        }

        this.totalPopulation = totalPop;
        this.totalConsumption = totalCons;
        this.populationDensity = this.totalPopulation / (this.Size.x * this.Size.y);
    }

}
