using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityDataWatcher : MonoBehaviour, IDataWatcher
{

    private bool refreshNeeded = false;
    private ConditionList conditionList = new ConditionList();
    private Municipality municipality;
    private List<GameObject> dataObjects = new List<GameObject>();

    public GameObject buildingPrefab;
    public GameObject roadPrefab;

    public void reactToChange(Municipality municipality, ConditionList conditionList)
    {
        this.conditionList = conditionList;
        this.municipality = municipality;
        refreshNeeded = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        buildingPrefab = Resources.Load("Prefabs/Building") as GameObject;
        roadPrefab = Resources.Load("Prefabs/Road") as GameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(refreshNeeded)
        {
            foreach (GameObject gameObject in dataObjects)
            {
                Destroy(gameObject);
            }
            dataObjects.Clear();
            var drawnBuildings = new List<Vector2Int>();
            foreach (Building b in municipality.buildings)
            {
                // another building with the same coordinates exists
                // or the building overlaps with a road
                if (drawnBuildings.Contains(b.Location) || collisionDetected(b, municipality.roads))
                {
                    if (drawnBuildings.Contains(b.Location))
                    {
                        Debug.Log("ERROR: Building already exists");
                    }
                    //do nothing and go to next building
                    continue;
                }

                GameObject go = Instantiate(buildingPrefab, locationToUnityLocation(b.Location), Quaternion.identity);
                
                foreach (Condition c in conditionList.conditions)
                {
                    if (c.isFullfilled(b))
                    {
                        applyVisualization(c.visualizer, go);
                    }
                }

                dataObjects.Add(go);
                drawnBuildings.Add(b.Location);


            }
            foreach (Road r in municipality.roads)
            {
                r.calculateLength();
                Quaternion roadRotation = Quaternion.Euler(0, 0, 0);
                Vector3Int startLocation = locationToUnityLocation(r.Start);
                Vector3 midpointLocation = new Vector3(0,0,0);
                int orientation = 0; //1 = north, 2 = east, 3 = south, 4 = west
                orientation = getOrientation(r);
                float roadlength = 0;
                float midpoint = 0;

                switch (orientation)
                {
                    case 1:
                        //to North

                        // get road orientation, length and midpoint
                        // scaling is done from the middle of objects so the scaled road must be instatiated at it's midpoint
                        roadRotation = Quaternion.Euler(0, 0, 0);
                        roadlength = Mathf.Abs(r.End.y - r.Start.y);
                        midpoint = r.Start.y + (roadlength / 2f);
                        midpointLocation = new Vector3(r.Start.x,0,midpoint);
                        break;
                    case 2:
                        //to East
                        roadRotation = Quaternion.Euler(0, 90, 0);
                        roadlength = Mathf.Abs(r.End.x - r.Start.x);
                        midpoint = r.Start.x + (roadlength / 2f);
                        midpointLocation = new Vector3(midpoint, 0, r.Start.y);
                        break;
                    case 3:
                        //to South
                        roadRotation = Quaternion.Euler(0, 0, 0);
                        roadlength = Mathf.Abs(r.End.y - r.Start.y);
                        midpoint = r.Start.y - (roadlength / 2f);
                        midpointLocation = new Vector3(r.Start.x, 0, midpoint);
                        break;
                    case 4:
                        //to West
                        roadRotation = Quaternion.Euler(0, 90, 0);
                        roadlength = Mathf.Abs(r.End.x - r.Start.x);
                        midpoint = r.Start.x - (roadlength / 2f);
                        midpointLocation = new Vector3(midpoint, 0, r.Start.y);
                        break;
                    default:
                        Debug.Log("ERROR, road start- and endpoints must either shape a horizontal or vertical line");
                        break;
                }

                // make a road and scale it
                GameObject road = Instantiate(roadPrefab, midpointLocation, roadRotation);
                road.transform.localScale += new Vector3(0, 0, roadlength);

                // update texture of road
                road.GetComponentInChildren<MeshRenderer>().material.mainTextureScale = new Vector2(1, roadlength);
                foreach (Condition c in conditionList.conditions)
                {
                    if (c.isFullfilled(r))
                    {
                        applyVisualization(c.visualizer, road);
                    }
                }

                // add the new road to dataObjects
                dataObjects.Add(road);
            }

            refreshNeeded = false;
        }
    }

    private void applyVisualization(Visualizer visualizer, GameObject gameObject)
    {
        switch (visualizer.visualizationName)
        {
            case "ColorHighlighter":
                UColorHighlighter uch = gameObject.AddComponent<UColorHighlighter>();
                uch.setColor(visualizer.floatParams);
                break;
            case "SizePulser":
                USizePulser usp = gameObject.AddComponent<USizePulser>();
                usp.setScalingAttributes(visualizer.floatParams);
                break;
            default: Debug.Log("ERROR: Unknown visualization - " + visualizer.visualizationName); break;
        }
    }

    public static Vector3Int locationToUnityLocation(Vector2Int location)
    {
        return new Vector3Int(location.x, 0, location.y);
    }

    private int getOrientation(Road r) //1 = north, 2 = east, 3 = south, 4 = west
    {
        if (r.Start.x == r.End.x) //North or South
        {
            if (r.Start.y > r.End.y) //South
            {
                return 3;
            }
            else return 1; //North
        }
        else if (r.Start.y == r.End.y) //East or West
        {
            if (r.Start.x > r.End.x) //West
            {
                return 4;
            }
            else return 2; //East
        }
        else return 0;
    }
    private bool collisionDetected(Building b, List<Road> roads)
    {

        foreach (Road r in roads)
        {
            int orientation = getOrientation(r);

            bool collision = false;

            // there is a collision if the building is on the road
            switch (orientation)
            {
                case 1:
                    collision = (r.Start.x == b.location.x && r.Start.y <= b.location.y && b.location.y <= r.End.y);
                    break;
                case 2:
                    collision = (r.Start.y == b.location.y && r.Start.x >= b.location.x && b.location.x >= r.End.x);
                    break;
                case 3:
                    collision = (r.Start.x == b.location.x && r.Start.y >= b.location.y && b.location.y >= r.End.y);
                    break;
                case 4:
                    collision = (r.Start.y == b.location.y && r.Start.x <= b.location.x && b.location.x <= r.End.x);
                    break;
                default:
                    Debug.Log("ERROR");
                    break;
            }
            if (collision)
            {
                Debug.Log("ERROR: Buildings and roads are overlapping");
                return true;
            }
        }

        return false;
    }
}
