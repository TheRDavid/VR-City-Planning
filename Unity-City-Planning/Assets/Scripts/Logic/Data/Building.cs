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

    public Building(int consumption, int population, Vector2Int location, Vector3Int size)
    {
        this.Consumption = consumption;
        this.Population = population;
        this.Location = location;
        this.Size = size;
        updateScore();
    }

    public void updateScore()
    {
        buildingScore = this.Population + this.Consumption + (int) this.Size.magnitude;
    }

    public bool collisionWithBuildings(List<Building> buildings){
        var coordinates = buildings.Select((building, index) => building.location);
        return coordinates.Contains(this.location);
    }

    public bool collisionWithBuildings(List<Vector2Int> buildings){
        return buildings.Contains(this.location);
    }

    public bool collisionWithRoads(List<Road> roads){
        foreach (Road r in roads)
        {
            int orientation = r.getOrientation();

            bool collision = false;

            // there is a collision if the building is on the road
            switch (orientation)
            {
                case 1:
                    collision = (r.Start.x == this.location.x && r.Start.y <= this.location.y && this.location.y <= r.End.y);
                    break;
                case 2:
                    collision = (r.Start.y == this.location.y && r.Start.x <= this.location.x && this.location.x <= r.End.x);
                    break;
                case 3:
                    collision = (r.Start.x == this.location.x && r.Start.y >= this.location.y && this.location.y >= r.End.y);
                    break;
                case 4:
                    collision = (r.Start.y == this.location.y && r.Start.x >= this.location.x && this.location.x >= r.End.x);
                    break;
                default:
                    ErrorHandler.instance.reportError("Invalid orientation of road detected", r);
                    break;
            }
            if (collision)
            {
                ErrorHandler.instance.reportError("Building is overlapping with a road", this);
                return true;
            }
        }

        return false;
    }
}
