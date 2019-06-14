using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazeSelect : MonoBehaviour
{
    public float sightlength = 100.0f;
    public MapEntity selectedObj;
    string lastHitID = "";
    public Vector3Int quadrant;

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
                    quadrant = new Vector3Int(quadX, 0, quadZ);

                    if (Input.GetKeyUp(KeyCode.Space))
                    {
                        switch(MenuSelect.instance.index)
                        {
                            case 1:
                                // make a default building
                                break;
                            case 2:
                                // make a business
                                break;
                            case 3:
                                // factory
                                break;
                            case 4:
                                // road
                                break;
                            default:
                                break;
                        }
                    }
                }
                else
                {
                    MapEntity selection = Municipality.instance.entityByID(seen.transform.name);
                    if (selection != null && !lastHitID.Equals(selection.ID))
                    {
                        lastHitID = selection.ID;
                        selectedObj = selection;
                    }
                }
            }
            else
            {
                lastHitID = "";
                selectedObj = null;
            }
        }
    }
}
