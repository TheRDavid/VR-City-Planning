using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityDataWatcher : MonoBehaviour, IDataWatcher
{

    private bool refreshNeeded = false;
    private string jsonRules;
    private Municipality municipality;
    private List<GameObject> dataObjects = new List<GameObject>();

    public GameObject buildingPrefab;
    public GameObject roadPrefab;

    public void reactToChange(Municipality municipality, string jsonRules)
    {
        this.jsonRules = jsonRules;
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

            foreach (Building b in municipality.buildings)
            {
                GameObject go = Instantiate(buildingPrefab, locationToUnityLocation(b.Location), Quaternion.identity);
                dataObjects.Add(go);
                


            }

            foreach (Road r in municipality.roads)
            {
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

                // add the new road to dataObjects
                dataObjects.Add(road);
            }

            refreshNeeded = false;
        }
    }

    public Vector3Int locationToUnityLocation(Vector2Int location)
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
}
