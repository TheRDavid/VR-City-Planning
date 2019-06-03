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
}
