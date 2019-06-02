using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MapEntity
{
    [SerializeField]
    public string ID = Guid.NewGuid().ToString();
}
