using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[Serializable]
public class Road : MapEntity
{
    public Vector2Int start, end;
    public int length;

    public Vector2Int Start
    {
        get { return start; }
        set { 
            start = value;
            this.length = getLength();
        }
    }
    public Vector2Int End
    {
        get { return end; }
        set { 
            end = value;
            this.length = getLength();
        }
    }
    public int Length
    {
        get { return getLength(); }
    }

    public Road(Vector2Int start, Vector2Int end)
    {
        this.start = start;
        this.end = end;
        this.length = getLength();
    }

    private int getLength()
    {
        this.length = (int) Vector2Int.Distance(Start, End);
        return length;
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
            if (collide(b, this))
            {
                ErrorHandler.instance.reportError("A building is overlapping with a road", this);
                return true;
            }
        }

        return false;
    }
}
