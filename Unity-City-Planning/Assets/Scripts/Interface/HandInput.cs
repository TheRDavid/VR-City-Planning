using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class HandInput : MonoBehaviour
{
    private bool triggerPulled = false;

    void Start()
    {
        SteamVR_Actions.default_trigger_pull.AddOnChangeListener(handleTriggerPull, SteamVR_Input_Sources.Any);
    }

    private void handleTriggerPull(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
    {
        triggerPulled = !triggerPulled;
        ErrorHandler.instance.reportError("Trigger " + triggerPulled);
    }

    void Update()
    {
    }
    
}
