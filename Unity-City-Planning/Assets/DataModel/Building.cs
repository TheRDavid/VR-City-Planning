using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Building
{
    private int population;
    private int consumption;
    private Vector3Int size;
    private Vector2Int location;

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
}
