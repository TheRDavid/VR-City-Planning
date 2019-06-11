using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[Serializable]
public class Road : MapEntity
{
    public Vector2Int start, end;

    public Vector2Int Start
    {
        get { return start; }
        set { 
            start = value; 
        }
    }
    public Vector2Int End
    {
        get { return end; }
        set { 
            end = value; 
        }
    }

    public Road(Vector2Int start, Vector2Int end)
    {
        this.start = start;
        this.end = end;
    }

    public int length()
    {
        return (int) Vector2Int.Distance(Start, End);
    }
}
