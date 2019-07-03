using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using System.Linq;

[Serializable]
public class Building : MapEntity
{
    public int population, consumption;
    public Vector3Int size;
    public Vector2Int location;

    public string Category;
    public static readonly List<string> BuildingCategories = new List<string>(
        new string[] { "Default", "Business", "Industrial" });
    
    public int Consumption {
        get { return consumption; }
        set { consumption = value; updateScore(); }
    }
    public int Population {
        get { return population; }
        set { population = value; updateScore(); }
    }
    public Vector3Int Size
    {
        get { return size; }
        set { size = value; updateScore(); }
    }
    public Vector2Int Location
    {
        get { return location; }
        set { location = value; }
    }
    public int buildingScore { get; private set; }

    public Building(int consumption, int population, Vector2Int location, Vector3Int size, string cat)
    {
        this.Consumption = consumption;
        this.Population = population;
        this.Location = location;
        this.Size = size;

        if (BuildingCategories.Contains(cat))
        {
            this.Category = cat;
        }
        else
        {
            Debug.Log("Invalid building category entered");
        }

        updateScore();
    }

    public void updateScore()
    {
        buildingScore = this.Consumption / this.Population;
    }

    public double distance(Building other){
        return Vector2Int.Distance(this.location, other.location);
    }

    internal bool collisionWithBuildings(List<Building> buildings)
    {
        foreach(Building b in buildings)
        {
            if (location.Equals(b.Location)) return true;
        }
        return false;
    }

    internal bool collisionWithRoads(List<Road> roads)
    {
        foreach(Road r in roads)
        {
            if (collide(this, r)) return true;
        }
        return false;
    }
}
