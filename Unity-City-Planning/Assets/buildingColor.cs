using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buildingColor : MonoBehaviour
{

    Color groenig = new Color32(10, 200, 0, 0);


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       Renderer rend = gameObject.GetComponentsInChildren<Renderer>()[0];
        rend.material.shader = Shader.Find("_Color");
        rend.material.SetColor("_Color", groenig);
    }
}
