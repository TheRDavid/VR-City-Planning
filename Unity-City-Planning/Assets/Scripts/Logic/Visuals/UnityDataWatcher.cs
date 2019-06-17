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

    public GameObject defaultBuildingPrefab;
    public GameObject businessBuildingPrefab;
    public GameObject industrialBuildingPrefab;
    public GameObject roadPrefab;
    public GameObject grassPrefab;

    public void reactToChange(Municipality municipality, ConditionList conditionList)
    {
        this.conditionList = conditionList;
        this.municipality = municipality;
        this.municipality.updateSpaces();
        Municipality.instance = municipality;
        refreshNeeded = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        defaultBuildingPrefab = Resources.Load("Prefabs/Building") as GameObject;
        businessBuildingPrefab = Resources.Load("Prefabs/BusinessFlat") as GameObject;
        industrialBuildingPrefab = Resources.Load("Prefabs/Industrial") as GameObject;
        roadPrefab = Resources.Load("Prefabs/Road") as GameObject;
        grassPrefab = Resources.Load("Prefabs/Grass") as GameObject;
    }

    void assignID(GameObject o, String ID)
    {
        o.name = ID;
        o.transform.name = ID;
        foreach(Transform child in o.transform)
        {
            assignID(child.gameObject, ID);
        }
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

            GameObject greenspace = Instantiate(grassPrefab, new Vector3(municipality.size.x / 2, (float)-0.0001, municipality.size.y / 2), Quaternion.identity);
            greenspace.transform.localScale += new Vector3(municipality.size.x, 0, municipality.size.y);
            assignID(greenspace, "ground");
            var drawnBuildings = new List<Building>();
            foreach (Building b in municipality.buildings)
            {
                // another building with the same coordinates exists
                // or the building overlaps with a road
                if (b.collisionWithBuildings(drawnBuildings) || b.collisionWithRoads(municipality.roads))
                {
                    ErrorHandler.instance.reportError("An entity already exists at this location", b);
                    //do nothing and go to next building
                    continue;
                }

                GameObject go;

                switch (b.Category)
                {
                    case "Business":
                        go = Instantiate(businessBuildingPrefab, locationToUnityLocation(b.Location), Quaternion.identity);
                        break;
                    case "Industrial":
                        go = Instantiate(industrialBuildingPrefab, locationToUnityLocation(b.Location), Quaternion.identity);
                        break;
                    case "Default":
                        go = Instantiate(defaultBuildingPrefab, locationToUnityLocation(b.Location), Quaternion.identity);
                        break;
                    default:
                        go = Instantiate(defaultBuildingPrefab, locationToUnityLocation(b.Location), Quaternion.identity);
                        ErrorHandler.instance.reportError("Invalid building category: " + b.Category); 
                        break;
                }

                foreach (Condition c in conditionList.conditions)
                {
                    if (c.isFullfilled(b))
                    {
                        applyVisualization(c.visualizer, go);
                    }
                }
                
                dataObjects.Add(go);
                assignID(go, b.ID);
                drawnBuildings.Add(b);


            }
            foreach (Road r in municipality.roads)
            {
                Quaternion roadRotation = Quaternion.Euler(0, 0, 0);
                Vector3Int startLocation = locationToUnityLocation(r.Start);
                Vector3 midpointLocation = new Vector3(0,0,0);
                int orientation = 0; //1 = north, 2 = east, 3 = south, 4 = west
                orientation = r.getOrientation();
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
                        ErrorHandler.instance.reportError("Road start- and endpoints must either shape a horizontal or vertical line", r);
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
                assignID(road, r.ID);
                dataObjects.Add(road);
            }

            refreshNeeded = false;
        }
    }

    public void applyVisualization(Visualizer visualizer, GameObject gameObject)
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
            default: ErrorHandler.instance.reportError("Unknown visualization - " + visualizer.visualizationName); break;
        }
    }

    public static Vector3Int locationToUnityLocation(Vector2Int location)
    {
        return new Vector3Int(location.x, 0, location.y);
    }

}
