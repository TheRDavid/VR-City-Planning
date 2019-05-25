using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorMarker : ObjectMarker
{

    public Color color;
    
    public ColorMarker(GameObject gameObject, Color color) : base(gameObject)
    {
        this.color = color;
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (Renderer r in gameObject.GetComponentsInChildren<Renderer>())
        {
            r.material.color = color;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
