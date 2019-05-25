using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityDataWatcher : MonoBehaviour, IDataWatcher
{

    private bool refreshNeeded = false;
    private Municipality municipality;
    private List<GameObject> dataObjects = new List<GameObject>();

    public GameObject buildingPrefab;
    public GameObject roadPrefab;

    public void reactToChange(Municipality municipality)
    {
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

                if (b.Consumption > 3)
                {
                    foreach (Renderer r in go.GetComponentsInChildren<Renderer>())
                    {
                        r.material.color = Color.red;
                    }
                } else
                {
                    foreach (Renderer r in go.GetComponentsInChildren<Renderer>())
                    {
                        r.material.color = Color.white;

                    }
                }


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

                        // make a road, scale it, then add it to dataObjects
                        GameObject northRoad = Instantiate(roadPrefab, midpointLocation, roadRotation);
                        northRoad.transform.localScale += new Vector3(0, 0, roadlength);
                        dataObjects.Add(northRoad);
                        break;
                    case 2:
                        //to East
                        roadRotation = Quaternion.Euler(0, 90, 0);
                        roadlength = Mathf.Abs(r.End.x - r.Start.x);
                        for (int x = 0; x < roadlength; x++)
                        {
                            GameObject go = Instantiate(roadPrefab, (startLocation + new Vector3Int(x, 0, 0)), roadRotation);
                            dataObjects.Add(go);
                        }
                        break;
                    case 3:
                        //to South
                        roadRotation = Quaternion.Euler(0, 0, 0);
                        roadlength = Mathf.Abs(r.End.y - r.Start.y);
                        midpoint = r.Start.y - (roadlength / 2f);
                        midpointLocation = new Vector3(r.Start.x, 0, midpoint);
                        
                        GameObject southRoad = Instantiate(roadPrefab, midpointLocation, roadRotation);
                        southRoad.transform.localScale += new Vector3(0, 0, roadlength);
                        dataObjects.Add(southRoad);
                        break;
                    case 4:
                        //to West
                        roadRotation = Quaternion.Euler(0, 90, 0);
                        roadlength = Mathf.Abs(r.End.x - r.Start.x);
                        for (int x = 0; x < roadlength; x++)
                        {
                            GameObject go = Instantiate(roadPrefab, (startLocation - new Vector3Int(x, 0, 0)), roadRotation);
                            dataObjects.Add(go);
                        }
                        break;
                    default:
                        Debug.Log("ERROR");
                        break;
                }
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
