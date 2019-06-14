﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class InsertBuilding : MonoBehaviour
{
    public InputField xCoord;
    public InputField yCoord;
    public InputField consumption;
    public InputField population;

    public Dropdown categoryDropdown;

    // Start is called before the first frame update
    void Start()
    {
        //populate the dropdown with building categories
        categoryDropdown.ClearOptions();
        categoryDropdown.AddOptions(Building.BuildingCategories);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Insert()
    {
        int x = 0;
        int y = 0;
        int consumptionInput = 0;
        int populationInput = 0;

        if (int.TryParse(xCoord.text, out x) 
            && int.TryParse(yCoord.text, out y)
            && int.TryParse(consumption.text, out consumptionInput)
            && int.TryParse(population.text, out populationInput))
        {
            string category = Building.BuildingCategories[categoryDropdown.value];

            Building newBuilding = new Building(consumptionInput, populationInput, new Vector2Int(x, y), Vector3Int.one, category);

            Municipality.InsertBuilding(newBuilding);

            xCoord.text = "";
            yCoord.text = "";
            consumption.text = "";
            population.text = "";
            categoryDropdown.value = 0;

        }
        else {
            ErrorHandler.instance.reportError("Invalid values were entered for the building. Please enter only integer numbers.");
        }


    }
}