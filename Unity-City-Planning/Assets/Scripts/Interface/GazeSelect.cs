using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazeSelect : MonoBehaviour
{
    public float sightlength = 100.0f;
    public MapEntity selectedObj;
    string lastHitID = "";
    public Vector3Int quadrant;
    private Vector3 bannedPosition = new Vector3(0, -10, 0);
    private GameObject selectionCube;
    public static GazeSelect instance;
    public GameObject selectedGameObject = null;

    private void Start()
    {
        instance = this;
        selectionCube = GameObject.Find("SelectionCube");    
    }

    void FixedUpdate()
    {
        RaycastHit seen;
        Ray raydirection = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(raydirection, out seen, sightlength))
        {
            if (seen.transform.gameObject != null)
            {
                int quadX = (int)seen.point.x;
                int quadZ = (int)seen.point.z;
                quadrant = new Vector3Int(quadX, 0, quadZ);

                selectionCube.transform.position = quadrant;

                if (!MenuSelect.instance.isActiveAndEnabled) return;

                if (seen.transform.name.Equals("ground"))
                {
                    if (Input.GetKeyUp(KeyCode.Space))
                    {
                        MapEntity entity = null;
                        switch (MenuSelect.instance.index)
                        {
                            case 1:
                                entity = new Building(10, 25, new Vector2Int(quadX, quadZ), Vector3Int.one, "Default");
                                break;
                            case 2:
                                entity = new Building(30, 0, new Vector2Int(quadX, quadZ), Vector3Int.one, "Business");
                                // make a business
                                break;
                            case 3:
                                entity = new Building(150, 0, new Vector2Int(quadX, quadZ), Vector3Int.one, "Industrial");
                                break;
                            case 4:
                                // todo
                                break;
                            default:
                                break;
                        }
                        Municipality.InsertMapEntity(entity);
                    }
                }
                else
                {
                    MapEntity selection = Municipality.instance.entityByID(seen.transform.name);

                    if (selection == null) return;


                    if (typeof(Building).Equals(selection.GetType()))
                    {
                        quadX = (int)seen.transform.position.x;
                        quadZ = (int)seen.transform.position.z;
                        quadrant = new Vector3Int(quadX, 0, quadZ);
                        selectionCube.transform.position = quadrant;
                    }
                    if (selection != null && !lastHitID.Equals(selection.ID))
                    {
                        lastHitID = selection.ID;
                        selectedObj = selection;
                    }

                    if (Input.GetKeyUp(KeyCode.Space))
                    {
                        selectedGameObject = seen.transform.parent.gameObject;

                        if (typeof(Building).Equals(selection.GetType()))
                        {
                            Building b = selection as Building;
                            EditBuilding.instance.Show(b);
                        }
                        else if (typeof(Road).Equals(selection.GetType()))
                        {
                            Road b = selection as Road;
                        }
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
