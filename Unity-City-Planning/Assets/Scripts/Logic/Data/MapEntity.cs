using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MapEntity
{
    [SerializeField]
    public string ID = Guid.NewGuid().ToString();

    public static bool collide(Building b, Road r) {

        int orientation = r.getOrientation();

        // there is a collision if the building is on the road
        switch (orientation)
        {
            case 1:
                return r.Start.x == b.location.x && r.Start.y <= b.location.y && b.location.y <= r.End.y;

            case 2:
                return r.Start.y == b.location.y && r.Start.x <= b.location.x && b.location.x <= r.End.x;

            case 3:
                return r.Start.x == b.location.x && r.Start.y >= b.location.y && b.location.y >= r.End.y;

            case 4:
                return r.Start.y == b.location.y && r.Start.x >= b.location.x && b.location.x >= r.End.x;

            default:
                ErrorHandler.instance.reportError("Invalid orientation of road detected", r);
                return true;
        }

    }

}
