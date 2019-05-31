using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class InputFieldExample : MonoBehaviour
{
    public InputField xCoord;
    public InputField yCoord;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ButtonClick()
    {
        int x = 0;
        int y = 0;

        if (int.TryParse(xCoord.text, out x) && int.TryParse(yCoord.text, out y))
        {
            // parsing was successful
            GameObject buildingPrefab = Resources.Load("Prefabs/Building") as GameObject;
            Instantiate(buildingPrefab, new Vector3Int(x, 0, y), Quaternion.identity);
        }
        else {
            Debug.Log("Invalid values entered for coordinates");
        }


    }
}
