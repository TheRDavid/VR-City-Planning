using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class USizePulser : MonoBehaviour
{

    private Vector3 size = Vector3.one, diff = Vector3.zero, defaultSize = Vector3.one;
    private float duration = 1;
    private bool direction = true; // 1 -> towards size, -1 -> towards default
    private float startTime;
    
    void Start()
    {
        startTime = Time.time;   
    }
    
    void Update()
    {
        if ((Time.time - startTime >= duration))
        {
            direction = !direction;
            startTime = Time.time;
        }
        Vector3 currentSize = Vector3.one;
        if (direction)
            currentSize = defaultSize + diff * (Time.time - startTime) / duration;
        else
            currentSize = size - diff * (Time.time - startTime) / duration;
        gameObject.transform.localScale = currentSize;
    }

    internal void setScalingAttributes(float[] floatParams)
    {
        size = new Vector3(floatParams[0], floatParams[1], floatParams[2]);
        duration = floatParams[3];
        defaultSize = gameObject.transform.localScale;
        diff = size - defaultSize;
    }
}
