using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UColorHighlighter : MonoBehaviour
{
    private Color color = Color.red;
    void Start()
    {
    }
    
    void Update()
    {
    }

    internal void setColor(float[] floatParams)
    {
        color = new Color(floatParams[0], floatParams[1], floatParams[2], floatParams[3]);
        foreach (Renderer r in gameObject.GetComponentsInChildren<Renderer>())
        {
            r.material.color = color;
        }
    }
}
