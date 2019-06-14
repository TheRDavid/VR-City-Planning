using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class InsertRoad : MonoBehaviour
{
    public InputField startX, endX;
    public InputField startY, endY;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Insert(){
        int inputStartX = 0;
        int inputStartY = 0;
        int inputEndX = 0;
        int inputEndY = 0;

        if (int.TryParse(startX.text, out inputStartX)
            && int.TryParse(startY.text, out inputStartY)
            && int.TryParse(endX.text, out inputEndX)
            && int.TryParse(endY.text, out inputEndY))
        {
            Road newRoad = new Road(new Vector2Int(inputStartX, inputStartY),
                                    new Vector2Int(inputEndX, inputEndY));

            Municipality.InsertRoad(newRoad);

                startX.text = "";
                startY.text = "";
                endX.text = "";
                endY.text = "";
            }
        else {
            ErrorHandler.instance.reportError("Invalid values were entered for the road. Please enter only integer numbers.");
        }
    }
}
