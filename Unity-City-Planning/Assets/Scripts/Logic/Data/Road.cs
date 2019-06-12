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
}
