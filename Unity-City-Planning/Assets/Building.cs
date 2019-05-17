using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Building : MonoBehaviour
{
    private int population;
    private int consumption;
    private Vector3Int size;

    public int Consumption {
        get { return consumption; }
        set { updateScore(); }
    }
    public int Population {
        get { return population; }
        set { updateScore(); }
    }
    public Vector3Int Size {
        get { return size; }
        set { updateScore(); }
    }
    public int buildingScore { get; private set; }

    public Building(int consumption, int population, Vector3Int size)
    {
        this.Consumption = consumption;
        this.Population = population;
        this.Size = size;
        updateScore();
    }

    public void updateScore()
    {
        buildingScore = this.Population * this.Consumption + (int) this.Size.magnitude;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
