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

            var drawnBuildings = new List<Vector2Int>();

            foreach (Building b in municipality.buildings)
            {
                // another building with the same coordinates exists
                // or the building overlaps with a road
                if (drawnBuildings.Contains(b.Location) || collisionDetected(b,municipality.roads)){
                    //do nothing and go to next building
                    continue;
                }

                GameObject go = Instantiate(buildingPrefab, locationToUnityLocation(b.Location), Quaternion.identity);
                dataObjects.Add(go);

                drawnBuildings.Add(b.Location);

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
                int orientation = 0; //1 = north, 2 = east, 3 = south, 4 = west
                orientation = getOrientation(r);
                int roadlength = 0;

                switch(orientation)
                {
                    case 1:
                        //to North, 
                        roadRotation = Quaternion.Euler(0, 0, 0);
                        roadlength = Mathf.Abs(r.End.y - r.Start.y);
                        for (int z = 0; z < roadlength; z++)
                        {
                            GameObject go = Instantiate(roadPrefab, (startLocation + new Vector3Int(0, 0, z)), roadRotation);
                            dataObjects.Add(go);
                        }
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

                        for (int z = 0; z < roadlength; z++)
                        {
                            GameObject go = Instantiate(roadPrefab, (startLocation - new Vector3Int(0, 0, z)), roadRotation);
                            dataObjects.Add(go);
                        }
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

    private bool collisionDetected(Building b, List<Road> roads){

        foreach (Road r in roads){
            int orientation = getOrientation(r);

            bool collision = false;

            // there is a collision if the building is on the road
            switch (orientation){
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
            if (collision){
                return true;
            }
        }

        return false;
    }
}
