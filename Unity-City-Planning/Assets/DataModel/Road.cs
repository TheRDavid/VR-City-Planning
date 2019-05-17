using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Road
{
    private Vector2Int start;
    private Vector2Int end;

    public Vector2Int Start
    {
        get { return start; }
        set { start = value; calculateLength(); }
    }
    public Vector2Int End
    {
        get { return end; }
        set { end = value; calculateLength(); }
    }
    public float Length { get; set; }

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
