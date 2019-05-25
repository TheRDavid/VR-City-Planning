using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[Serializable]
public class Road
{
    public Vector2Int start, end;

    public Vector2Int Start
    {
        get { return start; }
        set { 
            start = value; 
            this.Length = calculateLength(); 
        }
    }
    public Vector2Int End
    {
        get { return end; }
        set { 
            end = value; 
            this.Length = calculateLength(); 
        }
    }
    public float Length { 
        get {
            return this.Length;
        } 
        private set {
            this.Length = value;
        }
    }

    public Road(Vector2Int start, Vector2Int end)
    {
        this.Start = start;
        this.End = end;
        this.Length = calculateLength();
    }

    private float calculateLength()
    {
        return Vector2.Distance(Start, End); 
    }
}
