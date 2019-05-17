using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Street
{
    private Vector2Int start;
    private Vector2Int end;

    public Vector2Int Start
    {
        get { return start; }
        set { start = value; }
    }
    public Vector2Int End
    {
        get { return end; }
        set { end = value; }
    }
    public float Length { get; set; }

    public Street(Vector2Int start, Vector2Int end)
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
