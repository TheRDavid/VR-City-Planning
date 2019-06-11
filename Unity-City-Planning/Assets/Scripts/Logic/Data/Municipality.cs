﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;
using System;

[Serializable]
public class Municipality
{
    [System.NonSerialized] public int streetSpace, buildingSpace, greenSpace;
    public Vector2Int size;
    public List<Building> buildings;
    public List<Road> roads;

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
        set { updateGreenSpace(); }
    }

    public Municipality(List<Building> buildings, List<Road> roads, Vector2Int size)
    {
        this.buildings = buildings;
        this.roads = roads;
        this.size = size;
        updateStreetSpace();
        updateBuildingSpace();
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
            space += Math.Abs((int) r.length());
        }

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
        greenSpace = size.x * size.y - streetSpace - buildingSpace;
    }
}
