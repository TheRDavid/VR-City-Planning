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
                int roadlength = 0;
                if (r.Start.x == r.End.x)
                {
                    roadlength = Mathf.Abs(r.End.y - r.Start.y);
                    for (int z = 0; z < roadlength; z++)
                    {
                        GameObject go = Instantiate(roadPrefab, (startLocation + new Vector3Int(0, 0, z)), roadRotation);
                        dataObjects.Add(go);
                    }
                }
                else if (r.Start.y == r.End.y)
                {
                    
                    roadRotation = Quaternion.Euler(0, 90, 0);
                    roadlength = Mathf.Abs(r.End.x - r.Start.x);
                    for (int x = 0; x < roadlength; x++)
                    {
                        GameObject go = Instantiate(roadPrefab, (startLocation + new Vector3Int(x, 0, 0)), roadRotation);
                        dataObjects.Add(go);
                    }
                }
                else
                {
                    Debug.Log("ERROR");
                }
            }

            refreshNeeded = false;
        }
    }

    public Vector3Int locationToUnityLocation(Vector2Int location)
    {
        return new Vector3Int(location.x, 0, location.y);
    }
}
