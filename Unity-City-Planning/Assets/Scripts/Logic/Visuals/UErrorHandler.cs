using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UErrorHandler : ErrorHandler
{
    private ErrorUI errorUI;
    private bool init = false;

    public override void reportError(string msg)
    {
        if (!init && (errorUI = GameObject.Find("mainObject").AddComponent<ErrorUI>()) != null)
        {
            init = true;
        }

        //if (init)
         //errorUI.displayMessage("ERROR: " + msg);

    }

    public override void reportError(string msg, MapEntity entity)
    {
        msg += "\nObject reference: " + entity.ID;
        reportError(msg);
    }

}