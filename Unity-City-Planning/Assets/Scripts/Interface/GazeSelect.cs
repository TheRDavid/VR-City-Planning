using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazeSelect : MonoBehaviour
{
    public float sightlength = 100.0f;
    public GameObject selectedObj;
    string lastHitID = "";

    void FixedUpdate()
    {
        RaycastHit seen;
        Ray raydirection = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(raydirection, out seen, sightlength))
        {
            if (seen.transform.gameObject != null)
            {
                if (seen.transform.name.Equals("ground"))
                {
                    int quadX = (int)seen.point.x;
                    int quadZ = (int)seen.point.z;
                    Debug.Log(quadX + "x" + quadZ);
                }
                else
                {
                    MapEntity selection = Municipality.instance.entityByID(seen.transform.name);
                    if (selection != null && !lastHitID.Equals(selection.ID))
                    {
                        Debug.Log("Got a new hit: " + selection.GetType() + ", id: " + selection.ID);
                        lastHitID = selection.ID;
                    }
                }
            }
            else lastHitID = "";
        }
    }
}
