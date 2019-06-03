using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[Serializable]
public class Road : MapEntity
{
    public Vector2Int start, end;
    private float length;

    public Vector2Int Start
    {
        get { return start; }
        set { 
            start = value; 
            calculateLength(); 
        }
    }
    public Vector2Int End
    {
        get { return end; }
        set { 
            end = value; 
            calculateLength(); 
        }
    }
    public float Length { 
        get {
            return length;
        } 
        private set {
            length = value;
        }
    }

    public Road(Vector2Int start, Vector2Int end)
    {
        this.Start = start;
        this.End = end;
        calculateLength();
    }

    public void calculateLength()
    {
       this.Length = Vector2.Distance(Start, End); 
    }

    public int getOrientation() //1 = north, 2 = east, 3 = south, 4 = west
    {
        if (this.Start.x == this.End.x) //North or South
        {
            if (this.Start.y > this.End.y) //South
            {
                return 3;
            }
            else return 1; //North
        }
        else if (this.Start.y == this.End.y) //East or West
        {
            if (this.Start.x > this.End.x) //West
            {
                return 4;
            }
            else return 2; //East
        }
        else return 0;
    }

    public bool collisionWithBuildings(List<Building> buildings)
    {
        int orientation = this.getOrientation();

        foreach (Building b in buildings)
        {
            bool collision = false;

            // there is a collision if the building is on the road
            switch (orientation)
            {
                case 1:
                    collision = (this.Start.x == b.location.x && this.Start.y <= b.location.y && b.location.y <= this.End.y);
                    break;
                case 2:
                    collision = (this.Start.y == b.location.y && this.Start.x >= b.location.x && b.location.x >= this.End.x);
                    break;
                case 3:
                    collision = (this.Start.x == b.location.x && this.Start.y >= b.location.y && b.location.y >= this.End.y);
                    break;
                case 4:
                    collision = (this.Start.y == b.location.y && this.Start.x <= b.location.x && b.location.x <= this.End.x);
                    break;
                default:
                    ErrorHandler.instance.reportError("Invalid orientation of road detected", this);
                    break;
            }
            if (collision)
            {
                ErrorHandler.instance.reportError("A building is overlapping with a road", this);
                return true;
            }
        }

        return false;
    }
}
