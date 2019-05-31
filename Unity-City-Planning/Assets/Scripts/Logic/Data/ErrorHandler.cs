using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ErrorHandler
{
    public abstract void reportError(string msg);
    public abstract void reportError(string msg, MapEntity entity);

    public static ErrorHandler instance;
}
