using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable()]
public class Visualizer
{
    [SerializeField]
    public float[] floatParams;
    [SerializeField]
    public string[] stringParams;
    [SerializeField]
    public int[] intParams;
    [SerializeField]
    public string visualizationName;
        
    [Serializable()]
    public class SizePulser : Visualizer
    {
        public SizePulser(float sizeX, float sizeY, float sizeZ, float duration)
        {
            visualizationName = "SizePulser";
            floatParams = new float[] { sizeX, sizeY, sizeZ, duration };
        }
    }

    [Serializable()]
    public class ColorHighlighter : Visualizer
    {
        public ColorHighlighter(float r, float g, float b, float a)
        {                        
            visualizationName = "ColorHighlighter";
            floatParams = new float[] {r, g, b, a};
        }
    }

}