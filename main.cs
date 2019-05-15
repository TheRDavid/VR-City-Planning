using System.IO;
using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("Starting...");
        Building building1 = new Building(2, 400, 300);
        Building building2 = new Building(4, 450, 200);
        Building[] buildings = new Building[] {building1, building2};
        
        Municipality enschede = new Municipality(buildings, 396, 1000);
        
        Console.WriteLine("City population: " + enschede.Population + ", size: " + enschede.Size + ", green space: " + enschede.GreenSpace);
        Console.WriteLine("Building 1 score: " + building1.buildingScore);
        Console.WriteLine("Building 2 score: " + building2.buildingScore);
    }
    
    
}

class Building
{
    public int Consumption;
    public int Population;
    public int Size;
    // private int Location;
    
    public Building(int c, int p, int s) {
        Consumption = c;
        Population = p;
        Size = s;
    }
    
    public int buildingScore
    {
        get
        {
            return (Population * Consumption) + Size;
        }
        set
        {
            
        }
    }
}

class Municipality
{
    public Building[] Buildings;
    public int Streets;
    public int Size;
    
    public Municipality(Building[] b, int st, int s) {
        Buildings = b;
        Streets = st;
        Size = s;
    }

    public int Population
    {
        get
        {
            int p = 0;
            for (int i =0; i < Buildings.Length; i++)
            {
                p += Buildings[i].Population;
            }
            return p;
        }
        set
        {
            
        }
    }
    
    public int GreenSpace
    {
        get
        {
            int p = 0;
            for (int i =0; i < Buildings.Length; i++)
            {
                p += Buildings[i].Size;
            }
            return Size - Streets - p;
        }
        set
        {
            
        }
    }
}