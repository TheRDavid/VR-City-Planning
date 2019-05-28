using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[Serializable]
public class Road
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
}
